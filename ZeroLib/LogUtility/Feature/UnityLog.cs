/****************************************************
  文件：UnityLog.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/22 16:07:56
  功能：
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using log4net;
using UnityEngine;

namespace Zero.Utility
{
    public class UnityLog : BaseLog
    {
        public static UnityLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UnityLog();
                }
                return _instance;
            }
        }
        private static UnityLog _instance;
        public UnityLog(string prefix = "") : base(prefix) { }

        public override void Debug(object message)
        {
            if (CheckLevelLimit(LogLevel.DEBUG))
                UnityEngine.Debug.Log(prefix + message);
        }

        public override void Info(object message)
        {
            if (CheckLevelLimit(LogLevel.INFO))
                UnityEngine.Debug.Log(prefix + message);
        }

        public override void Warn(object message)
        {
            if (CheckLevelLimit(LogLevel.WARN))
                UnityEngine.Debug.LogWarning(prefix + message);
        }

        public override void Error(object message)
        {
            if (CheckLevelLimit(LogLevel.ERROR))
                UnityEngine.Debug.LogError(prefix + message);
        }

        public override void Fatal(object message)
        {
            if (CheckLevelLimit(LogLevel.FATAL))
                UnityEngine.Debug.LogError(prefix + message);
        }
    }
}
