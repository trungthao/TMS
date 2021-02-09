using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;
using TMS.Domain.Constants;
using TMS.Domain.Entities;
using TMS.Domain.Repositories;
using TMS.Library.Interfaces;
using TMS.Library.Utilities;
using TMS.Repositories.Constants;

namespace TMS.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly IDatabaseService _databaseService;

        public BaseRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Task<T> GetEntityById<T>(int id)
        {
            var storeName = GetStoreName(Enumeartions.StoreType.SelectById, typeof(T));

            return _databaseService.GetObjectAsync<T>(storeName, new { id });
        }

        /// <summary>
        /// Thực hiện thêm/sửa/xóa đối tượng
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public async Task<bool> SaveEntity(BaseEntity entity, IDbConnection conn = null, IDbTransaction trans = null)
        {
            if (entity.EntityState != Enumeartions.EntityState.None)
            {
                return await DoSaveEntity(entity, conn, trans);
            }

            return false;
        }

        /// <summary>
        /// Thực hiện thêm/sửa/xóa đối tượng
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        protected async Task<bool> DoSaveEntity(BaseEntity entity, IDbConnection conn = null, IDbTransaction trans = null)
        {
            var storeName = GetStoreName(Enumeartions.StoreType.Save, entity.GetType(), entity.EntityState);
            var param = ConvertUtils.ConvertObjectToDictionaryForStore(entity);

            if (entity.GetTypePrimaryKey() == typeof(int))
            {
                if (entity.EntityState == Enumeartions.EntityState.Insert)
                {
                    var primaryKeyValue = await _databaseService.ExecuteScalaAsync<int>(storeName, param, conn, trans);
                    entity.SetPrimaryKeyValue(primaryKeyValue);
                    return true;
                }
                else
                {
                    var numberOfRowAffected = await _databaseService.ExecuteNonQueryAsync(storeName, param, conn, trans);
                    return numberOfRowAffected > 0;
                }
            }

            return false;
        }

        protected string GetStoreName(Enumeartions.StoreType storeType, Type entityType, Enumeartions.EntityState entityState = Enumeartions.EntityState.None)
        {
            var entityName = entityType.Name;
            string storeNameTemplate = string.Empty;

            switch (storeType)
            {
                case Enumeartions.StoreType.Save:
                    storeNameTemplate = GetSaveStoreName(entityName, entityState);
                    break;
                case Enumeartions.StoreType.SelectById:
                    storeNameTemplate = GetSelectByIdStoreName(entityName);
                    break;
            }

            return storeNameTemplate;
        }

        protected string GetSelectByIdStoreName(string entityName)
        {
            var storeNameTemplate = DatabaseConstants.mscTemplateStoreNameGetObjectById;
            return string.Format(storeNameTemplate, entityName);
        }

        protected string GetSaveStoreName(string entityName, Enumeartions.EntityState entityState)
        {
            string storeNameTemplate = string.Empty;

            switch (entityState)
            {
                case Enumeartions.EntityState.Insert:

                    storeNameTemplate = DatabaseConstants.mscTemplateStoreNameInsert;
                    break;
                case Enumeartions.EntityState.Update:
                    storeNameTemplate = DatabaseConstants.mscTemplateStoreNameUpdate;
                    break;
                case Enumeartions.EntityState.Delete:
                    storeNameTemplate = DatabaseConstants.mscTemplateStoreNameDeleteById;
                    break;
            }

            return string.Format(storeNameTemplate, entityName);
        }
    }
}