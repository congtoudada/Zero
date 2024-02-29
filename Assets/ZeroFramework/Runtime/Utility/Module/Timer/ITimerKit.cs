/****************************************************
  文件：ITimer.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/29 17:56:38
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 定时回调工具接口
    /// </summary>
    public interface ITimerKit
    {
        /// <summary>
        /// 执行定时回调
        /// </summary>
        /// <param name="delay">延迟执行时间（单位：秒）</param>
        /// <param name="callback">回调函数</param>
        /// <param name="count">回调执行次数，如果为-1则无限执行</param>
        /// <param name="interval">每次执行间隔</param>
        /// <returns>任务唯一id，可用于终止任务</returns>
        long Wait(float delay, Action callback, int count = 1, float interval = 0);
        
        /// <summary>
        /// 执行定时回调
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="callback"></param>
        /// <param name="param">外部传入的参数</param>
        /// <param name="count">回调执行次数，如果为-1则无限执行</param>
        /// <param name="interval">每次执行间隔</param>
        /// <returns>任务唯一id，可用于终止任务</returns>
        long Wait(float delay, Action<object> callback, object param, int count = 1, float interval = 0);
        
        /// <summary>
        /// 终止定时回调函数
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool Abort(long taskId);
        
        /// <summary>
        /// 清空所有定时任务
        /// </summary>
        void AbortAll();
    }
}
