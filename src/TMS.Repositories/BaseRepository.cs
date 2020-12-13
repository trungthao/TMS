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
            var storeName = GetStoreName(entity);
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

        protected string GetStoreName(BaseEntity entity)
        {
            var type = entity.GetType();
            var entityName = type.Name;
            string storeNameTemplate = string.Empty;

            switch (entity.EntityState)
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