/****************************************************
  文件：FsmResStateNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-06 17:30:20
  功能：
*****************************************************/

namespace Zero.Utility
{
    public class FsmResStateNode<T> : AbstractStateNode where T : FsmResStateNode<T>
    {
        //1.FsmInitializePackage
        //2.FsmUpdatePackageVersion
        //3.FsmUpdatePackageManifest
        //4.FsmCreatePackageDownloader
        //5.FsmDownloadPackageFiles
        //6.FsmDownloadPackageOver
        //7.FsmClearPackageCache
        //8.FsmUpdaterDone
        
        protected ILogger logger;
        public FsmResStateNode()
        {
            logger = ZeroToolKits.Instance.InnerLog.AllocateLogger(typeof(T), "[ Res ]");
        }
    }
}