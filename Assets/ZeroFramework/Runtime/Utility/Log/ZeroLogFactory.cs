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
#if UNITY_EDITOR
            string zeroPath = Path.Combine(Application.streamingAssetsPath, "zero");
#else
            string zeroPath = Path.Combine(Application.persistentDataPath, "zero");
#endif
            LOG_LOAD_PATH = Path.Combine(zeroPath, "configs", "log4net.config");
            LOG_OUTPUT_DIR = Path.Combine(zeroPath, "logs");
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
            return new MixLog(type, prefix);
        }
    }
}
