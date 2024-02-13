/****************************************************
  文件：UnRegister.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 19:21:56
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zero
{
  /// <summary>
  /// 事件解绑句柄
  /// </summary>
  public struct UnRegisterHandler : IUnRegister
  {
    private Action mOnUnRegister { get; set; }
    public UnRegisterHandler(Action onUnRegister) => mOnUnRegister = onUnRegister;

    public void UnRegister()
    {
      mOnUnRegister.Invoke();
      mOnUnRegister = null;
    }
  }
  
  /// <summary>
  /// 解绑脚本抽象基类。可以在注册事件的同时挂载解绑脚本，实现特定时机自动解绑事件
  /// </summary>
  public abstract class UnRegisterTrigger : MonoBehaviour
  {
    private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();
    public void AddUnRegister(IUnRegister unRegister) => mUnRegisters.Add(unRegister);

    public void RemoveUnRegister(IUnRegister unRegister) => mUnRegisters.Remove(unRegister);

    public void UnRegister()
    {
      foreach (var unRegister in mUnRegisters)
      {
        unRegister.UnRegister();
      }
      mUnRegisters.Clear();
      // mUnRegisters = null;
      // Destroy(this);
    }
  }
  
  /// <summary>
  /// 销毁时解绑的脚本
  /// </summary>
  public class UnRegisterOnDestroyTrigger : UnRegisterTrigger
  {
    private void OnDestroy()
    {
      UnRegister();
    }
  }
  
  /// <summary>
  /// 禁用时解绑的脚本
  /// </summary>
  public class UnRegisterOnDisableTrigger : UnRegisterTrigger
  {
    private void OnDisable()
    {
      UnRegister();
    }
  }
  
  /// <summary>
  /// 解绑扩展类，用于将解绑句柄绑定到特定游戏对象的特定生命周期，实现自动解绑事件
  /// </summary>
  public static class UnRegisterExtension
  {
    /// <summary>
    /// 绑定事件解绑句柄到销毁时
    /// </summary>
    /// <param name="unRegister"></param>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister,
      UnityEngine.GameObject gameObject)
    {
      var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

      if (!trigger)
      {
        trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
      }

      trigger.AddUnRegister(unRegister);

      return unRegister;
    }
    
    /// <summary>
    /// 绑定事件解绑句柄到销毁时
    /// </summary>
    /// <param name="self"></param>
    /// <param name="component"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IUnRegister UnRegisterWhenGameObjectDestroyed<T>(this IUnRegister self, T component)
      where T : UnityEngine.Component =>
      self.UnRegisterWhenGameObjectDestroyed(component.gameObject);
    
    /// <summary>
    /// 绑定事件解绑句柄到禁用时
    /// </summary>
    /// <param name="unRegister"></param>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static IUnRegister UnRegisterWhenDisabled(this IUnRegister unRegister,
      UnityEngine.GameObject gameObject)
    {
      var trigger = gameObject.GetComponent<UnRegisterOnDisableTrigger>();

      if (!trigger)
      {
        trigger = gameObject.AddComponent<UnRegisterOnDisableTrigger>();
      }

      trigger.AddUnRegister(unRegister);

      return unRegister;
    }
    
    /// <summary>
    /// 绑定事件解绑句柄到禁用时
    /// </summary>
    /// <param name="self"></param>
    /// <param name="component"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IUnRegister UnRegisterWhenDisabled<T>(this IUnRegister self, T component)
      where T : UnityEngine.Component =>
      self.UnRegisterWhenDisabled(component.gameObject);
  }
}