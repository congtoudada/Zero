/****************************************************
  文件：Bind.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/29 23:27:58
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

namespace Zero.Utility
{
    [ExecuteInEditMode]
    public class Bind : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum Method
        {
            Auto,
            Custom
        }
        [LabelText("绑定方式")] public Method bindMethod;

        [LabelText("自动绑定类型"), ShowIf("bindMethod", Method.Auto), ValueDropdown("_bindComponents")]
        [OnValueChanged("OnAutoBindChanged")]
        public string autoBindComponent;
        [SerializeField, HideInInspector]
        private int _lastAutoIndex = -1;

        [LabelText("手动绑定类型"), ShowIf("bindMethod", Method.Custom)]
        public string customBindComponent;
        
        [LabelText("属于"), ValueDropdown("GetBindPoints"), ShowInInspector]
        [OnValueChanged("OnBindPointChanged"), InlineButton("SelectBindPoint", " 选择 ")]
        public string bindPoint;
        [SerializeField, HideInInspector]
        private string _lastPointName = "";

        [LabelText("访问修饰符")]
        public ScriptBuilder.AccessEnum accessEnum = ScriptBuilder.AccessEnum.PUBLIC;
        
        [LabelText("字段名称（可选）"), Tooltip("无输入则默认绑定的类型名")]
        public string fieldName;

        [LabelText("注释（可选）"), Multiline(2)] public string annotation = null;

        private List<string> _bindComponents;
        private Dictionary<string, IBindPoint> _bindPoints;


        private void Reset()
        {
            OnDisable();
            OnEnable();
        }

        private void OnEnable()
        {
            //自动绑定组件
            var comps = GetComponents<Component>();
            _bindComponents = new List<string>(comps.Length);
            foreach (var comp in comps)
            {
                _bindComponents.Add(comp.GetType().FullName);
            }
            if (_lastAutoIndex >= 0 && _lastAutoIndex < _bindComponents.Count)
            {
                autoBindComponent = _bindComponents[_lastAutoIndex];
            }
            else
            {
                autoBindComponent = _bindComponents.Last();
            }

            //属于
            var parents = GetComponentsInParent<IBindPoint>();
            _bindPoints = new Dictionary<string, IBindPoint>(parents.Length);
            IBindPoint first = null;
            foreach (var parent in parents)
            {
                if (first == null) first = parent;
                var gameObjectName = parent.GetGameObj().name;
                _bindPoints.Add(gameObjectName, parent);
            }
            if (!string.IsNullOrEmpty(_lastPointName) && _bindPoints.ContainsKey(_lastPointName))
            {
                bindPoint = _lastPointName;
            }
            else
            {
                bindPoint = first?.GetGameObj().name;
            }
        }

        private void OnDisable()
        {
            _bindComponents.Clear();
            _bindPoints.Clear();
        }


        public IBindPoint GetBindPoint(string key)
        {
            return _bindPoints[key];
        }

        // [Button("测试", ButtonSizes.Large)]
        // public void DebugButton()
        // {
        //     // Debug.Log("hello".Substring("hello.world".LastIndexOf(".", StringComparison.Ordinal) + 1));
        // }

        //生成Designer代码
        [Button("生成代码", ButtonSizes.Large)]
        public void CreateScriptButton()
        {
            _bindPoints[bindPoint]?.CreateScriptButton();
            // // Debug.Log("生成代码");
            // Debug.Log(bindPoint.ScriptName);
            // string guid = AssetDatabase.FindAssets(bindPoint.ScriptName + " t:script")?.First();
            // if (guid != null)
            // {
            //     //确认输出路径
            //     string path = AssetDatabase.GUIDToAssetPath(guid); //Assets相对路径
            //     path = ZeroToolKits.Instance.PathHelper.AssetsRelativeToAbsolute(path); //资产绝对路径
            //     string filename = Path.GetFileNameWithoutExtension(path);
            //     string className = filename;
            //     filename += ".Designer.cs";
            //     path = Path.Combine(Path.GetDirectoryName(path)!, filename);
            //
            //     //使用ScriptBuilder生成模板
            //     ScriptBuilder builder = new UnityScriptBuilder();
            //     var scriptInfo = builder.CreateScriptInfo();
            //     scriptInfo.Filename = Path.GetFileNameWithoutExtension(filename);
            //     scriptInfo.ClassName = "public partial class " + className;
            //     // if (File.Exists(path)) scriptInfo.TemplatePath = path;
            //     string content = builder.CreateScript(scriptInfo);
            //     if (string.IsNullOrEmpty(content))
            //     {
            //         return;
            //     }
            //
            //     string outputFieldType = null;
            //     string outputFieldName = null;
            //     switch (bindMethod)
            //     {
            //         case Method.Auto:
            //             outputFieldType = autoBindComponent;
            //             if (string.IsNullOrEmpty(fieldName))
            //             {
            //                 int lst = autoBindComponent.LastIndexOf(".", StringComparison.Ordinal);
            //                 if (lst != -1)
            //                     outputFieldName = autoBindComponent.Substring(lst + 1);
            //                 else
            //                     outputFieldName = autoBindComponent;
            //             }
            //             else
            //             {
            //                 outputFieldName = fieldName;
            //             }
            //
            //             break;
            //         case Method.Custom:
            //             outputFieldType = customBindComponent;
            //             outputFieldName = string.IsNullOrEmpty(fieldName) ? customBindComponent : fieldName;
            //             break;
            //     }
            //
            //     builder.AddField(ScriptBuilder.AccessEnum.PUBLIC, outputFieldType,
            //         outputFieldName, annotation: annotation);
            //
            //     content = builder.FillField(content);
            //
            //     //写入本地
            //     ZeroToolKits.Instance.File.TextTool.Write(path, content);
            //     AssetDatabase.Refresh();
            //     EditorUtility.RevealInFinder(path);
            // }
        }
        
        private IEnumerable<string> GetBindPoints()
        {
            return _bindPoints.Keys;
        }
        
        public void SelectBindPoint()
        {
            Debug.Log(bindPoint);
            Debug.Log(bindPoint);
        }

        // public void AddSelf(ScriptBuilder builder)
        // {
        //     string outputFieldType = null;
        //     string outputFieldName = null;
        //     switch (bindMethod)
        //     {
        //         case Method.Auto:
        //             outputFieldType = autoBindComponent;
        //             if (string.IsNullOrEmpty(fieldName))
        //             {
        //                 int lst = autoBindComponent.LastIndexOf(".", StringComparison.Ordinal);
        //                 if (lst != -1)
        //                     outputFieldName = autoBindComponent.Substring(lst + 1);
        //                 else
        //                     outputFieldName = autoBindComponent;
        //             }
        //             else
        //             {
        //                 outputFieldName = fieldName;
        //             }
        //
        //             break;
        //         case Method.Custom:
        //             outputFieldType = customBindComponent;
        //             outputFieldName = string.IsNullOrEmpty(fieldName) ? customBindComponent : fieldName;
        //             break;
        //     }
        //     builder.AddField(ScriptBuilder.AccessEnum.PUBLIC, outputFieldType,
        //         outputFieldName, annotation: annotation);
        // }

        public void OnAutoBindChanged()
        {
            for (int i = 0; i < _bindComponents.Count; i++)
            {
                if (autoBindComponent == _bindComponents[i])
                {
                    _lastAutoIndex = i;
                    break;
                }
            }
        }

        public void OnBindPointChanged()
        {
            foreach (string key in _bindPoints.Keys)
            {
                if (bindPoint == key)
                {
                    _lastPointName = key;
                    break;
                }
            }
        }
#endif
    }
}