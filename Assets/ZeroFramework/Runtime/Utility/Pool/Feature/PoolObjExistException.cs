/****************************************************
  文件：PoolObjExistException.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/23 20:03:53
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 对象池异常
    /// </summary>
    public class PoolObjExistException : Exception
    {
        public PoolObjExistException(string message) : base(message)
        {

        }
    }
}
