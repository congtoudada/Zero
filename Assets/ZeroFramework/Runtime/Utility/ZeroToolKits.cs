/****************************************************
  文件：ZeroToolKits.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/22 14:25:26
  功能：打包时建议使用 [ZERO_RELEASE] 
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using log4net;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// Zero框架提供的工具箱，封装所有Utility
    /// </summary>
    public class ZeroToolKits : Singleton<ZeroToolKits>, IUtility
    {
        private ZeroToolKits()
        {
            
        }
        
        #region Config
        public IConfigKit _G
        {
            get
            {
                if (_g == null)
                {
                    _g = new ConfigKit(); //2024年2月13日02:08:00
                    _g.Equip(new ConfigInfo("application-runtime", ConfigInfo.LoadEnum.YAML));
                }
                return _g;
            }
        }
        private IConfigKit _g;
        #endregion
        
        #region Log
        //使用ZERO_RELEASE宏可以关闭日志系统
        public ILogKit InnerLog
        {
            get
            {
                if (_zeroLogK == null)
                {
                    ILoggerFactory loggerFactory = new ZeroLogFactory("[ Zero ] ");
                    loggerFactory.Init();
                    #if ZERO_RELEASE || DISABLE_LOG
                        _logKit = new LogKit(loggerFactory, false);
                    #else
                    _zeroLogK = new LogKit(loggerFactory);
                    #endif
                }
                return _zeroLogK;
            }
        }
        public ILogKit UserLog
        {
            get
            {
                if (_userLog == null)
                {
                    ILoggerFactory loggerFactory = new ZeroLogFactory(_G.Get<string>("utility.log.log4net.config"),
                        _G.Get<string>("utility.log.log4net.output"));
                    loggerFactory.Init();
                    _userLog = new LogKit(loggerFactory);
                }
                return _userLog;
            }
        }
        private ILogKit _userLog;
        private ILogKit _zeroLogK;
        #endregion
        
        #region File
        public IFileKit File
        {
            get
            {
                if (_file == null)
                {
                    _file = new FileKit();
                }
                return _file;
            }
        }
        private IFileKit _file;
        #endregion
        
        #region Pool
        public IPoolKit Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new PoolKit();
                }
                return _pool;
            }
        }
        private IPoolKit _pool;
        #endregion
        
        #region Timestamp
        public ITimestampKit Timestamp
        {
            get
            {
                if (_timestamp == null)
                {
                    _timestamp = new TimestampKit();
                }
                return _timestamp;
            }
        }
        private ITimestampKit _timestamp;
        #endregion
        
        #region Timer
        public ITimerKit Timer = TimerKit.Instance;
        #endregion
        
        #region UniEvent
        public IUniEventKit UniEvent = UniEventKit.Instance;
        #endregion

        #region Res
        public IYooResKit YooRes => YooResKit.Instance;
        #endregion
        
        #region String

        public StringKit Str => StringKit.Instance;

        #endregion

    }
}
