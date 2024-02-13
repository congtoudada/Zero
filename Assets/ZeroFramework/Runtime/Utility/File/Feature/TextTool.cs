/****************************************************
  文件：TextTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/27 19:51:22
  功能：
*****************************************************/
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Zero.Utility
{
    /// <summary>
    /// 文件模块：文本工具
    /// </summary>
    public class TextTool
    {
        /// <summary>
        /// C#原生同步读
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Read(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "";
        }

        /// <summary>
        /// UnityWebRequest同步读文本
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string ReadFromUri(string uri)
        {
            UnityWebRequest www = UnityWebRequest.Get(uri);
            www.SendWebRequest();
            while (!www.isDone) {}
            if (www.result == UnityWebRequest.Result.Success)
            {
                return www.downloadHandler.text;
            }
            return "";
        }
        
        /// <summary>
        /// C#原生同步写本地文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public void Write(string path, string content)
        {
            string dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(path, content);
        }
        
        /// <summary>
        /// C#原生异步读本地文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="readCallback"></param>
        public async void ReadAsync(string path, Action<string> readCallback)
        {
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                readCallback?.Invoke(content);
            }
        }
        
        /// <summary>
        /// C#原生异步写本地文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="writeCallback"></param>
        public async void WriteAsync(string path, string content, Action writeCallback)
        {
            string dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            await File.WriteAllTextAsync(path, content);
            writeCallback?.Invoke();
        }
        
        /// <summary>
        /// UnityWebRequest协程读文本
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator ReadFromUri(string uri, UnityAction<string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            yield return request.SendWebRequest();
            if (request.isDone)
            {
                callback?.Invoke(request.downloadHandler.text);
            }
            request.Dispose();
        }
    }
}