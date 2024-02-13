/****************************************************
  文件：BytesTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/27 19:51:22
  功能：
*****************************************************/
using System;
using System.Collections;
using System.IO;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Zero.Utility
{
    /// <summary>
    /// 文件模块：字节工具
    /// </summary>
    public class BytesTool
    {
        /// <summary>
        /// C#原生同步读字节
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] Read(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }
        
        /// <summary>
        /// C#原生同步写字节
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        public void Write(string path, byte[] bytes)
        {
            string dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllBytes(path, bytes);
        }
        
        /// <summary>
        /// C#原生异步读字节
        /// </summary>
        /// <param name="path"></param>
        /// <param name="readCallback"></param>
        public async void ReadAsync(string path, Action<byte[]> readCallback)
        {
            if (File.Exists(path))
            {
                byte[] content = await File.ReadAllBytesAsync(path);
                readCallback?.Invoke(content);
            }
        }
        
        /// <summary>
        /// C#原生异步写字节
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="writeCallback"></param>
        public async void WriteAsync(string path, byte[] content, Action writeCallback)
        {
            string dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            await File.WriteAllBytesAsync(path, content);
            writeCallback?.Invoke();
        }
    }
}