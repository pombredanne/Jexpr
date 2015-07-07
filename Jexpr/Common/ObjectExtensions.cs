using System;

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
    }
}