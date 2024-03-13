/****************************************************
  文件：UniLogger.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-05 21:07:01
  功能：
*****************************************************/

namespace Zero.Utility
{
    internal static class UniLogger
    {
        private static ILogger logger;

        static UniLogger()
        {
            logger = ZeroToolKits.Instance.InnerLog.AllocateLogger(typeof(UniLogger));
        }
        public static void Log(string info)
        {
            logger.Debug(info);
        }
        public static void Warning(string info)
        {
            logger.Warn(info);
        }
        public static void Error(string info)
        {
            logger.Error(info);
        }
    }
}