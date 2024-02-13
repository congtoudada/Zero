/****************************************************
  文件：IFileKit.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：2021/11/14 16:29:07
  功能：文件操作工具类
*****************************************************/

namespace Zero.Utility
{
    /// <summary>
    /// 文件工具接口
    /// </summary>
    public interface IFileKit : IUtility
    {
        BytesTool BytesTool { get; }
        ImageTool ImageTool { get; }
        JsonTool JsonTool { get; }
        TextTool TextTool { get; }
        YamlTool YamlTool { get; }
        ExcelTool ExcelTool { get; }
        
        /// <summary>
        /// 从Yaml字符串转为Json，再转为对象（适用于复杂对象）
        /// </summary>
        /// <param name="yamlStr"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Yaml2Json2Obj<T>(string yamlStr);
        
        /// <summary>
        /// 从路径读取Yaml字符串，转为Json，再转为对象（适用于复杂对象）
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Path2Yaml2Json2Obj<T>(string path);

    }
}