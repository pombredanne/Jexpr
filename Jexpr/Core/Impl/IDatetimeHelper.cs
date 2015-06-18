using System;

namespace Jexpr.Core.Impl
{
    public interface IDatetimeHelper
    {
        string GetTimestamp(DateTime value);
        DateTime Now();
    }
}