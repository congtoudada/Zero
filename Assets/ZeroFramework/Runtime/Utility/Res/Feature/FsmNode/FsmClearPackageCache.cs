/****************************************************
  文件：FsmClearPackageCache.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-11 21:21:20
  功能：
*****************************************************/

using YooAsset;

namespace Zero.Utility
{
    /// <summary>
    /// 7.清理未使用的缓存文件
    /// </summary>
    public class FsmClearPackageCache : FsmResStateNode<FsmClearPackageCache>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            PatchEventDefine.PatchStatesChange.SendEventMessage("清理未使用的缓存文件！");
            var packageName = (string)FSM.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearUnusedCacheFilesAsync();
            operation.Completed += Operation_Completed;
        }
        
        private void Operation_Completed(YooAsset.AsyncOperationBase obj)
        {
            FSM.ChangeState<FsmUpdaterDone>();
        }
    }
}