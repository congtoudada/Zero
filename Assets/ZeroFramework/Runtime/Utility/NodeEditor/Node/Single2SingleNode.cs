/****************************************************
  文件：Single2SingleNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 16:36:43
  功能：单输入单输出节点
*****************************************************/

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 一对一节点抽象基类
    /// </summary>
    public abstract class Single2SingleNode : ZeroNode
    {
        [OnValueChanged("OnChildrenChange")]
        public ZeroNode child;

        public Single2SingleNode()
        {
            type = Type.Single2Single;
        }
        
#if UNITY_EDITOR
        public override void AddChild(ZeroNode child)
        {
            Undo.RecordObject(this, $"{GetType().Name} (AddChild)");
            this.child = child;
            EditorUtility.SetDirty(this);
        }
        
        public override void RemoveChild(ZeroNode child)
        {
            Undo.RecordObject(this, $"{GetType().Name} (RemoveChild)");
            this.child = null;
            EditorUtility.SetDirty(this);
        }

        public override List<ZeroNode> GetChildren()
        {
            return new List<ZeroNode>(1) {child};
        }

        public void OnChildrenChange()
        {
            OnChildrenChangeInInspector?.Invoke();
        }
#endif
    }
}