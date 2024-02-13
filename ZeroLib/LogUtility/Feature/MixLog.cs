/****************************************************
  文件：MixLog.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 14:33:34
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Zero.Utility
{
    public class MixLog : BaseLog
    {
        private List<ILogger> _logList;
        
        public MixLog(Type type, string prefix = "") : base(prefix)
        {
            _logList = new List<ILogger>(2);
            _logList.Add(UnityLog.Instance);
            _logList.Add(new Log4netLog(type));
        }

        public override void Debug(object message)
        {
            foreach (var logger in _logList)
            {
                logger.Debug(prefix + message);
            }
        }

        public override void Info(object message)
        {
            foreach (var logger in _logList)
            {
                logger.Info(prefix + message);
            }
        }

        public override void Warn(object message)
        {
            foreach (var logger in _logList)
            {
                logger.Warn(prefix + message);
            }
        }

        public override void Error(object message)
        {
            foreach (var logger in _logList)
            {
                logger.Error(prefix + message);
            }
        }

        public override void Fatal(object message)
        {
            foreach (var logger in _logList)
            {
                logger.Fatal(prefix + message);
            }
        }

        public override void SetLevelLimit(LogLevel level)
        {
            base.SetLevelLimit(level);
            foreach (var logger in _logList)
            {
                logger.SetLevelLimit(level);
            }
        }
    }
}
