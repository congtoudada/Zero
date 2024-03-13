/****************************************************
  文件：ViewController.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-03-01 13:45:41
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Zero.Utility
{
    [ExecuteInEditMode]
    public class ViewController : MonoBehaviour, IBindPoint
    {
        public string ScriptName => this.GetType().Name;
#if UNITY_EDITOR
        [Button("测试", ButtonSizes.Large)]
        public void DebugButton()
        {
            // Debug.Log("hello".Substring("hello.world".LastIndexOf(".", StringComparison.Ordinal) + 1));
        }
        
        //生成Designer代码
        public GameObject GetGameObj()
        {
            return gameObject;
        }

        [Button("生成代码", ButtonSizes.Large)]
        public void CreateScriptButton()
        {
            // Debug.Log("生成代码");
            string guid = AssetDatabase.FindAssets(ScriptName + " t:script")?.First();
            if (guid == null)
            {
                Debug.LogError("获取当前脚本的guid失败，无法生成");
                return;
            }

            //1.确认输出路径
            string path = AssetDatabase.GUIDToAssetPath(guid); //Assets相对路径
            path = ZeroToolKits.Instance.PathHelper.AssetsRelativeToAbsolute(path); //资产绝对路径
            string filename = Path.GetFileNameWithoutExtension(path);
            string className = filename;
            filename += ".Designer.cs";
            path = Path.Combine(Path.GetDirectoryName(path)!, filename);

            //2.使用ScriptBuilder生成模板
            ScriptBuilder builder = new UnityScriptBuilder();
            var scriptInfo = builder.CreateScriptInfo();
            scriptInfo.Filename = Path.GetFileNameWithoutExtension(filename);
            scriptInfo.ClassName = "public partial class " + className;
            // if (File.Exists(path)) scriptInfo.TemplatePath = path;
            string content = builder.CreateScript(scriptInfo);
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            //3.收集目录下所有Bind脚本
            HashSet<string> fieldNameSet = new HashSet<string>();
            Dictionary<string, Bind> cache = new Dictionary<string, Bind>();
            foreach (var bind in GetComponentsInChildren<Bind>())
            {
                if (bind.GetBindPoint(bind.bindPoint).ScriptName == ScriptName)
                {
                    string bindType = "";
                    string fieldName = "";
                    if (bind.bindMethod == Bind.Method.Auto)
                    {
                        bindType = bind.autoBindComponent;
                        fieldName = bind.autoBindComponent.Substring(
                            bind.autoBindComponent.LastIndexOf(".", StringComparison.Ordinal)+1);
                    }
                    else
                    {
                        bindType = bind.customBindComponent;
                        fieldName = bind.fieldName;
                    }
                    
                    //处理字段同名
                    int repeat = 0;
                    string saveFieldName = fieldName;
                    while (fieldNameSet.Contains(saveFieldName))
                    {
                        saveFieldName = fieldName;
                        repeat++;
                        saveFieldName += repeat;
                    }
                    fieldName = saveFieldName;
                    fieldNameSet.Add(fieldName);
                    cache.Add(fieldName, bind);
                    
                    builder.AddField(bind.accessEnum, bindType, fieldName, annotation: bind.annotation);
                }
            }
            content = builder.FillField(content);
            
            //写入本地
            ZeroToolKits.Instance.File.TextTool.Write(path, content);
            
            //批量赋值
            foreach (var key in cache.Keys)
            {
                if (cache[key].bindPoint == gameObject.name)
                {
                    FieldInfo fieldInfo = GetType().GetField(key); // 对于字段
                    if (fieldInfo != null)
                    {
                        Bind bind = cache[key];
                        string bindType = "";
                        if (bind.bindMethod == Bind.Method.Auto)
                        {
                            bindType = bind.autoBindComponent.Substring(bind.autoBindComponent.LastIndexOf(".", StringComparison.Ordinal)+1);
                        }
                        else
                        {
                            bindType = bind.customBindComponent;
                        }
                        // //TODO：赋值 2024年3月11日22:03:28
                        // Debug.Log(bindType);
                        fieldInfo.SetValue(this, bind.GetComponent(bindType));
                    }
                }
            }
            
            AssetDatabase.Refresh();
            EditorUtility.RevealInFinder(path);
        }
#endif
    }
}