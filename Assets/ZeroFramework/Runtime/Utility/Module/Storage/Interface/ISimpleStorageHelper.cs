/****************************************************
  文件：ISimpleStorageHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 10:32:49
  功能：简单存取键值对
*****************************************************/

namespace Zero.Utility
{
    public interface ISimpleStorageHelper
    {
        /// <summary>
        /// 保存int
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SaveInt(string key, int value);
        /// <summary>
        /// 读取int
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int LoadInt(string key, int defaultValue = 0);
        /// <summary>
        /// 保存float
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SaveFloat(string key, float value);
        /// <summary>
        /// 读取float
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        float LoadFloat(string key, float defaultValue = 0);
        /// <summary>
        /// 保存string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SaveString(string key, string value);
        /// <summary>
        /// 读取string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string LoadString(string key, string defaultValue = "");
        /// <summary>
        /// 清除指定键值对
        /// </summary>
        /// <param name="key"></param>
        void DeleteKey(string key);
        /// <summary>
        /// 清除所有键值对
        /// </summary>
        void DeleteAll();
    }
}