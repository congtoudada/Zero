/****************************************************
  文件：ConfigExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/27 16:44:07
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Samples
{
    public class ConfigExample : MonoBehaviour
    {
        private void Start()
        {
            //打印全局默认配置
            Debug.Log(ZeroToolKits.Instance._G);
            foreach (var key in ZeroToolKits.Instance._G.Keys)
            {
                Debug.Log(key);
            }
            //使用RESOURCES加载SCRIPTABLE配置（ZeroConfig_1包含依赖项ZeroConfig_2）
            ZeroToolKits.Instance._G.Equip(new ConfigInfo("ZeroConfig_1", 
                ConfigInfo.FileType.SCRIPTABLE,
                ConfigInfo.LoadType.RESOURCES, 
                ConfigInfo.OrderType.AFTER_INCLUDE));
            
            Debug.Log(ZeroToolKits.Instance._G);
            
        }
    }
}