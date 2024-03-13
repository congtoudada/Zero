/****************************************************
  文件：IPoolKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/23 19:58:57
  功能：Nothing
*****************************************************/
using System;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 对象池工具接口
    /// </summary>
    public interface IPoolKit : IUtility
    {
        /// <summary>
        /// 创建SimplePool
        /// </summary>
        /// <param name="createFunc">用于在池为空时创建新实例。在大多数情况下，这只是()=> new T()</param>
        /// <param name="actionOnGet">从池中获取实例时调用</param>
        /// <param name="actionOnRelease">当实例返回到池时调用。这可用于清理或禁用实例</param>
        /// <param name="actionOnDestroy">当由于池达到最大大小而无法将元素返回到池时调用</param>
        /// <param name="collectionCheck">将实例返回到池时执行集合检查。如果实例已经在池中，则会抛出异常。集合检查仅在编辑器中执行</param>
        /// <param name="defaultCapacity">创建堆栈时使用的默认容量 【仅SimplePool可用】</param>
        /// <param name="maxSize">池的最大大小。当池达到最大大小时，返回到池中的任何其他实例都将被丢弃，并可以被垃圾收集。这可以用来防止池增长到非常大的规模。</param>
        /// <param name="isPreLoad">是否预生成对象【仅SimplePool可用，SafeObjectPool是必须的】</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IZeroObjectPool<T> AllocateSimpleObjectPool<T>(Func<T> createFunc, Action<T> actionOnGet,
            Action<T> actionOnRelease, Action<T> actionOnDestroy,
            bool collectionCheck, int defaultCapacity, int maxSize, bool isPreLoad = false);
        
        /// <summary>
        /// 创建SafePool
        /// </summary>
        /// <param name="createFunc">用于在池为空时创建新实例。在大多数情况下，这只是()=> new T()</param>
        /// <param name="actionOnGet">从池中获取实例时调用</param>
        /// <param name="actionOnRelease">当实例返回到池时调用。这可用于清理或禁用实例</param>
        /// <param name="actionOnDestroy">当由于池达到最大大小而无法将元素返回到池时调用</param>
        /// <param name="collectionCheck">将实例返回到池时执行集合检查。如果实例已经在池中，则会抛出异常。集合检查仅在编辑器中执行</param>
        /// <param name="maxSize">池的最大大小。当池达到最大大小时，返回到池中的任何其他实例都将被丢弃，并可以被垃圾收集。这可以用来防止池增长到非常大的规模。</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IZeroObjectPool<T> AllocateSafeObjectPool<T>(Func<T> createFunc, Action<T> actionOnGet,
            Action<T> actionOnRelease, Action<T> actionOnDestroy,
            bool collectionCheck, int maxSize);
    }
}