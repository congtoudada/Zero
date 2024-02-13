/****************************************************
  文件：ResExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/11 21:41:49
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Windows;
using YooAsset;
using Zero.Utility;
using ILogger = Zero.Utility.ILogger;

namespace Zero.Samples
{
    public class ResExample : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        private readonly IUniEventGroupKit _eventGroup = new UniEventGroupKit();
        public EPlayMode playMode;
        public DecryptionType decryptionType;
        private ILogger logger;
        public string remoteIP = "https://www.icongbao.com";
        public string appVersion = "v1.0";
        
        private IEnumerator Start()
        {
            logger = ZeroToolKits.Instance.InnerLog.AllocateLogger(typeof(ResExample));
            //注册监听事件 (From:Patch系统)
            _eventGroup.AddListener<PatchEventDefine.InitializeFailed>(OnHandleEventMessage);
            _eventGroup.AddListener<PatchEventDefine.PatchStatesChange>(OnHandleEventMessage);
            _eventGroup.AddListener<PatchEventDefine.FoundUpdateFiles>(OnHandleEventMessage);
            _eventGroup.AddListener<PatchEventDefine.DownloadProgressUpdate>(OnHandleEventMessage);
            _eventGroup.AddListener<PatchEventDefine.PackageVersionUpdateFailed>(OnHandleEventMessage);
            _eventGroup.AddListener<PatchEventDefine.PatchManifestUpdateFailed>(OnHandleEventMessage);
            _eventGroup.AddListener<PatchEventDefine.WebFileDownloadFailed>(OnHandleEventMessage);
            
            IYooResKit yooResKit = ZeroToolKits.Instance.YooRes;
            //------------------- OfflinePlayMode -------------------
            //加载Unity资产
            PatchInfo info = new PatchInfo(playMode, "TestPackage", 
                decryptionType: decryptionType, 
                remoteIP: remoteIP, appVersion: appVersion);
            yield return yooResKit.Init(info);
            var package = yooResKit.GetPackage();
            package.LoadAssetAsync<GameObject>("Sphere").Completed += handle => handle.InstantiateAsync();
            
            //------------------- HostPlayMode -------------------
            //加载Raw资产
            PatchInfo rawInfo = new PatchInfo(playMode, "TestRawPackage", 
                decryptionType: decryptionType, buildPipeline:
                EDefaultBuildPipeline.RawFileBuildPipeline.ToString(), 
                isSetDefault: false, 
                remoteIP: remoteIP, appVersion: appVersion);
            yield return yooResKit.Init(rawInfo);
            
            var rawPackage = yooResKit.GetPackage("TestRawPackage");
            if (videoPlayer != null)
            {
                rawPackage.LoadRawFileAsync("ResVideo").Completed += handle =>
                {
                    videoPlayer.source = VideoSource.Url;
                    string url = handle.GetRawFilePath();
                    if (!url.EndsWith(".mp4"))
                    {
                        System.IO.File.Move(url, url + ".mp4");
                    }
                    videoPlayer.url = url + ".mp4";
                    videoPlayer.Play();
                };
            }
            
            //加载Unity资产（验证默认包可用）
            package.LoadAssetAsync<GameObject>("Cube").Completed += handle => handle.InstantiateAsync();
        }
        
    private void OnHandleEventMessage(IEventMessage message)
    {
        if (message is PatchEventDefine.InitializeFailed)
        {
            System.Action callback = () =>
            {
                UserEventDefine.UserTryInitialize.SendEventMessage();
            };
            logger.Info($"Failed to initialize package !");
            callback(); //自动触发回调
        }
        else if (message is PatchEventDefine.PatchStatesChange)
        {
            var msg = message as PatchEventDefine.PatchStatesChange;
            logger.Info("PatchStatesChange: " + msg.Tips); //自动触发回调
        }
        else if (message is PatchEventDefine.FoundUpdateFiles)
        {
            var msg = message as PatchEventDefine.FoundUpdateFiles;
            System.Action callback = () =>
            {
                UserEventDefine.UserBeginDownloadWebFiles.SendEventMessage();
            };
            float sizeMB = msg.TotalSizeBytes / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            logger.Info($"Found update patch files, Total count {msg.TotalCount} Total szie {totalSizeMB}MB");
            callback(); //自动触发回调
        }
        else if (message is PatchEventDefine.DownloadProgressUpdate)
        {
            var msg = message as PatchEventDefine.DownloadProgressUpdate;
            string currentSizeMB = (msg.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
            string totalSizeMB = (msg.TotalDownloadSizeBytes / 1048576f).ToString("f1");
            logger.Info($"{msg.CurrentDownloadCount}/{msg.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB");
        }
        else if (message is PatchEventDefine.PackageVersionUpdateFailed)
        {
            System.Action callback = () =>
            {
                UserEventDefine.UserTryUpdatePackageVersion.SendEventMessage();
            };
            logger.Info($"Failed to update static version, please check the network status.");
            callback(); //自动触发回调
        }
        else if (message is PatchEventDefine.PatchManifestUpdateFailed)
        {
            System.Action callback = () =>
            {
                UserEventDefine.UserTryUpdatePatchManifest.SendEventMessage();
            };
            logger.Info($"Failed to update patch manifest, please check the network status.");
            callback(); //自动触发回调
        }
        else if (message is PatchEventDefine.WebFileDownloadFailed)
        {
            var msg = message as PatchEventDefine.WebFileDownloadFailed;
            System.Action callback = () =>
            {
                UserEventDefine.UserTryDownloadWebFiles.SendEventMessage();
            };
            logger.Info($"Failed to download file : {msg.FileName}");
            callback(); //自动触发回调
        }
        else
        {
            throw new System.NotImplementedException($"{message.GetType()}");
        }
    }
    }
}