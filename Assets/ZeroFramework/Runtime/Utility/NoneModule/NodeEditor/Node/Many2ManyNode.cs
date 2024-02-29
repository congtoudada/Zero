/****************************************************
  文件：Many2ManyNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 16:38:50
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Zero.Utility
{
    /// <summary>
    /// 多对多节点抽象基类
    /// </summary>
    public abstract class Many2ManyNode : ZeroNode
    {
        [OnValueChanged("OnChildrenChange")]
        public List<ZeroNode> children = new List<ZeroNode>();

        public Many2ManyNode()
        {
            type = Type.Many2Many;
        }
        
#if UNITY_EDITOR
        public override void AddChild(ZeroNode child)
        {
            Undo.RecordObject(this, $"{GetType().Name} (AddChild)");
            children.Add(child);
            EditorUtility.SetDirty(this);
        }

        public override void RemoveChild(ZeroNode child)
        {
            Undo.RecordObject(this, $"{GetType().Name} (RemoveChild)");
            this.children.Remove(child);
            EditorUtility.SetDirty(this);
        }

        public override List<ZeroNode> GetChildren()
        {
            return children;
        }

        public void OnChildrenChange()
        {
            OnChildrenChangeInInspector?.Invoke();
        }
#endif
    }
}