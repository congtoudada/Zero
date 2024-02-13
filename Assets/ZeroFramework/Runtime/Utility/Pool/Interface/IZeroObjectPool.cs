/****************************************************
  文件：IObjectPool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/23 19:58:57
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// Zero框架的对象池接口（参考Unity官方的ObjectPool）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IZeroObjectPool<T>
    {
        /// <summary>
        /// 从对象池中获取一个对象，触发actionOnGet事件
        /// </summary>
        /// <returns></returns>
        T Get();
        
        /// <summary>
        /// 将对象归还到对象池。归还后的对象应视为销毁，后续不可继续使用。
        /// </summary>
        /// <param name="obj"></param>
        void Release(T obj);
        
        /// <summary>
        /// 将对象池的所有对象清空，触发actionOnDestroy事件
        /// </summary>
        void Clear();
        
        /// <summary>
        /// 返回池已创建但当前正在使用，且尚未返回的对象数
        /// </summary>
        /// <returns></returns>
        int GetCountActive();
        
        /// <summary>
        /// 活动和非活动对象的总数（CountActive + CountInactive）
        /// </summary>
        /// <returns></returns>
        int GetCountAll();
        
        /// <summary>
        /// 池中当前可用的对象数
        /// </summary>
        /// <returns></returns>
        int GetCountInactive();
    }
}
