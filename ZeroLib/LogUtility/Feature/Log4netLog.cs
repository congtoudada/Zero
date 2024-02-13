/****************************************************
  文件：Log4netLog.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/22 14:14:58
  功能：Nothing
*****************************************************/
using System;
using System.Diagnostics;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using UnityEngine;

namespace Zero.Utility
{
    public class Log4netLog : BaseLog
    {
        private ILog _logger;
        private Type type;

        public static void Init(string configPath, string outputPath)
        {
            //配置文件内获取
            GlobalContext.Properties["ApplicationLogPath"] = outputPath;
            FileInfo file = new System.IO.FileInfo(configPath); //获取log4net配置文件
            XmlConfigurator.ConfigureAndWatch(file); //加载log4net配置文件
            Application.quitting += () =>
            {
                LogManager.ShutdownRepository();
                LogManager.Shutdown();
            };
        }

        public Log4netLog(Type type, string prefix = "") : base(prefix)
        {
            this.type = type;
            _logger = LogManager.GetLogger(type);
        }

        private string ProcessMessage(object message)
        {
            // 获取调用LogWithStackTrace方法的堆栈信息
            StackTrace stackTrace = new StackTrace(true);
            return $"{type.Name}:{stackTrace.GetFrame(stackTrace.FrameCount-1).GetFileLineNumber()} - {message}";
        }

        public override void Debug(object message)
        {
            if (CheckLevelLimit(LogLevel.DEBUG))
                _logger.Debug(prefix + ProcessMessage(message));
        }
        public override void Info(object message)
        {
            if (CheckLevelLimit(LogLevel.INFO))
                _logger.Info(prefix + ProcessMessage(message));
        }

        public override void Warn(object message)
        {
            if (CheckLevelLimit(LogLevel.WARN))
                _logger.Warn(prefix + ProcessMessage(message));
        }

        public override void Error(object message)
        {
            if (CheckLevelLimit(LogLevel.ERROR))
                _logger.Error(prefix + ProcessMessage(message));
        }

        public override void Fatal(object message)
        {
            if (CheckLevelLimit(LogLevel.FATAL))
                _logger.Fatal(prefix + ProcessMessage(message));
        }
    }
}
