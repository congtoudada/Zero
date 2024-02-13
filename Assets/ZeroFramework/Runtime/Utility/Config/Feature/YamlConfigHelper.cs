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
    public class YamlConfigHelper : BaseConfigHelper<YamlConfigHelper>
    {
        public override JObject Resolve(ConfigInfo configInfo)
        {
            if (configInfo.loadMethod != ConfigInfo.LoadEnum.YAML) return null;
            string root = Resolve2Json(configInfo.location);
            return Resolve2JObject(root, "INCLUDE");
        }
        protected override string Resolve2Json(string location)
        {
            if (loadPathTrace.Contains(Path.GetFileName(location))) //存在环路则忽略
            {
                return null;
            }
            //加载Yaml
            var content = (resKit.GetPackage().LoadAssetSync<TextAsset>(location).AssetObject as TextAsset)?.text;
            if (content == null) return null;
            //Yaml->Json
            string json = fileKit.YamlTool.YamlToJson(content);
            //预处理
            json = ReConstructConfigBefore(json);
            return json;
        }
        
    }
}