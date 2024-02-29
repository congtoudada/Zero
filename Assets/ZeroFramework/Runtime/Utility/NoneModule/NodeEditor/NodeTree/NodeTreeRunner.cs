/****************************************************
  文件：NodeTreeRunner.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 16:30:04
  功能：
*****************************************************/

using System;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 节点树运行时脚本
    /// </summary>
    public class NodeTreeRunner : MonoBehaviour
    {
        public NodeTree tree;

        private void Update()
        {
            if (tree == null)
            {
                Debug.LogError("NodeTree is null");
                return;
            }
#if ENABLE_LEGACY_INPUT_MANAGER
            //开始
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (tree.treeState != ZeroNode.State.Running)
                {
                    Debug.Log("NodeTree Start!");
                    tree.TreeStart();
                }
            }
            
            //更新            
            if (tree.treeState == ZeroNode.State.Running)
            {
                tree.Update();
            }
            
            //退出
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("NodeTree End!");
                tree.TreeEnd();
            }
#else
            Debug.Log("Sorry, 示例使用Legacy InputSystem实现!")
#endif
        }
    }
}