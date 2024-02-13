/****************************************************
  文件：FsmDownloadPackageFiles.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-11 21:17:49
  功能：Step5.开始下载补丁文件！
*****************************************************/

using System.Collections;
using YooAsset;

namespace Zero.Utility
{
    /// <summary>
    /// 5.下载更新文件
    /// </summary>
    public class FsmDownloadPackageFiles : FsmResStateNode<FsmDownloadPackageFiles>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            PatchEventDefine.PatchStatesChange.SendEventMessage("Step5.开始下载补丁文件！");
            UniEventDriver.Instance.StartCoroutine(BeginDownload());
        }
        
        private IEnumerator BeginDownload()
        {
            var downloader = (ResourceDownloaderOperation)FSM.GetBlackboardValue("Downloader");
            downloader.OnDownloadErrorCallback = PatchEventDefine.WebFileDownloadFailed.SendEventMessage;
            downloader.OnDownloadProgressCallback = PatchEventDefine.DownloadProgressUpdate.SendEventMessage;
            downloader.BeginDownload();
            yield return downloader;

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
                yield break;

            FSM.ChangeState<FsmDownloadPackageOver>();
        }
    }
}