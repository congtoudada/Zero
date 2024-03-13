/****************************************************
  文件：BaseConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 00:26:37
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Zero.Utility
{
    public abstract class BaseConfigHelper : IConfigHelper
    {
        public const string KEYWORD_INCLUDE = "INCLUDE";
        private static Dictionary<string, string> _replaceRule;
        protected ILogger logger;
        protected IYooResKit resKit;
        protected IFileKit fileKit;
        protected List<string> loadPathTrace;
        protected ConfigInfo configInfo;

        static BaseConfigHelper()
        {
            _replaceRule = new Dictionary<string, string>()
            {
                //运行时
                { "Streaming".ToLower(), Application.streamingAssetsPath },
                { "Persistent".ToLower(), Application.persistentDataPath },
                { "StreamingPath".ToLower(), Application.streamingAssetsPath },
                { "PersistentPath".ToLower(), Application.persistentDataPath },
                { "StreamingAssetsPath".ToLower(), Application.streamingAssetsPath },
                { "PersistentDataPath".ToLower(), Application.persistentDataPath },
                //编辑器
                // { "ZeroAbsolutePath".ToLower(), Application.dataPath + "/ZeroFramework" },
                // { "ZeroRelativePath".ToLower(), "Assets/ZeroFramework" },
                // { "Zero".ToLower(), "Assets/ZeroFramework" }
            };
#if UNITY_EDITOR
            // string zeroFolder = Directory.GetDirectories("Assets", "ZeroFramework", SearchOption.AllDirectories)[0];
            // _replaceRule.Add("ZeroAbsolutePath".ToLower(), Path.Combine(Path.GetDirectoryName(Application.dataPath)!, zeroFolder).Replace("\\", "/") );
            // _replaceRule.Add("ZeroRelativePath".ToLower(), zeroFolder.Replace("\\", "/") );
            // _replaceRule.Add("Zero".ToLower(), zeroFolder.Replace("\\", "/") );
            
            _replaceRule.Add("ZeroAbsolutePath".ToLower(), ZeroToolKits.Instance.PathHelper.GetZeroFolderAbsolute());
            _replaceRule.Add("ZeroAbsolute".ToLower(), ZeroToolKits.Instance.PathHelper.GetZeroFolderAbsolute());
            _replaceRule.Add("ZeroRelativePath".ToLower(), ZeroToolKits.Instance.PathHelper.GetZeroFolderRelative());
            _replaceRule.Add("Zero".ToLower(), ZeroToolKits.Instance.PathHelper.GetZeroFolderRelative());
#endif
        }

        public BaseConfigHelper()
        {
            // logger = ZeroToolKits.Instance.InnerLog.AllocateLogger(typeof(T), "[ Config ]"); //日志系统依赖配置系统初始化，故不能直接使用
            logger = new UnityLog( "[ Config ]");
        }

        public virtual JObject Resolve(ConfigInfo configInfo)
        {
            this.configInfo = configInfo;
            loadPathTrace = new List<string>();
            fileKit = ZeroToolKits.Instance.File;
            if (this.configInfo.loadType == ConfigInfo.LoadType.ZERO_RES)
            {
                resKit = ZeroToolKits.Instance.YooRes;
            }
            return null;
        }

        protected abstract string Resolve2Json(string location);
        
        /// <summary>
        /// 将Json字符串转换为JObject
        /// </summary>
        /// <param name="json"></param>
        /// <param name="includeKeyword"></param>
        /// <returns></returns>
        protected JObject Resolve2JObject(string json, string includeKeyword)
        {
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
                JObject parent = fileKit.JsonTool.DeserializeObject<JObject>(json);
                // Debug.Log("当前配置信息: " + parent);
                // 解析依赖项
                List<JToken> dependenciesLocation = null;
                if (parent != null && parent.ContainsKey(includeKeyword))
                {
                    dependenciesLocation = parent[includeKeyword]?.ToList(); //获取依赖列表
                    if (parent[includeKeyword] is JArray parentIncludes)
                    {
                        for (int i = 0; i < parentIncludes.Count; i++)
                        {
                        
                            parentIncludes[i] = $"[ {configInfo.fileType.ToString().ToLower()} - " +
                                                $"{configInfo.loadType.ToString().ToLower()} ]: {parentIncludes[i]}";
                        }
                    }
                }
                if (dependenciesLocation != null)
                {
                    foreach (var dependencyLocation in dependenciesLocation)
                    {
                        string location = dependencyLocation.Value<string>();
                        string depJson = Resolve2Json(location);
                        loadPathTrace.Add(Path.GetFileName(location));
                        JObject child = Resolve2JObject(depJson, includeKeyword);
                        if (configInfo.orderType == ConfigInfo.OrderType.BEFORE_INCLUDE)
                            parent = child.OverrideConfig(parent); //不断用children去覆盖或追加parent配置
                        else
                            parent = parent.OverrideConfig(child); //不断用parent去覆盖或追加children配置
                    }
                }
                return parent;
            }
            catch (Exception e)
            {
                Debug.Log("请检查json合法性: " + json);
                Debug.LogError(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 预处理。处理特定语法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ReConstructConfigBefore(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            //替换${}里的内容
            string pattern = "\\$\\{([^}]*)\\}"; // 匹配 ${...} 内的内容  
            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                string content = match.Groups[1].Value.ToLower(); // 提取 ${...} 内的内容  
                if (_replaceRule != null && _replaceRule.TryGetValue(content, out string replacementValue))
                {
                    input = input.Replace(match.Value, replacementValue); // 替换匹配的字符串  
                }
            }
            return input;
        }
    }
}