/****************************************************
  文件：SimpleStorageInfo.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 10:33:23
  功能：
*****************************************************/

namespace Zero.Utility
{
    public class SimpleStorageInfo
    {
        public enum StorageMethod
        {
            PLAYER_PREFS,
            // JSON,
            // MYSQL
        }

        public StorageMethod method;

        public SimpleStorageInfo(StorageMethod method)
        {
            this.method = method;
        }
    }
}