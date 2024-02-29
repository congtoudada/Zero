/****************************************************
  文件：EditorUndoData.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 14:16:43
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Zero.Editor
{
    public class EditorUndoData : SerializedScriptableObject
    {
        public Dictionary<string, string> UndoDict = new Dictionary<string, string>(17); //用于Undo对象缓存字段
    }
}