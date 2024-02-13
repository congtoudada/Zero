/****************************************************
  文件：IConfigKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 14:03:38
  功能: 
*****************************************************/

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Zero.Utility
{
    /// <summary>
    /// 配置类接口
    /// </summary>
    public interface IConfigKit
    {
        /// <summary>
        /// 初始化配置，最终会整合成一套配置
        /// </summary>
        /// <param name="configInfo"></param>
        IConfigKit Init(ConfigInfo configInfo);
        
        /// <summary>
        /// 根据List顺序，初始化配置,，最终会整合成一套配置
        /// </summary>
        /// <param name="configInfoList"></param>
        IConfigKit Init(List<ConfigInfo> configInfoList);
        
        /// <summary>
        /// 获取值，由字符串访问配置 (格式："a.b.c")
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string key);
        
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string key, T value);
        
        /// <summary>
        /// 获取所有Key
        /// </summary>
        List<string> Keys { get; }
    }
}