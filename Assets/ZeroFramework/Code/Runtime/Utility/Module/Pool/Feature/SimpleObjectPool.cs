/****************************************************
  文件：SimpleObjectPool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 16:10:07
  功能：
*****************************************************/

using System;

namespace Zero.Utility
{
    /// <summary>
    /// 简单对象池：①预先创建对象数量由开发者决定；②池空再取会创建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleObjectPool<T> : ZeroObjectPool<T>
    {
        public SimpleObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, 
            Action<T> actionOnDestroy = null, bool collectionCheck = false, int defaultCapacity = 10, int maxSize = 100, bool isPreLoad = false)
            : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize, isPreLoad)
        {
            
        }
    }
}