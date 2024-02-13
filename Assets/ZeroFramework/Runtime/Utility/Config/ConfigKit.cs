/****************************************************
  文件：ConfigKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 14:29:42
  功能: 不能使用框架的日志打印，框架的日志打印需要Config配置！
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Zero.Utility
{
    public class ConfigKit : IUtility, IConfigKit
    {
        private JObject _G;
        
        public JToken this[string key]
        {
            get => Find(_G, key).Item3;
            set
            {
                var result = Find(_G, key);
                if (result.Item1 != null)
                    result.Item1[result.Item2] = value;
            }
        }
        
        /// <summary>
        /// 根据Key找到最小单元的JObject（JObject本质是一个Dictionary）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (JObject, string, JToken) Find(JObject _G, string key)
        {
            string[] keys = key.Split('.');
            if (keys.Length < 1) return (null, null, null);
            if (keys.Length == 1)
            {
                if (_G.TryGetValue(key, out var value))
                    return (_G, key, value);
                return (null, null, null);
            }

            JObject jObj = _G;
            string keyItem = null;
            JToken valItem = null;
            for (int i = 0; i < keys.Length; i++)
            {
                if (jObj.TryGetValue(keys[i], out var item))
                {
                    keyItem = keys[i];
                    valItem = item;
                    if (valItem is JObject new_jObj)
                    {
                        jObj = new_jObj;
                    }
                }
                else
                {
                    return (null, null, null);
                }
            }
            return (jObj, keyItem, valItem);
        }
        
        // 重写 ToString 方法  
        public override string ToString()  
        {  
            return _G.ToString();
        }  
        
        #region 实现IConfigKit
        public IConfigKit Init(ConfigInfo configInfo)
        {
            switch (configInfo.loadMethod)
            {
                case ConfigInfo.LoadEnum.SCRIPTABLE:
                    _G = new ScriptableConfigHelper().Resolve(configInfo)?.OverrideConfig(_G);
                    break;
                case ConfigInfo.LoadEnum.YAML:
                    _G = new YamlConfigHelper().Resolve(configInfo)?.OverrideConfig(_G);
                    break;
                case ConfigInfo.LoadEnum.JSON:
                    _G = new JsonConfigHelper().Resolve(configInfo)?.OverrideConfig(_G);
                    break;
            }
            return this;
        }

        public IConfigKit Init(List<ConfigInfo> configInfoList)
        {
            foreach(var configInfo in configInfoList)
            {
                Init(configInfo);
            }
            return this;
        }

        public T Get<T>(string key)
        {
            var token = this[key];
            if (token != null)
            {
                return token.ToObject<T>();
            }
            return default(T);
        }

        public void Set<T>(string key, T value)
        {
            this[key] = JToken.FromObject(value);
        }

        public List<string> Keys
        {
            get
            {
                List<string> keys = new List<string>();
                List<string> paths = new List<string>();
                StringBuilder stringBuilder = new StringBuilder();
                CreateKeys(_G, paths, keys, stringBuilder);
                return keys;
            }
        }
        
        /// <summary>
        /// 生成所有访问Key
        /// </summary>
        /// <param name="jObj"></param>
        /// <param name="paths"></param>
        /// <param name="result"></param>
        /// <param name="stringBuilder"></param>
        public void CreateKeys(JObject jObj, List<string> paths, List<string> result, StringBuilder stringBuilder)
        {
            foreach (var pair in jObj)
            {
                //如果是字典类，则继续递归（非叶子）
                if (jObj[pair.Key] is JObject subJObj)
                {
                    paths.Add(pair.Key);
                    int idx = paths.Count - 1;
                    CreateKeys(subJObj, paths, result, stringBuilder);
                    paths.RemoveAt(idx); //回溯
                }
                else //如果非字典类则并入结果集（叶子）
                {
                    stringBuilder.Clear(); //每次存结果都清空，本身是初始化过程而非运行时，暂不优化
                    for(int i = 0; i < paths.Count(); i++)
                    {
                        stringBuilder.Append(paths[i]);
                        if (i != paths.Count - 1)
                        {
                            stringBuilder.Append(".");
                        }
                    }
                    result.Add(stringBuilder.ToString());
                }
            }
        }
        #endregion
    }
}