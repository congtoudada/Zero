/****************************************************
  文件：JsonConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 01:46:09
  功能：
*****************************************************/

using System.IO;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero.Utility
{
    public class JsonConfigHelper : BaseConfigHelper<JsonConfigHelper>
    {
        public override JObject Resolve(ConfigInfo configInfo)
        {
            if (configInfo.loadMethod != ConfigInfo.LoadEnum.JSON) return null;
            string root = Resolve2Json(configInfo.location);
            return Resolve2JObject(root, "INCLUDE");
        }

        protected override string Resolve2Json(string location)
        {
            if (loadPathTrace.Contains(Path.GetFileName(location))) //存在环路则忽略
            {
                return null;
            }
            //加载Json
            var content = (resKit.GetPackage().LoadAssetSync<TextAsset>(location).AssetObject as TextAsset)?.text;
            if (content == null) return null;
            //预处理
            string json = ReConstructConfigBefore(content);
            return json;
        }
    }
}