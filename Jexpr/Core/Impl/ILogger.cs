using System;

namespace Jexpr.Core.Impl
{
    public interface ILogger
    {
        void Log(string message);
        void Log(string message, Exception exception);
    }
}