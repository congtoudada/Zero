/****************************************************
  文件：UserEventDefine.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-11 21:05:18
  功能：
*****************************************************/

namespace Zero.Utility
{
    public class UserEventDefine
    {
        /// <summary>
        /// 用户尝试再次初始化资源包
        /// </summary>
        public class UserTryInitialize : IEventMessage
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryInitialize();
                UniEventKit.Instance.SendMessage(msg);
            }
        }

        /// <summary>
        /// 用户开始下载网络文件
        /// </summary>
        public class UserBeginDownloadWebFiles : IEventMessage
        {
            public static void SendEventMessage()
            {
                var msg = new UserBeginDownloadWebFiles();
                UniEventKit.Instance.SendMessage(msg);
            }
        }

        /// <summary>
        /// 用户尝试再次更新静态版本
        /// </summary>
        public class UserTryUpdatePackageVersion : IEventMessage
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryUpdatePackageVersion();
                UniEventKit.Instance.SendMessage(msg);
            }
        }

        /// <summary>
        /// 用户尝试再次更新补丁清单
        /// </summary>
        public class UserTryUpdatePatchManifest : IEventMessage
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryUpdatePatchManifest();
                UniEventKit.Instance.SendMessage(msg);
            }
        }

        /// <summary>
        /// 用户尝试再次下载网络文件
        /// </summary>
        public class UserTryDownloadWebFiles : IEventMessage
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryDownloadWebFiles();
                UniEventKit.Instance.SendMessage(msg);
            }
        }
    }
}