using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Library.Extensions
{
    public static class Extensions
    {
        public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            } 
            else
            {
                dic.Add(key, value);
            }

            return dic;
        }

        public static void SetValueByPropertyName(this object obj, string propName, object value)
        {
            var prop = obj.GetType().GetProperty(propName);
            prop.SetValue(obj, value);
        }


        public static object GetValueByPropertyName(this object obj, string propName)
        {
            var prop = obj.GetType().GetProperty(propName);
            return prop.GetValue(obj);
        }
    }
}
