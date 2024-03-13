/****************************************************
  文件：ConfigInfo.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-13 00:09:28
  功能：
*****************************************************/

using System.Collections.Generic;

namespace Zero.Utility
{
    public class ConfigInfo
    {
        public enum FileType
        {
            SCRIPTABLE, //Scriptable配置（不支持UnityWebRequest加载）
            YAML, //YAML配置
            JSON, //JSON配置
        }

        public enum LoadType
        {
            ZERO_RES, //资源系统加载配置（必须先初始化）
            UNITY_WEB_REQUEST, //UnityWebRequest加载配置
            RESOURCES //Resources加载配置
        }
        
        //加载次序：建议保持默认BEFORE_INCLUDE，如果修改最好整个项目加载次序一致，避免混乱
        public enum OrderType
        {
            BEFORE_INCLUDE, //在依赖前加载自身，自身的属性会被依赖项覆盖（依赖拥有绝对优先级）
            AFTER_INCLUDE, //在依赖后加载自身，依赖项会被自身属性覆盖（自身拥有绝对优先级）
        }
        public string location; //配置文件位置
        public FileType fileType; //加载文件类型
        public LoadType loadType; //加载方式
        public OrderType orderType; //加载次序

        public ConfigInfo(string location, FileType fileType, LoadType loadType, OrderType orderType = OrderType.BEFORE_INCLUDE)
        {
            this.location = location;
            this.fileType = fileType;
            this.loadType = loadType;
            this.orderType = orderType;
        }

        public override string ToString()
        {
            return $"{nameof(location)}: {location}, {nameof(fileType)}: {fileType}, {nameof(loadType)}: {loadType}, {nameof(orderType)}: {orderType}";
        }
    }
}