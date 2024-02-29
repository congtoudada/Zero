/****************************************************
  文件：ZeroSubMenu.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 12:50:28
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Zero.Utility;
using Object = UnityEngine.Object;

namespace Zero.Editor
{
    public abstract class ZeroSubMenu : IController
    {
        [Utility.ReadOnly]
        public EditorUndoData undoData;
        // private Dictionary<string, List<string>> runtimeAttributes;
        private ReConstructConfigHelper processor;

        public ZeroSubMenu()
        {
            undoData = ScriptableObject.CreateInstance<EditorUndoData>();
            processor = new ReConstructConfigHelper();
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
            PreProcess(); //预处理
            LoadCache(); //从Model读缓存
            UpdateToUndoDict(); //更新到UndoDict
        }

        private void OnUndoRedoPerformed()
        {
            UpdateToWindow();
        }
        
        /// <summary>
        /// 对所有字段进行预处理，提供合法的默认值（主要是路径）
        /// </summary>
        private void PreProcess()
        {
            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            //动态添加特性
            // runtimeAttributes = new Dictionary<string, List<string>>(fields.Length);
            // foreach (FieldInfo field in fields)
            // {
            //     runtimeAttributes.Add(field.Name, new List<string>());
            //     // 检查字段是否已经有EditorUndoAttribute特性
            //     if (field.GetCustomAttribute<EditorUndoAttribute>() == null)
            //     {
            //         runtimeAttributes[field.Name].Add("Undo");
            //     }
            //     // 检查字段是否已经有OnValueChanged特性
            //     // if (field.GetCustomAttribute<OnValueChangedAttribute>() == null)
            //     // {
            //     //     runtimeAttributes[field.Name].Add("OnValueChanged");
            //     // }
            // }
            //
            
            //动态解析路径
            foreach (var field in fields)
            {
                // Debug.Log(field.Name);
                if (field.FieldType == typeof(string))
                {
                    field.SetValue(this, processor.ReConstructConfigBefore(field.GetValue(this)?.ToString()));
                }
            }
        }

        protected void LoadCache()
        {
            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var storage = this.GetUtility<ZeroToolKits>().Storage;
            foreach (var field in fields)
            {
                //如果含有EditorCacheAttribute，就尝试从字典中读取
                if (field.IsDefined(typeof(EditorCacheAttribute), false))
                {
                    string key = type.Name + "_" + field.Name;
                    string val = storage.LoadString(key, field.GetValue(this)?.ToString());
                    if (!string.IsNullOrEmpty(val))
                    {
                        if (field.FieldType == typeof(bool)) //如果属性是bool类型
                        {
                            field.SetValue(this, Convert.ToBoolean(val));
                        }
                        else
                        {
                            field.SetValue(this, val);
                        }
                    }
                }
            }
        }

        protected void SaveCache()
        {
            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var storage = this.GetUtility<ZeroToolKits>().Storage;
            foreach (var field in fields)
            {
                //如果含有EditorCacheAttribute，就尝试从字典中读取
                if (field.IsDefined(typeof(EditorCacheAttribute), false))
                {
                    string key = type.Name + "_" + field.Name;
                    storage.SaveString(key, field.GetValue(this)?.ToString());
                }
            }
        }

        //更新所有字段进Undo
        protected void UpdateToUndoDict()
        {
            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
            {
                string key = field.Name;
                //如果含有OnValueChangedAttribute，就尝试从字典中读取
                if (field.IsDefined(typeof(OnValueChangedAttribute), false))
                {
                    if (undoData.UndoDict.ContainsKey(key))
                    {
                        undoData.UndoDict[key] = field.GetValue(this)?.ToString();
                    }
                    else
                    {
                        undoData.UndoDict.Add(key, field.GetValue(this)?.ToString());
                    }
                }
            }
        }
        
        protected void UpdateToWindow()
        {
            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
            {
                string key = field.Name;
                //如果含有OnValueChangedAttribute，就尝试从字典中读取
                if (field.IsDefined(typeof(OnValueChangedAttribute), false))
                {
                    if (field.FieldType == typeof(bool)) //如果属性是bool类型
                    {
                        field.SetValue(this, Convert.ToBoolean(undoData.UndoDict[key]));
                    }
                    else
                    {
                        field.SetValue(this, undoData.UndoDict[key]);
                    }
                }
            }
        }

        public void OnDestroy()
        {
            SaveCache();
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            Object.DestroyImmediate(undoData);
        }
        
        protected void UndoRecord()
        {
            Undo.RecordObject(undoData, "UndoRecord");
            this.UpdateToUndoDict();
        }

        protected string Relative2Absolute(string relativePath)
        {
            return this.GetUtility<ZeroToolKits>().PathHelper.AssetsRelativeToAbsolute(relativePath);
        }
        
        public IArchitecture GetArchitecture()
        {
            return ZeroEditorArchitecture.Interface;
        }
    }
}