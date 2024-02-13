/****************************************************
  文件：Many2SingleNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 18:53:26
  功能：
*****************************************************/

using UnityEditor;

namespace Zero.Utility
{
    /// <summary>
    /// 多对一节点抽象基类
    /// </summary>
    public abstract class Many2SingleNode : Single2SingleNode
    {
        public Many2SingleNode()
        {
            type = Type.Many2Single;
        }
    }
}