using System.Threading.Tasks;
using TMS.Domain.Entities;

namespace TMS.Domain.Repositories
{
    public interface IAccountRepository : IBaseRepository
    {
        Task<Account> GetAccountByEmail(string email);
    }
}