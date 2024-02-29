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
    public interface IConfigKit : IUtility
    {
        /// <summary>
        /// 生成ConfigInfo配置，用于定义加载规则（非频繁操作，不使用对象池）
        /// </summary>
        /// <param name="location">配置文件路径</param>
        /// <param name="fileType">配置文件类型</param>
        /// <param name="loadType">加载配置文件的方式</param>
        /// <param name="orderType">加载次序（不建议改）</param>
        /// <returns></returns>
        ConfigInfo CreateConfigInfo(string location, ConfigInfo.FileType fileType,
            ConfigInfo.LoadType loadType, ConfigInfo.OrderType orderType = ConfigInfo.OrderType.BEFORE_INCLUDE);
        
        /// <summary>
        /// 初始化配置，最终会整合成一套配置
        /// </summary>
        /// <param name="configInfo"></param>
        IConfigKit Equip(ConfigInfo configInfo);
        
        /// <summary>
        /// 根据List顺序，初始化配置,，最终会整合成一套配置
        /// </summary>
        /// <param name="configInfoList"></param>
        IConfigKit Equip(List<ConfigInfo> configInfoList);
        
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