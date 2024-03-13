/****************************************************
  文件：ZeroLogFactory.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/27 19:51:22
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 日志工厂
    /// </summary>
    public class ZeroLogFactory : ILoggerFactory
    {
        public readonly string LOG_LOAD_PATH;
        public readonly string LOG_OUTPUT_DIR;
        private string _prefix;
        
        public ZeroLogFactory(string prefix = "")
        {
            // string zeroPath = Path.Combine(Application.streamingAssetsPath, "Zero");
            // LOG_LOAD_PATH = Path.Combine(zeroPath, "Configs", "log4net.config"); //log4net配置
            // LOG_OUTPUT_DIR = Path.Combine(zeroPath, "Logs"); //日志输出路径
            // LOG_LOAD_PATH = ZeroToolKits.Instance._G.Get<string>("utility.log.log4net.config"); //log4net配置
            // LOG_OUTPUT_DIR = ZeroToolKits.Instance._G.Get<string>("utility.log.log4net.output"); //日志输出路径
            LOG_LOAD_PATH = ZeroToolKits.Instance._G.Get<string>(ZeroConfigKey.UTILITY__LOG__LOG4NET__CONFIG); //log4net配置
            LOG_OUTPUT_DIR = ZeroToolKits.Instance._G.Get<string>(ZeroConfigKey.UTILITY__LOG__LOG4NET__OUTPUT); //日志输出路径
            if (!File.Exists(LOG_LOAD_PATH))
            {
                Debug.LogWarning("找不到log4net配置文件: " + LOG_LOAD_PATH);
            }
            _prefix = prefix;
            
        }
        public ZeroLogFactory(string configPath, string outputPath, string prefix = "")
        {
            LOG_LOAD_PATH = configPath;
            LOG_OUTPUT_DIR = outputPath;
            this._prefix = prefix;
        }
        
        public void Init()
        {
            try
            {
                Log4netLog.Init(LOG_LOAD_PATH, LOG_OUTPUT_DIR);
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }

        public void SetPrefix(string prefix)
        {
            _prefix = prefix;
        }

        public ILogger Create(Type type, string prefix = "")
        {
            //外部传入的prefix为空，就使用初始化时指定的prefix
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = _prefix;
            }
            return new MixLog(type, prefix); //log4net + UnityEngine日志打印
        }
    }
}
