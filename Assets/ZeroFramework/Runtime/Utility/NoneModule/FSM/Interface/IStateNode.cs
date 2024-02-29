/****************************************************
  文件：IStateNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/3 16:15:55
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IStateNode
    {
        /// <summary>
        /// 获取状态机
        /// </summary>
        IStateMachine FSM { get; }
        
        /// <summary>
        /// 判断是否切换状态
        /// </summary>
        /// <returns></returns>
        bool OnCondition();
        
        /// <summary>
        /// 创建时调用
        /// </summary>
        void OnCreate(IStateMachine stateMachine);
        
        /// <summary>
        /// 进入时调用
        /// </summary>
        void OnEnter();
        
        /// <summary>
        /// Update时调用
        /// </summary>
        void OnUpdate();
        
        /// <summary>
        /// FixUpdate时调用
        /// </summary>
        void OnFixedUpdate();
        
        /// <summary>
        /// OnGUI时调用
        /// </summary>
        void OnGUI();
        
        /// <summary>
        /// Exit时调用
        /// </summary>
        void OnExit();
    }

}
