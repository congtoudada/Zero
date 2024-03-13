/****************************************************
  文件：Rule.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/29 16:04:15
  功能：定义架构接口规则
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero
{
  /// <summary>
  /// 取得架构的接口
  /// </summary>
  public interface IBelongToArchitecture
  {
    /// <summary>
    /// 取得架构
    /// </summary>
    /// <returns></returns>
    IArchitecture GetArchitecture();
  }
  
  /// <summary>
  /// 设置架构接口
  /// </summary>
  public interface ICanSetArchitecture
  {
    /// <summary>
    /// 设置架构
    /// </summary>
    /// <param name="architecture"></param>
    void SetArchitecture(IArchitecture architecture);
  }
  
  /// <summary>
  /// 从架构获取Model的接口
  /// </summary>
  public interface ICanGetModel : IBelongToArchitecture
  { }
  
  public static class CanGetModelExtension
  {
    public static T GetModel<T>(this ICanGetModel self) where T : class, IModel =>
      self.GetArchitecture().GetModel<T>();
  }
  
  /// <summary>
  /// 从架构获取System的接口
  /// </summary>
  public interface ICanGetSystem : IBelongToArchitecture
  { }
  
  public static class CanGetSystemExtension
  {
    public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem =>
      self.GetArchitecture().GetSystem<T>();
  }
  
  /// <summary>
  /// 从架构获取Utility的接口
  /// </summary>
  public interface ICanGetUtility : IBelongToArchitecture
  { }
  
  public static class CanGetUtilityExtension
  {
    public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility =>
      self.GetArchitecture().GetUtility<T>();
  }
  
  /// <summary>
  /// 从架构绑定事件的接口
  /// </summary>
  public interface ICanRegisterEvent : IBelongToArchitecture
  { }
  
  public static class CanRegisterEventExtension
  {
    public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) =>
      self.GetArchitecture().RegisterEvent<T>(onEvent);

    public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) =>
      self.GetArchitecture().UnRegisterEvent<T>(onEvent);
    
    public static void UnRegisterEvent<T>(this ICanRegisterEvent self) =>
      self.GetArchitecture().UnRegisterEvent<T>();
  }
  
  /// <summary>
  /// 从架构发送事件的接口
  /// </summary>
  public interface ICanSendEvent : IBelongToArchitecture
  { }
  
  public static class CanSendEventExtension
  {
    public static void SendEvent<T>(this ICanSendEvent self) where T : new() =>
      self.GetArchitecture().SendEvent<T>();

    public static void SendEvent<T>(this ICanSendEvent self, T e) => self.GetArchitecture().SendEvent<T>(e);
  }
  
  /// <summary>
  /// 从架构发送命令的接口
  /// </summary>
  public interface ICanSendCommand : IBelongToArchitecture
  { }
  
  public static class CanSendCommandExtension
  {
    public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new() =>
      self.GetArchitecture().SendCommand<T>(new T());

    public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand =>
      self.GetArchitecture().SendCommand<T>(command);

    public static TResult SendCommand<TResult>(this ICanSendCommand self, ICommand<TResult> command) =>
      self.GetArchitecture().SendCommand(command);
  }

  /// <summary>
  /// 从架构发送查询的接口
  /// </summary>
  public interface ICanSendQuery : IBelongToArchitecture
  { }
  
  public static class CanSendQueryExtension
  {
    public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query) =>
      self.GetArchitecture().SendQuery(query);
  }
  
  /// <summary>
  /// 初始化接口
  /// </summary>
  public interface ICanInit
  {
    /// <summary>
    /// 是否初始化
    /// </summary>
    bool Initialized { get; set; }
    /// <summary>
    /// 初始化
    /// </summary>
    void Init();
    /// <summary>
    /// 重置
    /// </summary>
    void Deinit();
  }
}
