using System;
using System.Collections.Generic;
using System.Text;

namespace Zero.Utility
{
    public abstract class BaseLog : ILogger
    {
        public string prefix;
        public LogLevel limitLevel;

        public BaseLog(string prefix = "")
        {
            this.prefix = prefix;
        }

        public abstract void Debug(object message);
        public abstract void Error(object message);
        public abstract void Fatal(object message);
        public abstract void Info(object message);
        public abstract void Warn(object message);
        public void Log(object message, LogLevel level)
        {
            switch(level)
            {
                case LogLevel.DEBUG:
                    Debug(message);
                    break;
                case LogLevel.INFO:
                    Info(message);
                    break;
                case LogLevel.WARN:
                    Warn(message);
                    break;
                case LogLevel.ERROR:
                    Error(message);
                    break;
                case LogLevel.FATAL:
                    Fatal(message);
                    break;
            }
        }
        public virtual void SetLevelLimit(LogLevel level)
        {
            limitLevel = level;
        }
        protected bool CheckLevelLimit(LogLevel currentLevel)
        {
            return currentLevel >= limitLevel;
        }
    }
}
