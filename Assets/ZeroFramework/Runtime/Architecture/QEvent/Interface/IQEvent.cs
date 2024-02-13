/****************************************************
  文件：IQEvent.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/29 22:27:05
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero
{
  /// <summary>
  /// 简单事件公共接口部分
  /// </summary>
  public interface IQEventCommon
  {
    void Clear();

    int GetInvocationList();
  }
  
  /// <summary>
  /// 不含参数的简单事件接口
  /// </summary>
  public interface IQEvent : IQEventCommon
  {
    IUnRegister Register(Action onEvent);

    void UnRegister(Action onEvent);

    void Trigger();
  }
  
  /// <summary>
  /// 含一个参数的简单事件接口
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IQEvent<T> : IQEventCommon
  {
    IUnRegister Register(Action<T> onEvent);
    
    void UnRegister(Action<T> onEvent);

    void Trigger(T param);
  }
  
  /// <summary>
  /// 含一个参数及一个返回值的简单事件接口
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="TR"></typeparam>
  public interface IQEvent<T, TR> : IQEventCommon
  {
  IUnRegister Register(Func<T, TR> onEvent);

  void UnRegister(Func<T, TR> onEvent);

  TR Trigger(T param);
  }
}
