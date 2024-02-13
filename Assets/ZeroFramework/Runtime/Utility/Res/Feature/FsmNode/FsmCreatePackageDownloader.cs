/****************************************************
  文件：FsmCreatePackageDownloader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-11 20:49:54
  功能：Step4.创建补丁下载器！
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Zero.Utility
{
    /// <summary>
    /// 4.创建文件下载器
    /// </summary>
    public class FsmCreatePackageDownloader : FsmResStateNode<FsmCreatePackageDownloader>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            PatchEventDefine.PatchStatesChange.SendEventMessage("Step4.创建补丁下载器！");
            UniEventDriver.Instance.StartCoroutine(CreateDownloader());
        }

        private IEnumerator CreateDownloader()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            
            var packageName = (string)FSM.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            FSM.SetBlackboardValue("Downloader", downloader);

            if (downloader.TotalDownloadCount == 0)
            {
                logger.Info("Not found any download files !");
                FSM.ChangeState<FsmUpdaterDone>();
            }
            else
            {
                // 发现新更新文件后，挂起流程系统（由事件系统决定是否进一步下载）
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                PatchEventDefine.FoundUpdateFiles.SendEventMessage(totalDownloadCount, totalDownloadBytes);
            }
        }
    }
}