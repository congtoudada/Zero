/****************************************************
  文件：IBindableProperty.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/29 22:37:03
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 可绑定属性接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindableProperty<T>
    {
        /// <summary>
        /// 绑定比较器
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        BindableProperty<T> WithComparer(Func<T, T, bool> comparer);
        /// <summary>
        /// 属性值
        /// </summary>
        T Value { get; set; }
        /// <summary>
        /// 设置值但不触发回调函数
        /// </summary>
        /// <param name="newValue"></param>
        void SetValueWithoutEvent(T newValue);
        /// <summary>
        /// 注册回调函数，不立即触发
        /// </summary>
        /// <param name="onValueChanged">T1:旧值 T2:新值</param>
        /// <returns></returns>
        IUnRegister Register(Action<T, T> onValueChanged);
        /// <summary>
        /// 注册回调函数，并立即触发
        /// </summary>
        /// <param name="onValueChanged">T1:旧值 T2:新值</param>
        /// <returns></returns>
        IUnRegister RegisterWithInitValue(Action<T, T> onValueChanged);
        /// <summary>
        /// 解绑回调函数
        /// </summary>
        /// <param name="onValueChanged"></param>
        void UnRegister(Action<T, T> onValueChanged);
        /// <summary>
        /// 清空回调函数
        /// </summary>
        void Clear();
    }
}
