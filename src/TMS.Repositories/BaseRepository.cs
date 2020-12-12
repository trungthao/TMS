using System.Data;
using System.Threading.Tasks;
using TMS.Domain.Constants;
using TMS.Domain.Entities;
using TMS.Domain.Models;
using TMS.Domain.Repositories;
using TMS.Library.Interfaces;

namespace TMS.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly IDatabaseService _databaseService;

        public BaseRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> SaveEntity(BaseEntity entity, IDbConnection conn = null, IDbTransaction trans = null)
        {
            if (entity.EntityState != Enumeartions.EntityState.None)
            {
                return await DoSaveEntity(entity, conn, trans);
            }

            return -1;
        }

        protected async Task<int> DoSaveEntity(BaseEntity entity, IDbConnection conn = null, IDbTransaction trans = null)
        {
            if (entity.EntityState == Enumeartions.EntityState.Insert)
            {
                var id = await _databaseService.ExecuteScalaAsync<int>("Proc_InsertTest", entity, conn, trans);
                return id;
            }

            return -1;
        }
    }
}