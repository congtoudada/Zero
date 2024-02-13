/****************************************************
  文件：BaseConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 00:26:37
  功能：
*****************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Zero.Utility
{
    public static class JObjectExtension
    {
        /// <summary>
        /// 配置重载。对于oldObj，newObj新增的字段添加，相同字段覆盖
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="oldObj"></param>
        /// <returns></returns>
        public static JObject OverrideConfig(this JObject newObj, JObject oldObj)
        {
            if (newObj == null) return oldObj;
            if (oldObj == null) return newObj;
            foreach (var pair in newObj)
            {
                if (oldObj.ContainsKey(pair.Key))
                {
                    //如果是字典类，则继续递归
                    if (oldObj[pair.Key] is JObject)
                    {
                        oldObj[pair.Key] = (newObj[pair.Key] as JObject)?.OverrideConfig(oldObj[pair.Key] as JObject);
                    }
                    else //如果非字典类则直接更新
                    {
                        oldObj[pair.Key] = pair.Value;
                    }
                }
                else
                {
                    oldObj.Add(pair.Key, pair.Value);
                }
            }
            return oldObj;
        }
    }
    
    public abstract class BaseConfigHelper<T> : IConfigHelper where T : BaseConfigHelper<T>
    {
        private static Dictionary<string, string> _replaceRule;
        protected ILogger logger;
        protected IYooResKit resKit;
        protected IFileKit fileKit;
        protected List<string> loadPathTrace;

        static BaseConfigHelper()
        {
            _replaceRule = new Dictionary<string, string>()
            {
                { "Streaming".ToLower(), Application.streamingAssetsPath },
                { "Persistent".ToLower(), Application.persistentDataPath },
                { "StreamingPath".ToLower(), Application.streamingAssetsPath },
                { "PersistentPath".ToLower(), Application.persistentDataPath },
                { "StreamingAssetsPath".ToLower(), Application.streamingAssetsPath },
                { "PersistentDataPath".ToLower(), Application.persistentDataPath },
                { "ZeroAbsolutePath".ToLower(), Path.Combine(Application.dataPath, "ZeroFramework") },
                { "ZeroRelativePath".ToLower(), Path.Combine("Assets", "ZeroFramework") }
            };
        }

        public BaseConfigHelper()
        {
            logger = ZeroToolKits.Instance.InnerLog.AllocateLogger(typeof(T), "[ Config ]");
            resKit = ZeroToolKits.Instance.YooRes;
            fileKit = ZeroToolKits.Instance.File;
            loadPathTrace = new List<string>();
        }

        public abstract JObject Resolve(ConfigInfo configInfo);

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
            JObject oldObj = fileKit.JsonTool.DeserializeObject<JObject>(json);
            // 解析依赖项
            JObject newObj = null;
            List<JToken> dependenciesLocationToken = null;
            if (oldObj != null && oldObj.ContainsKey(includeKeyword))
            {
                dependenciesLocationToken = oldObj[includeKeyword]?.ToList();
            }
            if (dependenciesLocationToken != null)
            {
                foreach (var dependencyLocationToken in dependenciesLocationToken)
                {
                    string dependencyLocation = dependencyLocationToken.Value<string>();
                    string depJson = Resolve2Json(dependencyLocation);
                    loadPathTrace.Add(Path.GetFileName(dependencyLocation));
                    newObj = Resolve2JObject(depJson, includeKeyword);
                    oldObj = newObj.OverrideConfig(oldObj); //不断用newObj去覆盖扩展oldObj
                }
            }
            return oldObj;
        }

        /// <summary>
        /// 预处理。处理特定语法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected string ReConstructConfigBefore(string input)
        {
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