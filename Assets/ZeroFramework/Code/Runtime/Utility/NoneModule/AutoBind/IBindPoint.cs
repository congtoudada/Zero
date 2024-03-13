/****************************************************
  文件：IBindPoint.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-03-01 13:40:35
  功能：
*****************************************************/

using System;
using UnityEngine;

namespace Zero.Utility
{
    public interface IBindPoint
    {
        string ScriptName { get; }

        GameObject GetGameObj();
        
        void CreateScriptButton();
    }
}