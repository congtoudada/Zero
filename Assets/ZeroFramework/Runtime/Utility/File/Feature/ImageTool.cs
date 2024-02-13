/****************************************************
  文件：ImageTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/27 19:51:22
  功能：
*****************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Zero.Utility
{
    /// <summary>
    /// 文件模块：图片工具
    /// </summary>
    public class ImageTool
    {
        /// <summary>
        /// UnityWebRequest协程读Texture2D
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator ReadFromUri(string imageUrl, UnityAction<Texture2D> callback)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return webRequest.SendWebRequest();
            DownloadHandlerTexture texRequest = webRequest.downloadHandler as DownloadHandlerTexture;
            if (texRequest != null && texRequest.isDone)
            {
                callback?.Invoke(texRequest.texture);
            }
            else
            {
                callback?.Invoke(null);
            }
            webRequest.Dispose();
        }
        
        /// <summary>
        /// UnityWebRequest协程读Sprite
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator ReadFromUri(string imageUrl, UnityAction<Sprite> callback)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return webRequest.SendWebRequest();
            DownloadHandlerTexture texRequest = webRequest.downloadHandler as DownloadHandlerTexture;
            if (texRequest != null && texRequest.isDone)
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
    }
}