using System;

namespace Jexpr.Core.Impl
{
    public class DatetimeHelper : IDatetimeHelper
    {
        public string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}