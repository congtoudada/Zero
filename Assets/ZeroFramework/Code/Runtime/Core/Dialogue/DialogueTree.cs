/****************************************************
  文件：DialogueTree.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/8 16:43:14
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Core
{
    /// <summary>
    /// 对话树
    /// </summary>
    [CreateAssetMenu(menuName=("Zero/DialogueTree"), fileName=("DialogueTree"))]
    public class DialogueTree : NodeTree
    {
        public override void Update() {
            base.Update();
        }
    
        public override void TreeStart(){
            base.TreeStart();
        }
    
        public override void TreeEnd(){
            base.TreeEnd();
        }
    }
}
