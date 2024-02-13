/****************************************************
  文件：JsonTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/27 19:51:22
  功能：
*****************************************************/
using Newtonsoft.Json;

namespace Zero.Utility
{
    /// <summary>
    /// 文件模块：Json工具（借助NewtonJson实现）
    /// </summary>
    public class JsonTool
    {
        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerializeObject(object obj)
        {
            #if ZERO_RELEASE
            return JsonConvert.SerializeObject(obj);
            #else
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
            #endif
        }
        
        /// <summary>
        /// 从json字符串中反序列化为对象
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}