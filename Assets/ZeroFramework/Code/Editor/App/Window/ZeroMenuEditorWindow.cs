/****************************************************
  文件：ZeroMenuEditorWindow.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/25 15:08:25
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Zero.Utility;

namespace Zero.Editor
{
    /// <summary>
    /// Zero框架编辑器拓展的窗口类
    /// </summary>
    public class ZeroMenuEditorWindow : OdinMenuEditorWindow, IController
    {
        [MenuItem("Zero框架/内置工具", priority = 1)]
        private static void OpenWindow()
        {
            GetWindow<ZeroMenuEditorWindow>("Zero Window").Show();
        }

        [MenuItem("Zero框架/清除编辑器缓存", priority = 100)]
        private static void DeleteEditorCache()
        {
            ZeroEditorArchitecture.Interface.GetUtility<ZeroToolKits>().Storage.DeleteAll();
            Debug.Log("清除编辑器缓存成功！");
        }
        
        [MenuItem("Zero框架/打开Persistent目录", priority = 101)]
        private static void OpenPersistent()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            
            // this.GetUtility<ZeroToolKits>().Storage.DeleteAll(); //清空所有本地Key-Value缓存
            
            tree.Add("ConfigKey生成", new ConfigMenu());
            tree.Add("UI", new UIMenu());
            // tree.Add("AssetBundle资源清单", new ABUtilityEditor(dict));
            // tree.Add("AuKey一键生成", new AuAutoKeyEditor(dict));
            
            return tree;
        }

        protected override void OnDestroy()
        {
            foreach (var item in MenuTree.MenuItems)
            {
                (item.Value as ZeroSubMenu)?.OnDestroy();
            }
            base.OnDestroy();
        }


        public IArchitecture GetArchitecture()
        {
            return ZeroEditorArchitecture.Interface;
        }
    }
}