/****************************************************
  文件：YooResKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/5 11:07:16
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Zero.Utility
{
    public class YooResKit : SingletonMono<YooResKit>, IYooResKit
    {
        private TypeEventSystem _typeEventSystem = new TypeEventSystem();

        private bool _isInitialized = false;
        private string _defaultPackageName = null;

        private YooResKit()
        {
            //2024年2月14日14:42:18
            //需求：先加载完配置，再运行其他逻辑
            // PatchInfo info = new PatchInfo(EPlayMode.OfflinePlayMode, "ZeroRawPackage"); //加载本地配置
            // Init(info);
        }

        public IEnumerator Init(PatchInfo patchInfo)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                // 初始化事件系统
                UniEventKit.Instance.Initalize();
            
                //初始化资源系统
                YooAssets.Initialize();
            }

            //创建默认的资源包
            var package = YooAssets.CreatePackage(patchInfo.packageName);
            
            //设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容
            YooAssets.SetDefaultPackage(package);

            // 开始补丁更新流程
            if (patchInfo.buildPipeline == null) patchInfo.buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline.ToString();
            PatchOperation operation = new PatchOperation(patchInfo);
            YooAssets.StartOperation(operation);
            yield return operation;
            
            if (patchInfo.isSetDefault || _defaultPackageName == null)
            {
                _defaultPackageName = patchInfo.packageName;
            }
            else
            {
                if (_defaultPackageName != null)
                    YooAssets.SetDefaultPackage(YooAssets.GetPackage(_defaultPackageName));
            }
        }

        public ResourcePackage GetPackage(string packageName = null)
        {
            if (packageName == null)
                packageName = _defaultPackageName;
            if (YooAssets.ContainsPackage(packageName))
            {
                return YooAssets.GetPackage(packageName);
            }
            return null;
        }

        public void SetDefaultPackage(string packageName)
        {
            YooAssets.SetDefaultPackage(GetPackage(packageName));
        }
    }
}