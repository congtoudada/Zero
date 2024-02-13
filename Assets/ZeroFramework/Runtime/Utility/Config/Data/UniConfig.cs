/****************************************************
  文件：UniConfig.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/5 14:59:54
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero.Utility
{
    [CreateAssetMenu(menuName=("Zero/UniConfig"), fileName=("UniConfig_"))]
    public class UniConfig : SerializedScriptableObject
    {
        [Header("前缀")]
        public string prefix;
        
        [Header("值数据")]
        public Dictionary<string, string> configs = new Dictionary<string, string>();
        
        [Header("表数据")]
        public Dictionary<string, List<string>> configsTable = new Dictionary<string, List<string>>();
        
        [Header("依赖项")]
        public List<string> INCLUDE = new List<string>();
    }
}
