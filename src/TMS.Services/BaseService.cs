using System.Threading;
using System;
using System.Threading.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Models;
using TMS.Domain.Repositories;
using TMS.Domain.Services;
using MySql.Data.MySqlClient;
using TMS.Library.Interfaces;
using System.Data;

namespace TMS.Services
{
    public class BaseService : IBaseService
    {
        protected readonly IBaseRepository _repository;
        protected readonly IConfigService _configService;
        public BaseService(IBaseRepository repository, IConfigService configService)
        {
            _repository = repository;
            _configService = configService;
        }
        public async Task<SaveBaseEntityResponse> SaveEntity(BaseEntity entity)
        {
            SaveBaseEntityResponse response = new SaveBaseEntityResponse();
            BeforeSaveEntity(entity);
            await DoSaveEntity(entity, response);
            AfterSaveEntity(entity, response);

            return response;
        }

        protected void BeforeSaveEntity(BaseEntity entity)
        {
        }

        private async Task DoSaveEntity(BaseEntity entity, SaveBaseEntityResponse response)
        {
            var connectionString = _configService.GetConnectionStrings();
            MySqlConnection conn = null;
            IDbTransaction trans = null;
            try
            {
                conn = new MySqlConnection(connectionString);
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    trans = await conn.BeginTransactionAsync();

                    BeforeSaveEntityWithTransaction(entity, response, conn, trans);
                    response.Data = await _repository.SaveEntity(entity, conn, trans);
                    AfterSaveEntityWithTransaction(entity, response, conn, trans);

                    trans.Commit();
                }
            }
            catch (System.Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        await conn.CloseAsync();
                    }

                    await conn.DisposeAsync();
                }

                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }

        private void AfterSaveEntityWithTransaction(BaseEntity entity, SaveBaseEntityResponse response, IDbConnection conn, IDbTransaction trans)
        {
        }

        private void BeforeSaveEntityWithTransaction(BaseEntity entity, SaveBaseEntityResponse response, IDbConnection conn, IDbTransaction trans)
        {
        }

        private void AfterSaveEntity(BaseEntity entity, SaveBaseEntityResponse response)
        {
        }
    }
}