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
        public enum LoadEnum
        {
            SCRIPTABLE,
            YAML,
            JSON
        }
        public string location;
        public LoadEnum loadMethod;

        public ConfigInfo(string location, LoadEnum loadMethod)
        {
            this.location = location;
            this.loadMethod = loadMethod;
        }

        public override string ToString()
        {
            return $"{nameof(location)}: {location}, {nameof(loadMethod)}: {loadMethod}";
        }
    }
}