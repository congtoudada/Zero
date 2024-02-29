/****************************************************
  文件：IClock.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/27 22:08:17
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 时钟接口，适用于计算代码运行耗时
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// 得到总耗时 ms
        /// </summary>
        /// <returns></returns>
        long GetTotalUseTime();
        
        /// <summary>
        /// 得到平均耗时 ms
        /// </summary>
        /// <returns></returns>
        double GetAverageUseTime();
        
        /// <summary>
        /// 得到第一轮耗时 ms
        /// </summary>
        /// <returns></returns>
        long GetFirstUseTime();
        
        /// <summary>
        /// 得到最近一轮耗时 ms
        /// </summary>
        /// <returns></returns>
        long GetCurrentUseTime();
        
        /// <summary>
        /// 得到调用Tic-Toc的轮数
        /// </summary>
        /// <returns></returns>
        int GetCount();
        
        /// <summary>
        /// 开始计时
        /// </summary>
        void Tic();
        
        /// <summary>
        /// 结束一轮计时
        /// </summary>
        void Toc();
        
        /// <summary>
        /// 重置计时器
        /// </summary>
        void Reset();
    }
}
