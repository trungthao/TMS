using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Models;
using TMS.Domain.Repositories;
using TMS.Domain.Services;
using TMS.Library.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TMS.Domain.Constants;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;
using AutoMapper;

namespace TMS.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository,
            IEmailService emailService,
            IOptions<JwtSettings> jwtSettings,
            IMapper mapper,
            IConfigService configService) : base(repository, configService)
        {
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
            _userRepository = repository;
            _mapper = mapper;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authModel, string ipAddress)
        {
            var user = await _userRepository.GetUserByEmail(authModel.Email);

            if (user == null || !user.IsVerified || !BC.Verify(authModel.Password, user.PasswordHash))
            {
                return null;
            }

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user, ipAddress);
            await SaveEntity(refreshToken);

            return new AuthenticateResponse(){
                UserId = user.UserId,
                Email = user.Email,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task Register(User userEntity, string origin)
        {
            //SendAlreadyRegisteredEmail(userEntity.Email, origin);
            userEntity.VerificationToken = RandomTokenString();
            userEntity.PasswordHash = BC.HashPassword(userEntity.Password);
            userEntity.EntityState = Enumeartions.EntityState.Insert;

            await SaveEntity(userEntity);

            SendVerificationEmail(userEntity, origin);
        }

        private void SendAlreadyRegisteredEmail(string email, string origin)
        {
            string message = string.Empty;
            if (!string.IsNullOrEmpty(origin))
            {
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            }
            else
            {
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";
            }
        }

        private void SendVerificationEmail(User user, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/account/verify-email?token={user.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{user.VerificationToken}</code></p>";
            }

            // send email code here
            //_emailService.Send(to: user.Email, 
            //    subject: "Sign-up Verification API - Verify Email",
            //    html: $@"<h4>Verify Email</h4>
            //        <p>Thanks for registering!
            //        {message}
            //    "
            //);
        }

        /// <summary>
        /// sinh ra chuỗi token từ userInfo và secretKey được config trong jwtSetting
        /// </summary>
        /// <param name="user">thông tin của user đang đăng nhập</param>
        /// <returns>chỗi jwt token</returns>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(User user, string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    UserId = user.UserId,
                    Token = Convert.ToBase64String(randomBytes),
                    ExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays),
                    CreatedDate = DateTime.UtcNow,
                    CreatedByIp = ipAddress,
                    EntityState = Enumeartions.EntityState.Insert
                };
            }
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        /// <summary>
        /// Xác thực email của người dùng
        /// </summary>
        /// <param name="token"></param>
        public async Task VerifyEmail(string token)
        {
            var user = await GetUserByVerifyToken(token);
            if (user == null)
            {
                throw new NullReferenceException();
            }

            user.VerifiedDate = DateTime.UtcNow;

            await _userRepository.VerifyToken(user);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var refreshToken = await _userRepository.GetRefreshTokenByToken(token);
            if (refreshToken == null) throw new NullReferenceException();
            var user = await GetEntityById<User>(refreshToken.UserId);
            if (user == null) throw new NullReferenceException();

            var newRefreshToken = GenerateRefreshToken(user, ipAddress);
            refreshToken.RevokedDate = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            refreshToken.EntityState = Enumeartions.EntityState.Update;

            await SaveEntity(refreshToken);
            await SaveEntity(newRefreshToken);
            await RemoveOldRefreshTokens(user);

            var jwtToken = GenerateJwtToken(user);
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;

            return response;
        }

        /// <summary>
        /// Lấy thông tin User theo VefiryToken truyền vào
        /// </summary>
        /// <param name="token">VefiryToken</param>
        /// <returns>thông tin user</returns>
        private async Task<User> GetUserByVerifyToken(string token)
        {
            return await _userRepository.GetUserByVerifyToken(token);
        }

        private async Task RemoveOldRefreshTokens(User user)
        {
            var expireDate = DateTime.UtcNow;
            await _userRepository.DeleteOldRefreshTokens(user.UserId, expireDate);
        }
    }
}
