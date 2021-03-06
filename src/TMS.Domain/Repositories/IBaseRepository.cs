using System.Data;
using System.Threading.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Models;

namespace TMS.Domain.Repositories
{
    public interface IBaseRepository
    {
        public Task<bool> SaveEntity(BaseEntity entity, IDbConnection conn = null, IDbTransaction trans = null);

        public Task<T> GetEntityById<T>(int id);
    }
}