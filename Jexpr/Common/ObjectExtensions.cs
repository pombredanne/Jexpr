using System;
using System.Collections.Generic;
using System.Linq;

namespace Jexpr.Common
{
    public static class ObjectExtensions
    {
        public static bool IsNumeric(this object expr)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(expr),
                System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

            return isNum;
        }

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var type = obj.GetType();
            var props = type.GetProperties();

            return props.ToDictionary(property => property.Name, property => property.GetValue(obj, null));
        }
    }
}