/****************************************************
  文件：FsmUpdatePackageManifest.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-11 20:30:41
  功能：Step3.更新资源清单！
*****************************************************/

using System.Collections;
using UnityEngine;
using YooAsset;

namespace Zero.Utility
{
    /// <summary>
    /// 3.更新资源清单
    /// </summary>
    public class FsmUpdatePackageManifest : FsmResStateNode<FsmUpdatePackageManifest>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            PatchEventDefine.PatchStatesChange.SendEventMessage("Step3.更新资源清单！");
            UniEventDriver.Instance.StartCoroutine(UpdateManifest());
        }
        
        private IEnumerator UpdateManifest()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            
            var packageName = (string)FSM.GetBlackboardValue("PackageName");
            var packageVersion = (string)FSM.GetBlackboardValue("PackageVersion");
            var package = YooAssets.GetPackage(packageName);
            bool savePackageVersion = true;
            var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                PatchEventDefine.PatchManifestUpdateFailed.SendEventMessage();
            }
            else
            {
                FSM.ChangeState<FsmCreatePackageDownloader>();
            }
        }
    }
}