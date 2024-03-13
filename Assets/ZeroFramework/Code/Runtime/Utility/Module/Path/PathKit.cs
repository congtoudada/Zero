/****************************************************
  文件：PathKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/27 16:50:27
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zero.Utility
{
    public class PathKit : Singleton<PathKit>, IPathKit
    {
        private bool _initialized = false;
        private string ZERO_FOLDER_ABSOLUTE;
        private string ZERO_FOLDER_RELATIVE;

        private PathKit()
        {
        }

        private void UpdateZeroFolder()
        {
            string zeroFolder = Directory.GetDirectories("Assets", "ZeroFramework", SearchOption.AllDirectories)[0];
            ZERO_FOLDER_ABSOLUTE = AssetsRelativeToAbsolute(zeroFolder);
            ZERO_FOLDER_RELATIVE = zeroFolder.Replace("\\", "/");
        }

        private void CheckAndInitZeroFolder()
        {
            if (!_initialized)
            {
                _initialized = true;
                UpdateZeroFolder();
            }
        }
        
#if UNITY_EDITOR
        public class AssetMoveListener : UnityEditor.AssetModificationProcessor //必须继承这个
        {
            /// <summary>
            /// 监听资源移动事件
            /// </summary>
            /// <param name="oldPath">旧路径</param>
            /// <param name="newPath">新路径</param>
            /// <returns></returns>
            public static AssetMoveResult OnWillMoveAsset(string oldPath,string newPath)
            {
                if (oldPath.EndsWith("ZeroFramework"))
                {
                    PathKit.Instance.UpdateZeroFolder();
                    Debug.Log("Detect ZeroFramework Move! Location: " + newPath);
                }
                return AssetMoveResult.DidNotMove;
            }
        }
#endif
        
        public string GetZeroFolderAbsolute()
        {
            CheckAndInitZeroFolder();
            return ZERO_FOLDER_ABSOLUTE;
        }

        public string GetZeroFolderRelative()
        {
            CheckAndInitZeroFolder();
            return ZERO_FOLDER_RELATIVE;
        }

        public string AssetsRelativeToAbsolute(string assetRelativePath)
        {
            if (assetRelativePath.StartsWith("Assets"))
            {
                return Path.Combine(Path.GetDirectoryName(Application.dataPath)!, assetRelativePath)
                    .Replace("\\", "/");
            }
            
            return Path.Combine(Application.dataPath, assetRelativePath)
                    .Replace("\\", "/");
        }


        public string GetStreamingAssetsPath()
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath + "/Win/";
#elif UNITY_STANDALONE_WIN
            return Application.streamingAssetsPath + "/Win/";
#elif UNITY_ANDROID
            return Application.streamingAssetsPath + "/Android/";
#elif UNITY_IOS
            return Application.streamingAssetsPath + "/IOS/";
#else
            return Application.streamingAssetsPath + "/Win/";
#endif
            }
    }
}
