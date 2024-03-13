/****************************************************
  文件：ZeroArchitecture.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/30 16:49:20
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero
{
    /// <summary>
    /// 提供自定义运行时Architecture实现类范例
    /// </summary>
    public class ZeroArchitecture : Architecture<ZeroArchitecture>
    {
        /// <summary>
        /// 初始化架构，用于注册各个功能模块
        /// </summary>
        protected override void Init()
        {
            // RegisterUtility(new ZeroToolKits());
            RegisterUtility(ZeroToolKits.Instance);
        }
    }
}
