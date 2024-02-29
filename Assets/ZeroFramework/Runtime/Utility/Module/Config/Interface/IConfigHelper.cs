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
        /// <summary>
        /// 解析ConfigInfo，生成配置对象
        /// </summary>
        /// <param name="configInfo"></param>
        /// <returns></returns>
        JObject Resolve(ConfigInfo configInfo);

        /// <summary>
        /// 预处理字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string ReConstructConfigBefore(string input);
    }
}