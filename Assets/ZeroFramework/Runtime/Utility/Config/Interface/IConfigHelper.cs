/****************************************************
  文件：IConfigHelper.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 00:23:27
  功能：
*****************************************************/

using Newtonsoft.Json.Linq;

namespace Zero.Utility
{
    public interface IConfigHelper
    {
        JObject Resolve(ConfigInfo configInfo);
    }
}