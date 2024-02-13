/****************************************************
  文件：FsmUpdatePackageVersion.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-05 20:08:10
  功能：Step2.获取最新的资源版本！
*****************************************************/

using System.Collections;
using UnityEngine;
using YooAsset;

namespace Zero.Utility
{
    /// <summary>
    /// 2.更新资源版本号
    /// </summary>
    public class FsmUpdatePackageVersion : FsmResStateNode<FsmUpdatePackageVersion>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            PatchEventDefine.PatchStatesChange.SendEventMessage("Step2.获取最新的资源版本！");
            UniEventDriver.Instance.StartCoroutine(UpdatePackageVersion());
        }

        private IEnumerator UpdatePackageVersion()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            
            var packageName = (string) FSM.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageVersionAsync();
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                logger.Warn(operation.Error);
                PatchEventDefine.PackageVersionUpdateFailed.SendEventMessage();
            }
            else
            {
                FSM.SetBlackboardValue("PackageVersion", operation.PackageVersion);
                FSM.ChangeState<FsmUpdatePackageManifest>();
            }
            
        }
    }
}