/****************************************************
  文件：ILogTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/22 14:14:58
  功能：
*****************************************************/
namespace Zero.Utility
{
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }

    public interface ILogger
    {
        /// <summary>
        /// 限制等级，只有大于等于该等级的日志允许被打印
        /// </summary>
        /// <param name="level"></param>
        void SetLevelLimit(LogLevel level);
        /// <summary>
        /// Lv1: 打印DEBUG
        /// </summary>
        /// <param name="message"></param>
        void Debug(object message);
        /// <summary>
        /// Lv2: 打印Info
        /// </summary>
        /// <param name="message"></param>
        void Info(object message);
        /// <summary>
        /// Lv3: 打印Warn
        /// </summary>
        /// <param name="message"></param>
        void Warn(object message);
        /// <summary>
        /// Lv4: 打印Error
        /// </summary>
        /// <param name="message"></param>
        void Error(object message);
        /// <summary>
        /// Lv5: 打印Fatal
        /// </summary>
        /// <param name="message"></param>
        void Fatal(object message);
        /// <summary>
        /// 打印指定日志等级的日志（默认Debug）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        void Log(object message, LogLevel level = LogLevel.DEBUG);
    }
}
