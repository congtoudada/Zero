/****************************************************
  文件：CLASS.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 10:52:26
  功能：
*****************************************************/

namespace Zero.Utility
{
    public interface ISimpleStorageKit : IUtility, ISimpleStorageHelper
    {
        /// <summary>
        /// 创建StorageInfo（无缓存）
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        SimpleStorageInfo CreateStorageInfo(SimpleStorageInfo.StorageMethod method);

        /// <summary>
        /// 重载StorageInfo
        /// </summary>
        /// <param name="storageInfo"></param>
        /// <returns></returns>
        ISimpleStorageKit EquipOverride(SimpleStorageInfo storageInfo);
    }
}