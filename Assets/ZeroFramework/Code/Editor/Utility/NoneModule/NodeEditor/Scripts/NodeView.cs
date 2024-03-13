/****************************************************
  文件：NodeView.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 17:17:43
  功能：
*****************************************************/

using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Zero.Utility;

namespace Zero.Editor
{
    /// <summary>
    /// 节点视图
    /// </summary>
    public class NodeView :  UnityEditor.Experimental.GraphView.Node
    {
        public ZeroNode node; //节点数据
        public Port input; //输入端口
        public Port output; //输出端口
        public Action<NodeView> OnNodeSelected; //选中当前节点时触发的回到函数
        private VisualElement nodeState;
        public NodeView(ZeroNode node) : base(NodeEditorConfig.NODE_VIEW_UXML)
        {
            this.node = node;
            base.title = Regex.Replace(node.name, "[0-9]", "");
            viewDataKey = node.guid;
            style.left = node.position.x;
            style.top = node.position.y;
            SetNodeViewContent();
            SetNodeViewContent();
            SetNodeViewStyle();
            CreateInputPorts();
            CreateOutputPorts();

            nodeState = this.Q("NodeState");
        }

        private void SetNodeViewContent()
        {
            //设置描述
            bool hasChinese = Regex.IsMatch(node.description, @"[\u4e00-\u9fa5]");
            int minLength = 18;
            if (hasChinese) minLength = 10;
            var descElem = this.Q<Label>("description");
            descElem.tooltip = node.description;
            int descLen = node.description.Length;
            if (descLen <= minLength)
            {
                descElem.text = node.description;
            }
            else
            {
                descElem.text = node.description.Substring(0, hasChinese? minLength - 1 : minLength - 2) + "...";
            }
        }

        //创建输入端口
        private void CreateInputPorts()
        {
            if (node.type == ZeroNode.Type.Single2Single || node.type == ZeroNode.Type.Single2Many)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
            }
            input.portName = "";
            inputContainer.Add(input);
        }
        
        //创建输出端口
        private void CreateOutputPorts()
        {
            if (node.type == ZeroNode.Type.Single2Single || node.type == ZeroNode.Type.Many2Single)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            output.portName = "";
            outputContainer.Add(output);
        }
        
        //节点样式
        private void SetNodeViewStyle()
        {
            switch (node.type)
            {
                case ZeroNode.Type.Single2Single:
                    this.Q("Head").AddToClassList("single2single");
                    break;
                case ZeroNode.Type.Many2Many:
                    this.Q("Head").AddToClassList("many2many");
                    break;
                case ZeroNode.Type.Single2Many:
                    this.Q("Head").AddToClassList("single2many");
                    break;
                case ZeroNode.Type.Many2Single:
                    this.Q("Head").AddToClassList("many2single");
                    break;
            }
        }
        
        //状态样式
        public void SetNodeStateStyle()
        {
            if (Application.isPlaying)
            {
                switch(node.currentState)
                {
                    case ZeroNode.State.Running:
                        nodeState.AddToClassList("running");
                        break;
                    default:
                        nodeState.RemoveFromClassList("running");
                        break;
                }
            }
        }
        
        //当节点选中时触发
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            OnNodeSelected?.Invoke(null);
        }

        //当设置位置时触发
        public override void SetPosition(Rect newPos)
        {
            Undo.RecordObject(node,"NodeView (Set Position)");
            base.SetPosition(newPos);
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }
    }
}