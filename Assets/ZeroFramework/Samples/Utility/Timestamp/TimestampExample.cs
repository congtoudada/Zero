/****************************************************
  文件：TimestampExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/27 12:22:22
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Samples
{
    public class TimestampExample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var timerKit = ZeroToolKits.Instance.Timestamp;
            var clock = timerKit.AllocateClock();
            
            for (int i = 0; i < 3; i++)
            {
                clock.Tic();
                long timestamp = timerKit.GetTimestamp();
                Debug.Log("Timestamp: " + timestamp);
                Debug.Log("TimestampString: " + timerKit.GetTimeString(timestamp));
                clock.Toc();
            }
            
            Debug.Log("---------- Clock ----------");
            Debug.Log("平均耗时: " + clock.GetAverageUseTime() + " ms");
            Debug.Log("第一轮耗时: " + clock.GetFirstUseTime() + " ms"); //若测试循环，第一轮通常Miss Cache，用时较长
            Debug.Log("最近一轮耗时: " + clock.GetCurrentUseTime() + " ms");
            Debug.Log("总用时: " + clock.GetTotalUseTime() + " ms");
            Debug.Log("总轮数: " + clock.GetCount() + " ms");
            
            clock.Reset();
            Debug.Log("清空计时器");
            Debug.Log("平均耗时: " + clock.GetAverageUseTime() + " ms");
        }
    }
}
