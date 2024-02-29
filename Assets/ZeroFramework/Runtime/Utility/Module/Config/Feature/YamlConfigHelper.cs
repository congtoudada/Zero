/****************************************************
  文件：YamlConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 00:22:56
  功能：
*****************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Zero.Utility
{
    public class YamlConfigHelper : BaseConfigHelper
    {
        public override JObject Resolve(ConfigInfo configInfo)
        {
            base.Resolve(configInfo);
            if (configInfo.fileType != ConfigInfo.FileType.YAML) return null;
            string root = Resolve2Json(configInfo.location);
            return Resolve2JObject(root, KEYWORD_INCLUDE);
        }
        protected override string Resolve2Json(string location)
        {
            if (loadPathTrace.Contains(Path.GetFileName(location))) //存在环路则忽略
            {
                return null;
            }
            //加载Yaml
            string content = null;
            switch (configInfo.loadType)
            {
                case ConfigInfo.LoadType.ZERO_RES:
                    content = (resKit.GetPackage().LoadAssetSync<TextAsset>(location).AssetObject as TextAsset)?.text;
                    break;
                case ConfigInfo.LoadType.UNITY_WEB_REQUEST:
                    content = fileKit.TextTool.ReadFromUri("file://" + location);
                    break;
                case ConfigInfo.LoadType.RESOURCES:
                    content = Resources.Load<TextAsset>(location)?.text;
                    break;
            }
            if (content == null) return null;
            //Yaml->Json
            string json = fileKit.YamlTool.YamlToJson(content);
            //预处理
            json = ReConstructConfigBefore(json);
            return json;
        }
        
    }
}