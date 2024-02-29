/****************************************************
  文件：InspectorViewer.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 17:15:20
  功能：
*****************************************************/

using UnityEditor;
using UnityEngine.UIElements;

namespace Zero.Editor
{
    /// <summary>
    /// Inspector视图
    /// </summary>
    public class InspectorViewer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorViewer, VisualElement.UxmlTraits>{}

        private UnityEditor.Editor _editor;

        public void UpdateSelection(NodeTreeViewer nodeTreeViewer, NodeView nodeView)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            if (nodeView != null && nodeView.node != null)
            {
                _editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            }
            else if (nodeTreeViewer != null && nodeTreeViewer.tree != null)
            {
                _editor = UnityEditor.Editor.CreateEditor(nodeTreeViewer.tree);
            }

            if (_editor != null)
            {
                IMGUIContainer container = new IMGUIContainer(() =>
                {
                    if (_editor.target)
                        _editor.OnInspectorGUI();
                });
                Add(container);
            }
        }
    }
}