/****************************************************
  文件：FsmDownloadPackageOver.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-11 21:19:34
  功能：
*****************************************************/

namespace Zero.Utility
{
    /// <summary>
    /// 6.下载完毕
    /// </summary>
    public class FsmDownloadPackageOver : FsmResStateNode<FsmDownloadPackageOver>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            PatchEventDefine.PatchStatesChange.SendEventMessage("Step6.补丁文件下载完毕！");
            FSM.ChangeState<FsmClearPackageCache>();
        }
    }
}