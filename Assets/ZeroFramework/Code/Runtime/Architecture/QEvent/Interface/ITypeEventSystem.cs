/****************************************************
  文件：ITypeEventKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 20:21:36
  功能：
*****************************************************/

using System;

namespace Zero
{
    /// <summary>
    /// 类型事件系统接口
    /// </summary>
    public interface ITypeEventSystem : IUtility
    {   
        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IUnRegister Register<T>(Action<T> onEvent);
        
        /// <summary>
        /// 解绑指定类型的指定事件
        /// </summary>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        void UnRegister<T>(Action<T> onEvent);
        
        /// <summary>
        /// 解绑指定类型的所有事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UnRegister<T>();
        
        /// <summary>
        /// 根据类型触发事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Send<T>() where T : new();
        
        /// <summary>
        /// 根据类型触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <typeparam name="T"></typeparam>
        void Send<T>(T e);
        
        /// <summary>
        /// 清空事件系统所有事件
        /// </summary>
        void Clear();
    }
}