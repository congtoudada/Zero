/****************************************************
  文件：BranchDialogue.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/8 18:00:13
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Core
{
    /// <summary>
    /// 分支对话节点
    /// </summary>
    public class BranchDialogue : Many2ManyNode
    {
        protected override ZeroNode OnUpdate()
        {
            return this;
        }

        protected override void OnStart()
        {
        
        }

        protected override void OnStop()
        {
            currentState = State.Waiting;
        }
    }
}
