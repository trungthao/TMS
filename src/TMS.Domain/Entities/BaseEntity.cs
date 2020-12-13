using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TMS.Library.Attributes;
using TMS.Domain.Constants;
using TMS.Library.Extensions;

namespace TMS.Domain.Entities
{
    public abstract class BaseEntity
    {
        #region Properties
        [NotColumn]
        public Enumeartions.EntityState EntityState { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Gán giá trị cho khóa chính
        /// </summary>
        /// <param name="value">giá trị sẽ gán cho khóa chính</param>
        /// <returns>
        /// true: gán thành công
        /// false: không tìm thấy khóa chính để gán
        /// </returns>
        public bool SetPrimaryKeyValue(object value)
        {
            var keyProp = GetPrimaryKeyProperty();
            if (keyProp != null)
            {
                keyProp.SetValue(this, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// lấy giá trị của primary key
        /// </summary>
        /// <returns></returns>
        public object GetPrimaryKeyValue()
        {
            var keyProp = GetPrimaryKeyProperty();
            if (keyProp != null)
            {
                return keyProp.GetValue(this);
            }

            return null;
        }

        /// <summary>
        /// Lấy property là key của class này
        /// </summary>
        /// <returns></returns>
        public PropertyInfo GetPrimaryKeyProperty()
        {
            var properties = this.GetType().GetProperties();
            if (properties != null)
            {
                var keyProp = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true) != null);
                if (keyProp != null)
                {
                    return keyProp;
                }
            }

            return null;
        }

        /// <summary>
        /// Lấy type của khóa chính
        /// </summary>
        /// <returns></returns>
        public Type GetTypePrimaryKey()
        {
            var primaryKeyProp = GetPrimaryKeyProperty();
            return primaryKeyProp.PropertyType;
        }

        public virtual string GetForeignKeyName()
        {
            return null;
        }

        /// <summary>
        /// Lấy danh sách detail
        /// </summary>
        /// <returns></returns>
        public virtual List<List<BaseEntity>> GetDetails()
        {
            return null;
        }
        #endregion
    }
}