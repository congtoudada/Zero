/****************************************************
  文件：NodeTree.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/8 15:19:00
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 节点树
    /// </summary>
    public class NodeTree : ScriptableObject
    {
        [ReadOnly] public NodeTree asset;
        [ReadOnly][FoldoutGroup("Debug")] public ZeroNode runningNode; //当前运行节点
        [ReadOnly][FoldoutGroup("Debug")] public ZeroNode.State currentState; //当前运行节点状态
        public List<ZeroNode> nodes = new List<ZeroNode>();
        public ZeroNode rootNode; //根节点
        public ZeroNode.State treeState = ZeroNode.State.Waiting; //树运行状态
        public int createID; //创建节点时附带的ID
        public event Action OnDestroyEvent; 
        
#if UNITY_EDITOR
        public class AssetDeletionListener : UnityEditor.AssetModificationProcessor
        {
            // 当资产被删除时调用
            public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
            {
                var asset = AssetDatabase.LoadAssetAtPath<NodeTree>(assetPath);
                if (asset != null)
                {
                    // 在这里执行您的处理逻辑，例如通知NodeEditor类
                    asset.Destroy();
                }
                return AssetDeleteResult.DidNotDelete;
            }
        }
#endif

        public virtual void Update()
        {
            if (runningNode == null)
            {
                TreeEnd();
                return;
            }
            if (treeState == ZeroNode.State.Running && runningNode != null)
            {
                runningNode = runningNode.Update();
            }
        }

        public virtual void TreeStart()
        {
            treeState = ZeroNode.State.Running;
            if (rootNode == null && nodes.Count > 0)
                rootNode = nodes[0];
            runningNode = rootNode;
        }

        public virtual void TreeEnd()
        {
            treeState = ZeroNode.State.Waiting;
            runningNode = null;
        }

        public void Destroy()
        {
            OnDestroyEvent?.Invoke();
        }

#if UNITY_EDITOR
        //新增节点
        public ZeroNode CreateNode(System.Type type)
        {
            Undo.RecordObject(this, "NodeTree (CreateNode)");
            if (ScriptableObject.CreateInstance(type) is ZeroNode node)
            {
                node.description = "description";
                node.name = type.Name + createID++;
                node.guid = GUID.Generate().ToString();
                node.asset = node;
                nodes.Add(node);
                if (!Application.isPlaying)
                {
                    AssetDatabase.AddObjectToAsset(node, this);
                }
                Undo.RegisterCreatedObjectUndo(node, "NodeTree (CreateNode)");
                AssetDatabase.SaveAssets();
                return node;
            }
            return null;
        }
        
        //删除节点
        public ZeroNode DeleteNode(ZeroNode node)
        {
            Undo.RecordObject(this, "NodeTree (DeleteNode)");
            nodes.Remove(node);
            Undo.DestroyObjectImmediate(node); //删除且记录Undo
            AssetDatabase.SaveAssets();
            return node;
        }
        
        //添加子节点
        public void AddChild(ZeroNode parent, ZeroNode child)
        {
            if (parent == null || child == null) return;
            parent.AddChild(child);
        }
        
        //删除子节点
        public void RemoveChild(ZeroNode parent, ZeroNode child)
        {
            if (parent == null || child == null) return;
            parent.RemoveChild(child);
        }
        
        //获取父节点下的所有子节点
        public List<ZeroNode> GetChildrenFromParent(ZeroNode parent)
        {
            if (parent == null) return null;
            return parent.GetChildren();
        }
#endif
    }
}
