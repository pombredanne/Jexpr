﻿using System;

namespace Jexpr.Core.Impl
{
    public class NullLogger : ILogger
    {
        public void Debug(string message)
        {
            
        }

        public void Debug(string message, Exception exception)
        {
            
        }

        public void Error(string message)
        {
          
        }

        public void Error(string message, Exception exception)
        {
            
        }

        public void Info(string message)
        {
           
        }

        public void Info(string message, Exception exception)
        {
           
        }

        public void Warn(string message)
        {
           
        }

        public void Warn(string message, Exception exception)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Fatal(string message, Exception exception)
        {
        }
    }
}