/****************************************************
  文件：PatchOperation.cs
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
    /// <summary>
    /// 补丁更新操作
    /// </summary>
    public class PatchOperation : GameAsyncOperation
    {
        private enum ESteps
        {
            None,
            Update,
            Done,
        }
    
        private readonly IUniEventGroupKit _eventGroup = new UniEventGroupKit();
        private readonly StateMachine _machine;
        private ESteps _steps = ESteps.None;
    
        public PatchOperation(PatchInfo patchInfo)
        {
            // 注册监听事件 (From:用户)
            _eventGroup.AddListener<UserEventDefine.UserTryInitialize>(OnHandleEventMessage);
            _eventGroup.AddListener<UserEventDefine.UserBeginDownloadWebFiles>(OnHandleEventMessage);
            _eventGroup.AddListener<UserEventDefine.UserTryUpdatePackageVersion>(OnHandleEventMessage);
            _eventGroup.AddListener<UserEventDefine.UserTryUpdatePatchManifest>(OnHandleEventMessage);
            _eventGroup.AddListener<UserEventDefine.UserTryDownloadWebFiles>(OnHandleEventMessage);
    
            // 创建状态机
            _machine = new StateMachine(this);
            _machine.AddState<FsmInitializePackage>();
            _machine.AddState<FsmUpdatePackageVersion>();
            _machine.AddState<FsmUpdatePackageManifest>();
            _machine.AddState<FsmCreatePackageDownloader>();
            _machine.AddState<FsmDownloadPackageFiles>();
            _machine.AddState<FsmDownloadPackageOver>();
            _machine.AddState<FsmClearPackageCache>();
            _machine.AddState<FsmUpdaterDone>();
    
            _machine.SetBlackboardValue("PackageName", patchInfo.packageName);
            _machine.SetBlackboardValue("PlayMode", patchInfo.playMode);
            _machine.SetBlackboardValue("BuildPipeline", patchInfo.buildPipeline);
            _machine.SetBlackboardValue("DecryptionType", patchInfo.decryptionType);
            _machine.SetBlackboardValue("RemoteIP", patchInfo.remoteIP);
            _machine.SetBlackboardValue("AppVersion", patchInfo.appVersion);
        }
        protected override void OnStart()
        {
            _steps = ESteps.Update;
            _machine.Run<FsmInitializePackage>();
        }
        protected override void OnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;
                
            if(_steps == ESteps.Update)
            {
                _machine.Update();
                if(_machine.CurrentState.GetType().FullName == typeof(FsmUpdaterDone).FullName)
                {
                    _eventGroup.RemoveAllListener();
                    Status = EOperationStatus.Succeed;
                    _steps = ESteps.Done;
                }
            }
        }
        protected override void OnAbort()
        {
        }
    
        /// <summary>
        /// 接收事件
        /// </summary>
        private void OnHandleEventMessage(IEventMessage message)
        {
            if (message is UserEventDefine.UserTryInitialize)
            {
                _machine.ChangeState<FsmInitializePackage>();
            }
            else if (message is UserEventDefine.UserBeginDownloadWebFiles)
            {
                _machine.ChangeState<FsmDownloadPackageFiles>();
            }
            else if (message is UserEventDefine.UserTryUpdatePackageVersion)
            {
                _machine.ChangeState<FsmUpdatePackageVersion>();
            }
            else if (message is UserEventDefine.UserTryUpdatePatchManifest)
            {
                _machine.ChangeState<FsmUpdatePackageManifest>();
            }
            else if (message is UserEventDefine.UserTryDownloadWebFiles)
            {
                _machine.ChangeState<FsmCreatePackageDownloader>();
            }
            else
            {
                throw new System.NotImplementedException($"{message.GetType()}");
            }
        }
    }
}