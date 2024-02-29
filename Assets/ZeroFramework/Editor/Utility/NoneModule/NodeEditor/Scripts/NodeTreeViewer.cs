/****************************************************
  文件：NodeTreeViewer.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 17:22:26
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Zero.Utility;

namespace Zero.Editor
{
    /// <summary>
    /// 节点树视图
    /// </summary>
    public class NodeTreeViewer : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NodeTreeViewer,GraphView.UxmlTraits>{}
        
        public NodeTree tree; //数据
        public Action<NodeView> OnNodeSelected; //节点被选中时的回调
        private string currentGUID;

        public NodeTreeViewer()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer()); //缩放画布
            this.AddManipulator(new ContentDragger()); //拖拽画布
            this.AddManipulator(new SelectionDragger()); //选中元素拖拽
            this.AddManipulator(new RectangleSelector()); //框选元素

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(NodeEditorConfig.NODE_TREE_VIEWER_USS);
            styleSheets.Add(styleSheet);
            
            Undo.undoRedoPerformed += OnUndoRedo;
        }
        
        //撤销重做回调函数
        private void OnUndoRedo()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }
        
        //右键菜单项
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<ZeroNode>();
            foreach (var type in types)
            {
                if (!type.IsAbstract)
                {
                    if (types.Count > 9)
                    {
                        evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}", 
                            (a) => CreateNode(type));
                    }
                    else
                    {
                        evt.menu.AppendAction($"{type.Name}", 
                            (a) => CreateNode(type));
                    }
                }
            }
        }
        
        //创建节点
        private void CreateNode(Type type)
        {
            //创建数据
            ZeroNode node = tree.CreateNode(type);
            CreateNodeView(node);
        }
        
        //创建节点视图
        private void CreateNodeView(ZeroNode node)
        {
            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected += OnNodeSelected;
            nodeView.OnNodeSelected += SaveSelectedGUID;
            node.OnChildrenChangeInInspector += PopulateView;
            AddElement(nodeView);
        }

        private void SaveSelectedGUID(NodeView nodeView)
        {
            if (nodeView != null)
                currentGUID = nodeView.node.guid;
        }

        private void PopulateView()
        {
            PopulateView(tree);
            if (GetNodeByGuid(currentGUID) is NodeView nodeView)
            {
                AddToSelection(nodeView);
            }
        }
        
        //绘制
        public void PopulateView(NodeTree tree)
        {
            if (tree == null)
            {
                DeleteElements(graphElements); //清空元素
                return;
            }
            this.tree = tree; //Tree数据
            graphViewChanged -= OnGraphViewChanged; //视图更改触发
            DeleteElements(graphElements); //清空元素
            graphViewChanged += OnGraphViewChanged;
            tree.nodes.ForEach(n => CreateNodeView(n)); //创建节点元素
            tree.nodes.ForEach(n =>
            {
                var children = n.GetChildren();
                children.ForEach(c =>
                {
                    if (c != null)
                    {
                        NodeView parentView = FindNodeView(n);
                        NodeView childView = FindNodeView(c);
                        Edge edge = parentView.output.ConnectTo(childView.input);
                        AddElement(edge);
                    }
                });
            }); //创建连接线
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            //删除节点
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    if (elem is NodeView nodeView)
                        tree.DeleteNode(nodeView.node); //删除数据
                    if (elem is Edge edge)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        tree.RemoveChild(parentView?.node, childView?.node);
                    }
                });
            }
            //创建连接线
            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.AddChild(parentView?.node, childView?.node);
                });
            }
            return graphViewChange;
        }
        
        //获取与给定端口兼容的所有端口
        private NodeView FindNodeView(ZeroNode node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }
        
        //设置端口连接规则
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            //①输出连输入；②不是自身
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction &&
                                                   endPort.node != startPort.node).ToList();
        }
        
        //更新运行时节点样式
        public void UpdateNodeStates()
        {
            nodes.ForEach(n =>
            {
                if (n is NodeView view)
                {
                    view.SetNodeStateStyle();
                }
            });
        }
    }
}