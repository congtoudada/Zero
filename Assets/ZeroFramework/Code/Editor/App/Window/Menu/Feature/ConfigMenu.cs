/****************************************************
  文件：ConfigMenu.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/28 10:24:04
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools.Constraints;
using Zero.Utility;

namespace Zero.Editor
{
    public class ConfigMenu : ZeroSubMenu
    {
        [TitleGroup("配置文件路径")]
        [TabGroup("配置文件路径/ConfigPathGroup", "Yaml")]
        [Sirenix.OdinInspector.FilePath, OnValueChanged("UndoRecord")]
        public string YamlPath = "";
        

        [TabGroup("配置文件路径/ConfigPathGroup", "Json")]
        [Sirenix.OdinInspector.FilePath, OnValueChanged("UndoRecord")]
        public string JsonPath = "";
        
        [TabGroup("配置文件路径/ConfigPathGroup","ZeroConfig")]
        [AssetsOnly, OnValueChanged("UndoRecord")]
        public ZeroConfig ZeroConfigAsset = null;
        
        [Title("是否追加写入")]
        [EditorCache, OnValueChanged("UndoRecord")]
        public bool IsAppend = true;

        [Title("输出文件名 (自带ConfigKey后缀)")]
        [Required, EditorCache, OnValueChanged("UndoRecord")]
        public string OutputName = "";
            
        [Title("输出文件夹")]
        [FolderPath, Required, EditorCache, OnValueChanged("UndoRecord")]
        public string OutputFolder = "${Zero}/Output/Utility/ConfigKey";

        [Button("解析测试", ButtonSizes.Large)]
        public void DebugButton()
        {
            // string outputPath = Path.Combine("file://" + Relative2Absolute(OutputFolder), OutputName + ".cs");
            // Debug.Log(outputPath);
            var config = ResolveConfig();
            config.Keys.ForEach(x=>Debug.Log(x));
        }
        
        //一键生成所有Key
        // [PropertySpace(SpaceBefore = 50)]
        [Button("生成Config Key", ButtonSizes.Large)]
        public void CreateConfigKey()
        {
            if (OutputName == "") return;
            try
            {
                var config = ResolveConfig();
                if (config == null) return;
                //写入脚本
                //文件名预处理 (首字母大写，强制以ConfigKey结尾）
                var charArray = OutputName.ToCharArray();
                if (!char.IsLetter(charArray[0]))
                {
                    Debug.LogError("非法命名，请检查: " + OutputName);
                    return;
                }
                charArray[0] = char.ToUpper(charArray[0]);
                string outputFileName = new string(charArray);
                if (!outputFileName.EndsWith("ConfigKey")) //文件名强制后缀"ConfigKey"
                {
                    outputFileName += "ConfigKey";
                }
                string outputPath = Path.Combine(Relative2Absolute(OutputFolder), outputFileName + ".cs");
                // configKit.Keys.ForEach(s => Debug.Log(s)); //测试
                if (config.Keys == null)
                {
                    Debug.LogError("ConfigKey is Null!");
                    return;
                }
                
                WriteScript(outputPath, config.Keys);
                SaveCache();
                AssetDatabase.Refresh();
                EditorUtility.RevealInFinder(Relative2Absolute(outputPath));
                Debug.Log("导出成功！路径: " + outputPath);
            }
            catch (Exception e)
            {
                Debug.LogError("导出失败: " + e.StackTrace);
            }
        }

        private IConfigKit ResolveConfig()
        {
            IConfigKit configKit = new ConfigKit();
            string absolutePath = null;
            if (YamlPath.EndsWith(".yaml") || YamlPath.EndsWith(".yml"))
            {
                Debug.Log("解析Yaml配置");
                absolutePath = Relative2Absolute(YamlPath);
                if (!CheckFileExist(absolutePath)) return null;
                configKit.Equip(configKit.CreateConfigInfo(absolutePath, ConfigInfo.FileType.YAML, ConfigInfo.LoadType.UNITY_WEB_REQUEST));
            }
            else if (JsonPath.EndsWith(".json"))
            {
                Debug.Log("解析Json配置");
                absolutePath = Relative2Absolute(JsonPath);
                if (!CheckFileExist(absolutePath)) return null;
                configKit.Equip(configKit.CreateConfigInfo(absolutePath, ConfigInfo.FileType.JSON, ConfigInfo.LoadType.UNITY_WEB_REQUEST));
                
            }
            else if (ZeroConfigAsset != null)
            {
                Debug.Log("解析ZeroConfig配置");
                absolutePath = Relative2Absolute(AssetDatabase.GetAssetPath(ZeroConfigAsset));
                if (!CheckFileExist(absolutePath)) return null;
                configKit.Equip(configKit.CreateConfigInfo(Path.GetFileNameWithoutExtension(absolutePath), ConfigInfo.FileType.SCRIPTABLE, ConfigInfo.LoadType.RESOURCES));
            }
            else
            {
                Debug.LogWarning("非法配置文件，解析失败!");
                return null;
            }
            return configKit;
        }

        private bool CheckFileExist(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                Debug.LogError("请检查路径合法性: " + path);
                return false;
            }

            return true;
        }

        private void WriteScript(string path, List<string> keys)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            
            if (!IsAppend) //不是追加赋值就删除原文件
            {
                if (File.Exists(path))
                    File.Delete(path);
            }

            string outputFilename = Path.GetFileNameWithoutExtension(path);
            //编写脚本生成器
            UnityScriptBuilder scriptBuilder = new UnityScriptBuilder();
            ScriptInfo scriptInfo = new ScriptInfo()
            {
                Filename = outputFilename,
                // Author = "聪头",
                // Email = "1322080797@qq.com",
                // Datetime = DateTime.UtcNow.AddHours(8).ToString(CultureInfo.InvariantCulture),
                Description = "Config Key",
                NamespaceName = "Zero.Utility",
                ClassName = "public class " + outputFilename,
                TemplatePath = IsAppend && File.Exists(path)? path : null
            };
            string scriptContent = scriptBuilder.CreateScript(scriptInfo);
            //填充脚本字段
            int succCount = 0; //成功更新字段数
            int passCount = 0; //跳过字段数
            foreach (var key in keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                //eg. UTILITY__NODE_EDITOR__UXML
                string fieldName = key.Replace(".", "__").ToUpper();
                if (scriptContent.Contains(fieldName))
                {
                    ++passCount;
                    continue; //已经存在Key则跳过
                }
                ++succCount;
                scriptBuilder.AddField(ScriptBuilder.AccessEnum.PUBLIC, "const string", fieldName, $"\"{key}\"");
            }
            scriptContent = scriptBuilder.FillField(scriptContent);
            Debug.Log($"成功更新字段数: {succCount} 跳过重复字段数: {passCount}");
            this.GetUtility<ZeroToolKits>().File.TextTool.Write(path, scriptContent);
        }
        
    }
}
