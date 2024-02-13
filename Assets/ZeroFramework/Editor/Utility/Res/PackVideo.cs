/****************************************************
  文件：PackVideo.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/12 16:12:00
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset.Editor;

namespace Zero.Editor
{
    public class PackVideo : IPackRule
    {
        PackRuleResult IPackRule.GetPackRuleResult(PackRuleData data)
        {
            string bundleName = data.AssetPath;
            string fileExtension = Path.GetExtension(data.AssetPath);
            fileExtension = fileExtension.Remove(0, 1);
            PackRuleResult result = new PackRuleResult(bundleName, fileExtension);
            return result;
        }
    }
}
