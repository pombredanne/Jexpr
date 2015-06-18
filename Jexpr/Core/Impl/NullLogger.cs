using System;

namespace Jexpr.Core.Impl
{
    public class NullLogger : ILogger
    {
        public void Log(string message)
        {
           
        }

        public void Log(string message, Exception exception)
        {
           
        }
    }
}