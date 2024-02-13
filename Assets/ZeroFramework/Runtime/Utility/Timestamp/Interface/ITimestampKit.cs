/****************************************************
  文件：ITimestampKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/27 12:24:58
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 时间戳工具接口
    /// </summary>
    public interface ITimestampKit : IUtility
    {
        /// <summary>
        /// 获得1970以来的时间戳
        /// </summary>
        /// <param name="isMilliseconds"></param>
        /// <returns></returns>
        long GetTimestamp(bool isMilliseconds = true);
        
        /// <summary>
        /// 根据毫秒时间戳获取字符串
        /// </summary>
        /// <param name="millionSeconds"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        string GetTimeString(long millionSeconds, string format = "yyyy-MM-dd HH:mm:ss");
        
        /// <summary>
        /// 获取IClock对象
        /// </summary>
        /// <returns></returns>
        IClock AllocateClock();
    }
}
