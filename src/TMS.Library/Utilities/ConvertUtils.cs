using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMS.Library.Attributes;
using TMS.Library.Extensions;

namespace TMS.Library.Utilities
{
    public class ConvertUtils
    {
        public static Dictionary<string, object> ConvertObjectToDictionaryForStore(object param)
        {
            var dicParam = new Dictionary<string, object>();
            var objType = param.GetType();
            var properties = objType.GetProperties();
            if (properties != null)
            {
                properties = properties.Where(prop => prop.GetCustomAttribute(typeof(NotColumnAttribute), true) == null).ToArray();
                foreach (var prop in properties)
                {
                    var propName = "$" + prop.Name;
                    object propValue = prop.GetValue(param);
                    if (prop.PropertyType == typeof(bool))
                    {
                        propValue = Convert.ToInt32(propValue);
                    }
                    dicParam.AddOrUpdate(propName, propValue);
                }
            }

            return dicParam;
        }
    }
}
