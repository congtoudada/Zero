using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using Zero.Utility;

namespace Zero.Editor
{
    /// <summary>
    /// 节点编辑器窗口
    /// </summary>
    public class NodeEditor : EditorWindow
    {
        public NodeTreeViewer nodeTreeViewer;
        public InspectorViewer inspectorViewer;
        
        [MenuItem("ZeroFramework/DialogueEditor")]
        public static void ShowExample()
        {
            NodeEditor wnd = GetWindow<NodeEditor>();
            wnd.titleContent = new GUIContent("NodeEditor");
        }
    
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is NodeTree)
            {
                ShowExample();
                return true;
            }
            return false;
        }
        
        public void CreateGUI()
        {
            try
            {
                // Each editor window contains a root VisualElement object
                VisualElement root = rootVisualElement;
    
                // Import UXML
                var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(NodeEditorConfig.NODE_EDITOR_UXML);
                // Debug.Log(NodeEditorConfig.NODE_EDITOR_UXML);
                visualTree.CloneTree(root);
    
                // Import USS
                var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(NodeEditorConfig.NODE_EDITOR_USS);
                // Debug.Log(NodeEditorConfig.NODE_EDITOR_USS);
                root.styleSheets.Add(styleSheet);
    
                nodeTreeViewer = root.Q<NodeTreeViewer>();
                inspectorViewer = root.Q<InspectorViewer>();
                nodeTreeViewer.OnNodeSelected = OnNodeSelectionChanged;
            }
            catch (Exception e)
            {
                // Debug.LogError($"请检查路径合法性: {NodeEditorConstant.NODE_EDITOR_UXML}");
                Debug.LogError(e.StackTrace);
            }
        }
    
        private void OnNodeSelectionChanged(NodeView view)
        {
            inspectorViewer.UpdateSelection(nodeTreeViewer, view);
        }
        
        private void OnSelectionChange()
        {
            if (Selection.activeObject is NodeTree nodeTree)
            {
                nodeTree.OnDestroyEvent += () =>
                {
                    nodeTreeViewer.PopulateView(null);
                };
                nodeTree.asset = nodeTree;
                nodeTreeViewer.PopulateView(nodeTree);
                inspectorViewer.UpdateSelection(nodeTreeViewer, null);
            }
        }
    
        private void OnInspectorUpdate()
        {
            nodeTreeViewer?.UpdateNodeStates();
        }
    }
}