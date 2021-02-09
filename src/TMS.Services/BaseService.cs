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
using TMS.Library.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;

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
            var isValid = ValidateSaveEntity(entity);
            if (isValid)
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

                        await DoSaveEntity(entity, response, conn, trans);

                        trans.Commit();
                    }
                }
                catch (Exception)
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
            AfterSaveEntity(entity, response);

            return response;
        }

        private bool ValidateSaveEntity(BaseEntity entity)
        {
            return true;
        }

        protected void BeforeSaveEntity(BaseEntity entity)
        {
        }

        private async Task<bool> DoSaveEntity(BaseEntity entity, SaveBaseEntityResponse response, IDbConnection conn, IDbTransaction trans)
        {
            BeforeSaveEntityWithTransaction(entity, response, conn, trans);
            var saveMasterSuccess = await _repository.SaveEntity(entity, conn, trans);
            AfterSaveEntityWithTransaction(entity, response, conn, trans);

            var hasAtLeastOneDetailFail = false; // có ít nhất 1 detail bị save fail
            if (saveMasterSuccess)
            {
                var details = entity.GetDetails();
                if (details != null && details.Count > 0)
                {
                    var masterId = entity.GetPrimaryKeyValue();
                    foreach (var lstDetailEntity in details)
                    {
                        if (lstDetailEntity != null && lstDetailEntity.Count > 0)
                        {
                            var firstDetailEntity = lstDetailEntity[0];
                            foreach (var detailEntity in lstDetailEntity)
                            {
                                var foreignKeyName = detailEntity.GetForeignKeyName();
                                if (!string.IsNullOrWhiteSpace(foreignKeyName))
                                {
                                    detailEntity.SetValueByPropertyName(foreignKeyName, masterId);
                                    var saveDetailSuccess = await DoSaveEntity(detailEntity, response, conn, trans);
                                    if (!saveDetailSuccess)
                                    {
                                        hasAtLeastOneDetailFail = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return saveMasterSuccess && !hasAtLeastOneDetailFail;
        }

        private void AfterSaveSuccess(BaseEntity entity, SaveBaseEntityResponse response)
        {
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

        /// <summary>
        /// Lấy entity theo id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetEntityById<T>(int id)
        {
            return await _repository.GetEntityById<T>(id);
        }
    }
}