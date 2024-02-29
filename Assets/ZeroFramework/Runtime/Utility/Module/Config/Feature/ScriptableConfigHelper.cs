/****************************************************
  文件：ScriptableConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 01:39:55
  功能：
*****************************************************/

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero.Utility
{
    public class ScriptableConfigHelper : BaseConfigHelper
    {
        public override JObject Resolve(ConfigInfo configInfo)
        {
            base.Resolve(configInfo);
            if (configInfo.fileType != ConfigInfo.FileType.SCRIPTABLE) return null;
            string root = Resolve2Json(configInfo.location);
            return Resolve2JObject(root, KEYWORD_INCLUDE);
        }

        protected override string Resolve2Json(string location)
        {
            if (loadPathTrace.Contains(Path.GetFileName(location))) //存在环路则忽略
            {
                return null;
            }
            //加载ScriptableObject
            ZeroConfig content = null;
            switch (configInfo.loadType)
            {
                case ConfigInfo.LoadType.ZERO_RES:
                    content = resKit.GetPackage().LoadAssetSync<ZeroConfig>(location).AssetObject as ZeroConfig;
                    break;
                case ConfigInfo.LoadType.UNITY_WEB_REQUEST:
                    Debug.LogError("错误: Scriptable资产不能使用UnityWebRequest解析");
                    break;
                case ConfigInfo.LoadType.RESOURCES:
                    content = Resources.Load<ZeroConfig>(location);
                    break;
            }
            //加载ScriptableObject->Json
            if (content == null) return null;
            string json = null;
            JObject result = null;
            if (content.configs.Count > 0)
            {
                json = fileKit.JsonTool.SerializeObject(content.configs);
                result = JsonConvert.DeserializeObject<JObject>(json);
            }
            if (content.configsTable.Count > 0)
            {
                json = fileKit.JsonTool.SerializeObject(content.configsTable);
                result?.Merge(JsonConvert.DeserializeObject<JObject>(json));
            }
            if (content.INCLUDE.Count > 0)
            {
                Dictionary<string, List<string>> include = new Dictionary<string, List<string>>(1)
                {
                    { KEYWORD_INCLUDE,  content.INCLUDE},
                };
                json = fileKit.JsonTool.SerializeObject(include);
                result?.Merge(JsonConvert.DeserializeObject<JObject>(json));
            }
            json = JsonConvert.SerializeObject(result);
            //预处理
            json = ReConstructConfigBefore(json);
            return json;
        }
    }
}