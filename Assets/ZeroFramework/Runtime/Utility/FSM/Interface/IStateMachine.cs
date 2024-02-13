/****************************************************
  文件：IStateMachine.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-03 16:35:36
  功能：
*****************************************************/
using System;

namespace Zero.Utility
{
    /// <summary>
    /// 有限状态机接口
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// 状态机持有者
        /// </summary>
        object Owner { get; }
        /// <summary>
        /// 当前状态节点
        /// </summary>
        IStateNode CurrentState { get; }
        /// <summary>
        /// 前一个状态节点
        /// </summary>
        IStateNode PreviousState { get; }
        /// <summary>
        /// 当前状态节点运行的帧数
        /// </summary>
        long FrameCountOfCurrentState { get; }
        /// <summary>
        /// 当前状态节点运行的秒数
        /// </summary>
        float SecondsOfCurrentState { get; }

        /// <summary>
        /// 添加状态节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IStateMachine AddState<T>() where T : IStateNode;
        
        /// <summary>
        /// 添加状态节点
        /// </summary>
        /// <param name="stateNode"></param>
        /// <returns></returns>
        IStateMachine AddState(IStateNode stateNode);

        /// <summary>
        /// 设置状态切换的监听 T1:旧状态ID T2:新状态ID
        /// </summary>
        /// <param name="onStateChanged"></param>
        /// <returns></returns>
        IStateMachine AddOnStateChanged(Action<IStateNode, IStateNode> onStateChanged);
        
        /// <summary>
        /// 移除状态切换监听 T1:旧状态ID T2:新状态ID
        /// </summary>
        /// <param name="onStateChanged"></param>
        /// <returns></returns>
        IStateMachine RemoveOnStateChanged(Action<IStateNode, IStateNode> onStateChanged);

        /// <summary>
        /// 移除状态节点（正在运行的状态除外）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool RemoveState<T>() where T : IStateNode;
        
        /// <summary>
        /// 移除状态节点（正在运行的状态除外）
        /// </summary>
        /// <param name="stateNode"></param>
        /// <returns></returns>
        bool RemoveState(IStateNode stateNode);

        /// <summary>
        /// 清空FSM及其Blackboard
        /// </summary>
        void Clear();
        
        /// <summary>
        /// 获得状态节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IStateNode TryGetState<T>() where T : IStateNode;
        
        /// <summary>
        /// 获得状态节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        IStateNode TryGetState(string nodeName);

        /// <summary>
        /// 启动状态机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Run<T>();
        
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ChangeState<T>();

        /// <summary>
        /// Update时调用
        /// </summary>
        void Update();
        
        /// <summary>
        /// FixUpdate时调用
        /// </summary>
        void FixedUpdate();
        
        /// <summary>
        /// OnGUI时调用
        /// </summary>
        void OnGUI();
        
        /// <summary>
        /// 设置Blackboard值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetBlackboardValue(string key, object value);
        
        /// <summary>
        /// 获取Blackboard值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetBlackboardValue(string key);
        
        /// <summary>
        /// 清空Blackboard
        /// </summary>
        void ClearBlackboard();
    }
}