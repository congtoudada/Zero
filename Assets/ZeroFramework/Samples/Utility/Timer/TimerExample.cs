/****************************************************
  文件：TimerExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/30 21:14:29
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Zero.Utility;

namespace Zero.Samples
{
    public class TimerExample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ITimerKit timerKit = TimerKit.Instance;
            Debug.Log("Begin!");
            timerKit.Wait(3.0f, () => { Debug.Log("Hello world!"); });
            Debug.Log("Bye!");

            long taskId = timerKit.Wait(1.0f, () => { Debug.Log("Abort Test"); });
            bool ret = timerKit.Abort(taskId);
            Debug.Log("终止taskId: " + taskId + " 结果: " + ret);
        }
    }
}
