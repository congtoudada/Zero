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

namespace Zero.Editor
{
    /// <summary>
    /// 节点编辑器需要的常量
    /// </summary>
    public class NodeEditorConfig : UnityEditor.AssetModificationProcessor//必须继承这个
    {
        private static string ZERO_LOCATION;
        private const string NODE_EDITOR_LOCATION = "Editor/Utility/NodeEditor/";
        //NodeEditor
        public static string NODE_EDITOR_UXML;
        public static string NODE_EDITOR_USS;
        //NodeTreeViewer
        public static string NODE_TREE_VIEWER_USS;
        //NodeView
        public static string NODE_VIEW_UXML;
        
        /// <summary>
        /// 监听资源移动事件
        /// </summary>
        /// <param name="oldPath">旧路径</param>
        /// <param name="newPath">新路径</param>
        /// <returns></returns>
        public static AssetMoveResult OnWillMoveAsset(string oldPath,string newPath)
        {
            if (oldPath.EndsWith("ZeroFramework"))
            {
                ZERO_LOCATION = newPath;
                UpdatePath();
                // NODE_EDITOR_UXML = ROOT_PATH + "UXML/NodeEditor.uxml";
                // NODE_EDITOR_USS = ROOT_PATH + "USS/NodeEditor.uss";
                // NODE_TREE_VIEWER_USS = ROOT_PATH + "/USS/NodeTreeViewer.uss";
                // NODE_VIEW_UXML = ROOT_PATH + "/UXML/NodeView.uxml";
            }
            // Debug.LogFormat($"移动资源！从旧路径:{oldPath}到新路径:{newPath}");
            //AssetMoveResult.DidNotMove表示该资源可以移动，Didmove表示不能移动   
            return AssetMoveResult.DidNotMove;
        }

        static NodeEditorConfig()
        {
            ZERO_LOCATION = "Assets/ZeroFramework/";
            UpdatePath();
        }

        private static void UpdatePath()
        {
            string curLocation = Path.Combine(ZERO_LOCATION, NODE_EDITOR_LOCATION);
            // NODE_EDITOR_UXML = Path.Combine(curLocation + "UXML/NodeEditor.uxml");
            // NODE_EDITOR_USS = Path.Combine(curLocation, "USS/NodeEditor.uss");
            // NODE_TREE_VIEWER_USS = Path.Combine(curLocation, "/USS/NodeTreeViewer.uss");
            // NODE_VIEW_UXML = Path.Combine(curLocation, "/UXML/NodeView.uxml");
            NODE_EDITOR_UXML = curLocation + "UXML/NodeEditor.uxml";
            NODE_EDITOR_USS = curLocation + "USS/NodeEditor.uss";
            NODE_TREE_VIEWER_USS = curLocation + "USS/NodeTreeViewer.uss";
            NODE_VIEW_UXML = curLocation + "UXML/NodeView.uxml";
        }
    }
}