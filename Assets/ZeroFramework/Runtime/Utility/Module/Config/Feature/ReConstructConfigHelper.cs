/****************************************************
  文件：ReConstructConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 16:45:51
  功能：只提供字符串处理业务
*****************************************************/

using Newtonsoft.Json.Linq;

namespace Zero.Utility
{
    public class ReConstructConfigHelper : BaseConfigHelper
    {
        public override JObject Resolve(ConfigInfo configInfo)
        {
            return null;
        }

        protected override string Resolve2Json(string location)
        {
            return null;
        }
    }
}