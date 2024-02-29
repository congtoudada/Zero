/****************************************************
  文件：ZeroEditorArchitecture.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/25 15:14:13
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Editor
{
    /// <summary>
    /// 提供自定义编辑器Architecture实现类范例
    /// </summary>
    public class ZeroEditorArchitecture : Architecture<ZeroEditorArchitecture>
    {
        protected override void Init()
        {
            this.RegisterUtility<ZeroToolKits>(ZeroToolKits.Instance);
        }
    }
}
