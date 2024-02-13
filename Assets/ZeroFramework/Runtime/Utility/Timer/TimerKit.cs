/****************************************************
  文件：Timer.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-30 20:09:23
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zero.Utility
{
    [MonoSingletonPath("ZeroFramework/Utility/TimerKit")]
    public class TimerKit : SingletonMono<TimerKit>, ITimerKit
    {
        public long currentTimerId;
        public List<TimerTask> timerList;
        private IZeroObjectPool<TimerTask> timerPool;

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            currentTimerId = 1;
            timerList = new List<TimerTask>();
            timerPool = new SimpleObjectPool<TimerTask>(Create, 
                null, null, null, false, 5, 101, false);
        }

        private TimerTask Create()
        {
            return new TimerTask();
        }
        
        private void Release(int idx)
        {
            timerList[idx].Reset();
            timerPool.Release(timerList[idx]);
            timerList.RemoveAt(idx);
        }
        
        private void Release(TimerTask task)
        {
            task.Reset();
            timerPool.Release(task);
            timerList.Remove(task);
        }

        public void Update()
        {
            if (timerList.Count <= 0) return;
            float time = Time.time;
            //处理每一个任务项
            for (int i = 0; i < timerList.Count; i++)
            {
                // 过滤次数不满足的
                if (timerList[i].count == 0)
                {
                    Release(i--);
                    continue;;
                }
                if(time >= timerList[i].destTime)
                {
                    timerList[i].callback(timerList[i].param);
                    timerList[i].destTime = time + timerList[i].interval;
                    if (timerList[i].count >= 1)
                        timerList[i].count--;
                }
            }
        }

        public long Wait(float delay, Action callback, int count = 1, float interval = 0)
        {
            var task = timerPool.Get();
            task.Init(currentTimerId, delay, _ => { callback();}, count, interval, null);
            timerList.Add(task);
            return currentTimerId++;
        }

        public long Wait(float delay, Action<object> callback, object param,  int count = 1, float interval = 0)
        {
            var task = timerPool.Get();
            task.Init(currentTimerId, delay, callback, count, interval, param);
            timerList.Add(task);
            return currentTimerId++;
        }

        public bool Abort(long taskId)
        {
            var task = timerList.FirstOrDefault(s => s.timerId == taskId);
            if (task != null)
            {
                Release(task);
                return true;
            }
            return false;
        }

        public void AbortAll()
        {
            for (int i = 0; i < timerList.Count; i++)
            {
                Release(i--);
            }
            timerPool.Clear();
            timerList.Clear();
            timerList = new List<TimerTask>();
        }
    }
}