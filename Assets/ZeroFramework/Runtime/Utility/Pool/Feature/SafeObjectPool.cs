/****************************************************
  文件：SafeObjectPool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 16:10:19
  功能：
*****************************************************/

using System;

namespace Zero.Utility
{
    /// <summary>
    /// 安全对象池：①预先创建所有对象 【必须】；②池空再取返回null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeObjectPool<T> : ZeroObjectPool<T>
    {
        public SafeObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, 
            Action<T> actionOnDestroy = null, bool collectionCheck = false, int maxSize = 100) 
            : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, maxSize, maxSize, true)
        {
        }

        public override T Get()
        {
            // 缓存池数量充足
            if (CountInactive > 0)
            {
                CountActive += 1;
                CountInactive -= 1;
                T obj = _pool.Pop();
                _actionOnGet?.Invoke(obj);
                return obj;
            }
            return default(T);
        }
    }
}