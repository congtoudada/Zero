/****************************************************
  文件：IYooResKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/5 11:07:05
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace Zero.Utility
{
    public enum DecryptionType
    {
        None,
        FileStream,
        FileOffset
    }
    
    public interface IYooResKit
    {
        /// <summary>
        /// 资源系统初始化
        /// </summary>
        /// <param name="playMode"></param>
        /// <param name="packageName"></param>
        /// <param name="buildPipeline"></param>
        /// <param name="decryptionType"></param>
        /// <param name="isSetDefault"></param>
        /// <returns></returns>
        IEnumerator Init(PatchInfo patchInfo);

        /// <summary>
        /// 获取Package（不传参表示获取默认Package）
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        ResourcePackage GetPackage(string packageName = null);

        /// <summary>
        /// 设置默认Package
        /// </summary>
        /// <param name="packageName"></param>
        void SetDefaultPackage(string packageName);
    }
}