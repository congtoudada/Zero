/****************************************************
  文件：FileKit.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：2021/11/14 16:29:07
  功能：文件操作工具类
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


namespace Zero.Utility
{
    /// <summary>
    /// 文件工具
    /// </summary>
    public class FileKit : IFileKit
    {
        #region 实现接口
        BytesTool IFileKit.BytesTool => Bytes;

        ImageTool IFileKit.ImageTool => Image;

        JsonTool IFileKit.JsonTool => Json;

        TextTool IFileKit.TextTool => Text;

        YamlTool IFileKit.YamlTool => Yaml;
        
        ExcelTool IFileKit.ExcelTool => Excel;

        public T Yaml2Json2Obj<T>(string yamlStr)
        {
            return Json.DeserializeObject<T>(Yaml.YamlToJson(yamlStr));
        }
        
        public T Path2Yaml2Json2Obj<T>(string path)
        {
            if (!File.Exists(path)) return default(T);
            return Yaml2Json2Obj<T>(Text.ReadFromUri("file://" + path));
        }
        #endregion
        
        #region 字节数组操作
        private BytesTool Bytes
        {
            get
            {
                if (_bytesTool == null)
                {
                    _bytesTool = new BytesTool();
                }
                return _bytesTool;
            }
        }
        private BytesTool _bytesTool;
        #endregion
        
        #region Image操作
        private ImageTool Image
        {
            get
            {
                if (_imageTool == null)
                {
                    _imageTool = new ImageTool();
                }
                return _imageTool;
            }
        }
        private ImageTool _imageTool;
        #endregion
        
        #region Json操作
        private JsonTool Json
        {
            get
            {
                if (_jsonTool == null)
                {
                    _jsonTool = new JsonTool();
                }
                return _jsonTool;
            }
        }
        private JsonTool _jsonTool;
        #endregion
        
        #region 文本文件操作
        private TextTool Text
        {
            get
            {
                if (_textTool == null)
                {
                    _textTool = new TextTool();
                }
                return _textTool;
            }
        }
        private TextTool _textTool;
        #endregion
        
        #region Yaml操作
        private YamlTool Yaml
        {
            get
            {
                if (_yamlTool == null)
                {
                    _yamlTool = new YamlTool();
                }
                return _yamlTool;
            }
        }
        private YamlTool _yamlTool;
        #endregion
        
        #region Excel操作
        private ExcelTool Excel
        {
            get
            {
                if (_excelTool == null)
                {
                    _excelTool = new ExcelTool();
                }
                return _excelTool;
            }
        }
        private ExcelTool _excelTool;
        #endregion
    }
}