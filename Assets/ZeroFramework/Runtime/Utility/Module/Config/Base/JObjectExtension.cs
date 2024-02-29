/****************************************************
  文件：JObjectExtension.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 00:00:17
  功能：
*****************************************************/

using System.Collections.Generic;
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
        public static JObject OverrideConfig(this JObject child, JObject parent)
        {
            if (child == null) return parent;
            if (parent == null) return child;
            foreach (var pair in child)
            {
                if (parent.ContainsKey(pair.Key))
                {
                    //如果是依赖，则根据规则追加
                    if (pair.Key == "INCLUDE")
                    {
                        JArray childDeps = pair.Value?.Value<JArray>();
                        if (childDeps != null)
                        {
                            foreach (var dep in childDeps)
                            {
                                parent[pair.Key].Value<JArray>().Add(dep);
                            }
                        }
                    }
                    else
                    {
                        //如果是字典类，则继续递归
                        if (parent[pair.Key] is JObject)
                        {
                            parent[pair.Key] = (child[pair.Key] as JObject)?.OverrideConfig(parent[pair.Key] as JObject);
                        }
                        else //如果非字典类则直接更新
                        {
                            parent[pair.Key] = pair.Value;
                        }
                    }
                }
                else
                {
                    parent.Add(pair.Key, pair.Value);
                }
            }
            return parent;
        }
    }
}