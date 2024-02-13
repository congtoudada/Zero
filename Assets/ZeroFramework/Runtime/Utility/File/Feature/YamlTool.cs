/****************************************************
  文件：YamlTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/27 19:51:22
  功能：
*****************************************************/
using System.IO;
using YamlDotNet.Serialization;

namespace Zero.Utility
{
    /// <summary>
    /// 文件模块：Yaml工具（借助YamlDotNet）
    /// </summary>
    public class YamlTool
    {
        /// <summary>
        /// Yaml字符串转Json字符串
        /// </summary>
        /// <param name="yaml"></param>
        /// <returns></returns>
        public string YamlToJson(string yaml)
        {
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize(new StringReader(yaml));
            if (yamlObject != null)
            {
                var serializer = new SerializerBuilder()
                    .JsonCompatible()
                    .Build();
                return serializer.Serialize(yamlObject);
            }
            return "";
        }
        
        /// <summary>
        /// 对象序列化成Yaml字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerializeObject(object obj)
        {
            var serializer = new Serializer();
            var yaml = serializer.Serialize(obj);
            return yaml;
        }
        
        /// <summary>
        /// Yaml字符串反序列化成对象
        /// </summary>
        /// <param name="yaml"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T DeserializeObject<T>(string yaml)
        {
            var deserializer = new Deserializer();
            return deserializer.Deserialize<T>(yaml);
        }
    }
}