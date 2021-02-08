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

namespace TMS.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository repository,
            IEmailService emailService,
            IOptions<JwtSettings> jwtSettings,
            IConfigService configService) : base(repository, configService)
        {
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authModel, string ipAddress)
        {
            var user = new User();

            if (user == null)
            {
                return null;
            }

            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);
            await SaveEntity(user);

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
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
            _emailService.Send(to: user.Email, 
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                    <p>Thanks for registering!
                    {message}
                "
            );
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
                    //new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays),
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
    }
}
