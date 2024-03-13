/****************************************************
  文件：ScriptInfo.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-29 09:48:45
  功能：
*****************************************************/

using System;
using System.Globalization;

namespace Zero.Utility
{
    /// <summary>
    /// 脚本基础信息
    /// </summary>
    public class ScriptInfo
    {
        private string filename;
        private string author = "聪头";
        private string email = "1322080797@qq.com";
        private string datetime = DateTime.UtcNow.AddHours(8).ToString(CultureInfo.InvariantCulture);
        private string description;

        private string namespaceName;
        private string className; //类名（包括继承信息）

        private string templatePath; //模板路径

        public string TemplatePath
        {
            get => templatePath;
            set => templatePath = value;
        }

        public string Filename
        {
            get => filename;
            set => filename = value;
        }

        public string Author
        {
            get => author;
            set => author = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }

        public string Datetime
        {
            get => datetime;
            set => datetime = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public string NamespaceName
        {
            get => namespaceName;
            set => namespaceName = value;
        }

        public string ClassName
        {
            get => className;
            set => className = value;
        }
        
    }
}