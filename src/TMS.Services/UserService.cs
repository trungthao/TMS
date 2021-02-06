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

namespace TMS.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly JwtSettings _jwtSettings;

        public UserService(IUserRepository repository,
            IOptions<JwtSettings> jwtSettings,
            IConfigService configService) : base(repository, configService)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authModel, string ipAddress)
        {
            var user = new User()
            {
                Id = 1,
                Username = "trungthao",
                Password = "tlynmm",
                EntityState = Enumeartions.EntityState.Update,
                RefreshTokens = new List<RefreshToken>()
            };

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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
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
    }
}
