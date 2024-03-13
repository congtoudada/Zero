/****************************************************
  文件：CustomScriptableConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-27 19:54:42
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
    public class CustomScriptableConfigHelper : BaseConfigHelper
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
            SerializedScriptableObject content = null;
            switch (configInfo.loadType)
            {
                case ConfigInfo.LoadType.ZERO_RES:
                    content = resKit.GetPackage().LoadAssetSync<SerializedScriptableObject>(location).AssetObject as SerializedScriptableObject;
                    break;
                case ConfigInfo.LoadType.RESOURCES:
                    content = Resources.Load<SerializedScriptableObject>(location);
                    break;
            }
            //加载ScriptableObject->Json
            if (content == null) return null;
            string json = JsonConvert.SerializeObject(content);
            //预处理
            json = ReConstructConfigBefore(json);
            return json;
        }
    }
}