/****************************************************
  文件：LogExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/22 14:14:58
  功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using log4net;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zero.Utility;

namespace Zero.Samples
{
  public class LogExample : MonoBehaviour
  {
    // Start is called before the first frame update
    void Start()
    {
      // 获取日志工具
      var logKit = ZeroToolKits.Instance.UserLog;
      // 1.直接打印日志
      logKit.Debug("直接调用", typeof(LogExample));
      // 2.使用logger打印日志
      var logger = logKit.AllocateLogger(typeof(LogExample));
      logger.Debug("使用Logger调用");
      
      //黑/白名单过滤（类型过滤）
      logKit.SetListFilter(true);
      logKit.AddBlackList(typeof(LogExample));
      logger = logKit.AllocateLogger(typeof(LogExample));
      logger.Debug("黑名单内容，无法打印"); //不打印
      
      logKit.RemoveBlackList(typeof(LogExample));
      logger = logKit.AllocateLogger(typeof(LogExample));
      logger.Debug("移出黑名单，正常打印！");
      
      //等级过滤
      logKit.SetLevelFilter(LogLevel.WARN);
      logger = logKit.AllocateLogger(typeof(LogExample)); //内部有缓存，可以无代价重新分配
      logger.Debug("WARN等级，无法打印Debug内容"); //不打印
      logger.SetLevelLimit(LogLevel.DEBUG); //或者直接设置当前logger
      logger.Debug("DEBUG等级，可以打印Debug内容");

      // 日志开关
      logKit.SetEnable(true);
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
      AssetDatabase.Refresh();
#endif
    }
  }
}

