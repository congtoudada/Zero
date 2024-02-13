/****************************************************
  文件：UniTaskExtension.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/28 22:44:28
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Zero.Utility;

namespace Zero.Extension
{
    /// <summary>
    /// UniTask扩展
    /// </summary>
    public static class UniTaskExtension
    {
        #region Utility/ImageTool
        public static async UniTaskVoid ReadFromUriByUniTask<T>(this ImageTool self, string imageUrl, UnityAction<Texture2D> callback)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            await webRequest.SendWebRequest();
            if (webRequest.downloadHandler is DownloadHandlerTexture texRequest && texRequest.isDone)
            {
                callback?.Invoke(texRequest.texture);
            }
            else
            {
                callback?.Invoke(null);
            }
            webRequest.Dispose();
        }
        
        public static async UniTaskVoid ReadFromUriByUniTask<T>(this ImageTool self, string imageUrl, UnityAction<Sprite> callback)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            await webRequest.SendWebRequest();
            if (webRequest.downloadHandler is DownloadHandlerTexture texRequest && texRequest.isDone)
            {
                Texture2D texture2D = texRequest.texture;
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                callback?.Invoke(sprite);
            }
            else
            {
                callback?.Invoke(null);
            }
            webRequest.Dispose();
        }
        #endregion
        
        #region Utility/Text
        public static async UniTaskVoid ReadAsyncByUniTask(this TextTool self, string path, Action<string> readCallback)
        {
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                readCallback?.Invoke(content);
            }
        }
        
        public static async UniTaskVoid WriteAsyncByUniTask(this TextTool self, string path, string content, Action writeCallback)
        {
            string dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            await File.WriteAllTextAsync(path, content);
            writeCallback?.Invoke();
        }
        
        public static async UniTaskVoid ReadFromUriByUniTask(this TextTool self, string uri, UnityAction<string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            await request.SendWebRequest();
            if (request.isDone)
            {
                callback?.Invoke(request.downloadHandler.text);
            }
            request.Dispose();
        }
        #endregion
    }
}
