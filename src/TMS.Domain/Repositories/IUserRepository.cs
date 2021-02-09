using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Domain.Entities;

namespace TMS.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository
    {
        Task<User> GetUserByVerifyToken(string token);

        Task VerifyToken(User user);

        Task<User> GetUserByEmail(string email);

        Task<RefreshToken> GetRefreshTokenByToken(string token);

        Task DeleteOldRefreshTokens(int userId, DateTime expireDate);
    }
}
