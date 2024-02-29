/****************************************************
  文件：UnityScriptBuilder.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 23:30:51
  功能：
*****************************************************/

using System.IO;
using System.Text;
using Zero.Utility;

namespace Zero.Editor
{
    public class UnityScriptBuilder : ScriptBuilder
    {
        public string CreateScript(ScriptInfo scriptInfo)
        {
            if (scriptInfo == null) return "";
            string templateFilePath = null;
            if (scriptInfo.TemplatePath != null) //加载用户模板
            {
                templateFilePath = scriptInfo.TemplatePath;
            }
            else //加载内置模板
            {
                templateFilePath = Path.Combine(TemplateFolder, string.IsNullOrEmpty(scriptInfo.NamespaceName) ? "PlainScriptTemplate.txt" : "PlainScriptNameSpaceTemplate.txt");
            }
            fieldTabCount = string.IsNullOrEmpty(scriptInfo.NamespaceName) ? 1 : 2;
            stringBuilder.Clear();
            string content = ZeroToolKits.Instance.File.TextTool.ReadFromUri("file://" + templateFilePath);
            stringBuilder.Append(content);
            
            stringBuilder.Replace("$FILENAME$", scriptInfo.Filename);
            stringBuilder.Replace("$AUTHOR$", scriptInfo.Author);
            stringBuilder.Replace("$EMAIL$", scriptInfo.Email);
            stringBuilder.Replace("$DATETIME$", scriptInfo.Datetime);
            stringBuilder.Replace("$DESCRIPTION$", scriptInfo.Description); 

            stringBuilder.Replace("$NAME_SPACE$", scriptInfo.NamespaceName);
            stringBuilder.Replace("$CLASS$", scriptInfo.ClassName);
            
            return stringBuilder.ToString();
        }
    }
}