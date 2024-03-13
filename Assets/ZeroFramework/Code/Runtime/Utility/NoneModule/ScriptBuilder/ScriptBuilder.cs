/****************************************************
  文件：ScriptBuilder.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 23:27:40
  功能：脚本构造者
*****************************************************/

using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.Utilities.Editor;
using Zero.Utility;

namespace Zero.Utility
{
    public abstract class ScriptBuilder
    {
        public enum AccessEnum
        {
            PUBLIC,
            PROTECTED,
            PRIVATE
        }

        protected int fieldTabCount = 0; //字段缩进
        protected static readonly string TemplateFolder;
        protected List<string> fieldLines = new List<string>();
        protected StringBuilder stringBuilder = new StringBuilder();

        static ScriptBuilder()
        {
            //TemplateFolder = Path.Combine(ZeroToolKits.Instance.PathHelper.GetZeroFolderAbsolute(), "Code/Editor/Utility/NoneModule/ScriptBuilder/Template");
            TemplateFolder =
                ZeroToolKits.Instance._EG.Get<string>(ZeroEditorConfigKey.UTILITY__SCRIPT_BUILDER__TEMPLATE);
        }

        public ScriptInfo CreateScriptInfo()
        {
            return new ScriptInfo();
        }

        public abstract string CreateScript(ScriptInfo scriptInfo);

        public void AddField(AccessEnum access, string typeName, string fieldName, string defaultValue = null, string annotation = null, List<string> attributes = null, int fieldTabCount = -1)
        {
            stringBuilder.Clear();

            //添加缩进（第一个元素不打）
            if (fieldLines.Count > 0)
            {
                if (fieldTabCount == -1) fieldTabCount = this.fieldTabCount; //使用默认缩进
                for (int i = 0; i < fieldTabCount; i++)
                {
                    stringBuilder.Append("\t");
                }
            }
            
            //添加特性（如果有）
            TryAddAttributes(stringBuilder, attributes);
            
            //添加访问修饰
            switch (access)
            {
                case AccessEnum.PUBLIC:
                    stringBuilder.Append("public ");
                    break;
                case AccessEnum.PROTECTED:
                    stringBuilder.Append("protected ");
                    break;
                case AccessEnum.PRIVATE:
                    stringBuilder.Append("private ");
                    break;
            }
            
            //添加类型和字段名（如果有默认值则赋值默认值）
            stringBuilder.Append(defaultValue == null
                ? $"{typeName} {fieldName};"
                : $"{typeName} {fieldName} = {defaultValue};");
            
            //添加注释（如果有）
            if (!string.IsNullOrEmpty(annotation))
                stringBuilder.Append($" //{annotation}");
            
            //加入结果集
            fieldLines.Add(stringBuilder.ToString());
        }

        public void ClearFieldLines()
        {
            fieldLines.Clear();
        }

        public string FillField(string content)
        {
            stringBuilder.Clear();
            foreach (var line in fieldLines)
            {
                stringBuilder.Append(line).AppendLine();
            }
            
            //添加缩进
            for (int i = 0; i < fieldTabCount; i++)
            {
                stringBuilder.Append("\t");
            }
            stringBuilder.Append("//Field End");
            return content.Replace("//Field End", stringBuilder.ToString());
        }
        

        private void TryAddAttributes(StringBuilder stringBuilder, List<string> attrs)
        {
            if (attrs == null) return;
            this.stringBuilder.Append($"[");
            for (int i = 0; i < attrs.Count; i++)
            {
                this.stringBuilder.Append(attrs[i]);
                if (i != attrs.Count - 1)
                    this.stringBuilder.Append(",");
            }

            this.stringBuilder.Append("] ");
        }
        
        // public void AddMethod(AccessEnum access, string returnType, string methodName, List<string> paramList,
        //     string methodBody, string annotation = null, List<string> attributes = null)
        // {
        //     _stringBuilder.Clear();
        //
        //     //添加注释（如果有）
        //     if (annotation != null)
        //         _stringBuilder.Append($"//{annotation}").AppendLine();
        //     
        //     //添加特性（如果有）
        //     TryAddAttributes(_stringBuilder, attributes);
        // }
        
    }
}