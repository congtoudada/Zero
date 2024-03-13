/****************************************************
  文件：EditorCache.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 12:43:07
  功能：
*****************************************************/

using System;

namespace Zero.Editor
{
    //缓存属性
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EditorCacheAttribute : Attribute
    { }
}