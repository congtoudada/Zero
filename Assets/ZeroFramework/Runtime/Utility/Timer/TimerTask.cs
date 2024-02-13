/****************************************************
  文件：TimerTask.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-30 20:11:10
  功能：
*****************************************************/

using System;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 定时任务最小单元
    /// </summary>
    [Serializable]
    public class TimerTask
    {
        public long timerId; //timerId
        public Action<object> callback; //执行的回调
        public float delay; //延迟执行的时间 单位:s
        public float destTime;  //执行回调时的时间 单位:s
        public int count;   //回调执行次数,默认执行1次，如果为-1则无限执行
        public float interval;  //回调次数大于1，每次执行间隔
        public object param; //外部传入的参数

        public TimerTask()
        {
            
        }

        public void Init(long timerId, float delay, Action<object> callback, int count, float interval, object param)
        {
            this.timerId = timerId;
            this.delay = delay;
            destTime = Time.time + delay;
            this.callback = callback;
            if (count <= 0) count = 1;
            this.count = count;
            if (interval <= 0) interval = delay;
            this.interval = interval;
            this.param = param;
        }

        public void Reset()
        {
            count = 0;
            callback = null;
        }
    }
}