using System.Collections.Generic;
using System.Linq;

namespace Jexpr.Common
{
    public static class DynamicExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var type = obj.GetType();
            var props = type.GetProperties();

            return props.ToDictionary(property => property.Name, property => property.GetValue(obj, null));
        }
    }
}