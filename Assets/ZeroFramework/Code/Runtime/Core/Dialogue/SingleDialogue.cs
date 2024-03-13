/****************************************************
  文件：SingleDialogue.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/8 16:45:16
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Core
{
    /// <summary>
    /// 单对话节点
    /// </summary>
    public class SingleDialogue : Single2SingleNode
    {
        public string content;
        protected override ZeroNode OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentState = State.Waiting; //结束自身
                return child; //返回下一个状态
            }
            return this;
        }

        protected override void OnStart()
        {
            Debug.Log("Content: " + content);
        }

        protected override void OnStop()
        {
            currentState = State.Waiting;
        }
    }
}
