/****************************************************
  文件：ICommonEventKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 21:14:17
  功能：
*****************************************************/

using System;

namespace Zero
{
    /// <summary>
    /// 通用事件系统接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ICommonEventSystem<TKey> : IUtility
    {
        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IUnRegister Register<T>(TKey key, Action<T> onEvent);
        
        /// <summary>
        /// 解绑指定Key的指定事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        void UnRegister<T>(TKey key, Action<T> onEvent);
        
        /// <summary>
        /// 解绑指定Key的所有事件
        /// </summary>
        /// <param name="key"></param>
        void UnRegister(TKey key);
        
        /// <summary>
        /// 根据Key触发事件
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        void Send<T>(TKey key) where T : new();
        
        /// <summary>
        /// 根据Key触发事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="e"></param>
        /// <typeparam name="T"></typeparam>
        void Send<T>(TKey key, T e);
        
        /// <summary>
        /// 清空事件系统所有事件
        /// </summary>
        void Clear();
    }
}