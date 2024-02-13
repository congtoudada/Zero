/****************************************************
  文件：ScriptableConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 01:39:55
  功能：
*****************************************************/

using System.IO;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Zero.Utility
{
    public class ScriptableConfigHelper : BaseConfigHelper<ScriptableConfigHelper>
    {
        public override JObject Resolve(ConfigInfo configInfo)
        {
            if (configInfo.loadMethod != ConfigInfo.LoadEnum.SCRIPTABLE) return null;
            string root = Resolve2Json(configInfo.location);
            return Resolve2JObject(root, "INCLUDE");
        }

        protected override string Resolve2Json(string location)
        {
            if (loadPathTrace.Contains(Path.GetFileName(location))) //存在环路则忽略
            {
                return null;
            }
            //加载ScriptableObject
            var content = resKit.GetPackage().LoadAssetSync<SerializedScriptableObject>(location).AssetObject as SerializedScriptableObject;
            //加载ScriptableObject->Json
            if (content == null) return null;
            string json = fileKit.JsonTool.SerializeObject(content);
            //预处理
            json = ReConstructConfigBefore(json);
            return json;
        }
    }
}