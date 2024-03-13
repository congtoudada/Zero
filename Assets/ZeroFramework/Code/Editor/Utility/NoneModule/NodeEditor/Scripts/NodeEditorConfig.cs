/****************************************************
  文件：NodeEditorConfig.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-01-08 17:29:01
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Zero.Utility;

namespace Zero.Editor
{
    /// <summary>
    /// 节点编辑器需要的常量
    /// </summary>
    public class NodeEditorConfig
    {
        //NodeEditor
        public static string NODE_EDITOR_UXML;
        public static string NODE_EDITOR_USS;
        //NodeTreeViewer
        public static string NODE_TREE_VIEWER_USS;
        //NodeView
        public static string NODE_VIEW_UXML;
        
        static NodeEditorConfig()
        {
            // NODE_EDITOR_UXML = ZeroToolKits.Instance._EG.Get<string>("utility.node_editor.uxml");
            // NODE_EDITOR_USS = ZeroToolKits.Instance._EG.Get<string>("utility.node_editor.uss");
            // NODE_VIEW_UXML = ZeroToolKits.Instance._EG.Get<string>("utility.node_editor.view_uxml");
            // NODE_TREE_VIEWER_USS = ZeroToolKits.Instance._EG.Get<string>("utility.node_editor.viewer_uss");
            NODE_EDITOR_UXML = ZeroToolKits.Instance._EG.Get<string>(ZeroEditorConfigKey.UTILITY__NODE_EDITOR__UXML);
            NODE_EDITOR_USS = ZeroToolKits.Instance._EG.Get<string>(ZeroEditorConfigKey.UTILITY__NODE_EDITOR__USS);
            NODE_VIEW_UXML = ZeroToolKits.Instance._EG.Get<string>(ZeroEditorConfigKey.UTILITY__NODE_EDITOR__VIEW_UXML);
            NODE_TREE_VIEWER_USS = ZeroToolKits.Instance._EG.Get<string>(ZeroEditorConfigKey.UTILITY__NODE_EDITOR__VIEWER_USS);
            // Debug.Log(NODE_EDITOR_UXML);
            // Debug.Log(NODE_EDITOR_USS);
            // Debug.Log(NODE_VIEW_UXML);
            // Debug.Log(NODE_TREE_VIEWER_USS);
        }
    }
}