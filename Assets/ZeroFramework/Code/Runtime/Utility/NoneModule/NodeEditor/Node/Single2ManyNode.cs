/****************************************************
  文件：Single2ManyNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 18:51:33
  功能：
*****************************************************/

using System.Collections.Generic;
using UnityEditor;

namespace Zero.Utility
{
    /// <summary>
    /// 一对多节点抽象基类
    /// </summary>
    public abstract class Single2ManyNode : Many2ManyNode
    {
        public Single2ManyNode()
        {
            type = Type.Single2Many;
        }
    }
}