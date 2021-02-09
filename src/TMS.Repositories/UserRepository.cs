using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Repositories;
using TMS.Library.Interfaces;

namespace TMS.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDatabaseService databaseService) : base(databaseService)
        {

        }

        /// <summary>
        /// Lấy thông tin User theo VefiryToken truyền vào
        /// </summary>
        /// <param name="token">VefiryToken</param>
        /// <returns>thông tin user</returns>
        public async Task<User> GetUserByVerifyToken(string token)
        {
            return await _databaseService.GetObjectAsync<User>("Proc_GetUserByVefiryToken", new { token });
        }

        /// <summary>
        /// Thực hiện cập nhật tgian vefiry token của user và update token về null
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task VerifyToken(User user)
        {
            await _databaseService.ExecuteNonQueryAsync("Proc_VerifyToken", new
            {
                p_UserId = user.UserId,
                p_VerifiedDate = user.VerifiedDate
            });
        }

        /// <summary>
        /// Lấy thông tin user bằng email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string email)
        {
            return await _databaseService.GetObjectAsync<User>("Proc_GetUserByEmail", new { p_Email = email });
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string token)
        {
            return await _databaseService.GetObjectAsync<RefreshToken>("Proc_GetRefreshTokenByToken", new { p_token = token });
        }

        public async Task DeleteOldRefreshTokens(int userId, DateTime expireDate)
        {
            await _databaseService.ExecuteNonQueryAsync("Proc_DeleteOldRefreshToken", new { p_userId = userId, p_expireDate = expireDate });
        }
    }
}
