/****************************************************
  文件：ReadOnlyDrawer.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/8 16:06:55
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zero.Utility;

namespace Zero.Editor
{
    /// <summary>
    /// 只读特性渲染方式
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    } 
}
