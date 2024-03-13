/****************************************************
  文件：ZeroToolKits.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/22 14:25:26
  功能：打包时建议使用 [ZERO_RELEASE] 
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using log4net;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
                    try
                    {
                        _g = new ConfigKit();
                        //加载项目默认配置
                        string zeroConfigPath = Path.Combine(Application.streamingAssetsPath, "Zero", "Configs", "application-runtime.yaml");
                        if (!System.IO.File.Exists(zeroConfigPath))
                        {
                            //此时本地配置还没加载，不可使用Log模块，否则会“死锁”
                            UnityEngine.Debug.LogWarning("找不到Zero根配置: " + zeroConfigPath);
                            _g = null;
                        }
                        else
                        {
                            _g.Equip(_g.CreateConfigInfo(zeroConfigPath, ConfigInfo.FileType.YAML, ConfigInfo.LoadType.UNITY_WEB_REQUEST));
                            InnerLog.LogOnce("默认配置加载完毕: " + _g, typeof(ZeroToolKits));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[ Zero ] _G初始化失败: " + e.StackTrace);
                        _g = null;
                    }
                    
                }
                return _g;
            }
        }
        private IConfigKit _g;
#if UNITY_EDITOR
        public IConfigKit _EG
        {
            get
            {
                if (_eg == null)
                {
                    try
                    {
                        _eg = new ConfigKit();
                        //加载项目默认配置
                        string zeroConfigPath = Path.Combine(Application.streamingAssetsPath, "Zero", "Configs", "application-editor.yaml");
                        if (!System.IO.File.Exists(zeroConfigPath))
                        {
                            //此时本地配置还没加载，不可使用Log模块，否则会“死锁”
                            UnityEngine.Debug.LogWarning("找不到Zero Editor根配置: " + zeroConfigPath);
                            _eg = null;
                        }
                        else
                        {
                            var info = _eg.CreateConfigInfo(zeroConfigPath, ConfigInfo.FileType.YAML,
                                ConfigInfo.LoadType.UNITY_WEB_REQUEST);
                            _eg.Equip(info);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[ Zero ] _EG初始化失败: " + e.StackTrace);
                        _g = null;
                    }
                }
                return _eg;
            }
        }
        private IConfigKit _eg;
#endif
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
        
        #region Path

        public IPathKit PathHelper => PathKit.Instance;
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
        
        #region Res
        public IYooResKit YooRes => YooResKit.Instance;
        #endregion
        
        #region Storage
        public ISimpleStorageKit Storage
        {
            get
            {
                if (_storage == null)
                {
                    _storage = new SimpleStorageKit(SimpleStorageInfo.StorageMethod.PLAYER_PREFS);
                }
                return _storage;
            }
        }
        private ISimpleStorageKit _storage;
        #endregion
        
        #region String
        public StringKit Str => StringKit.Instance;
        #endregion
        
        #region Timer
        public ITimerKit Timer = TimerKit.Instance;
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
        
        #region UniEvent
        public IUniEventKit UniEvent = UniEventKit.Instance;
        #endregion

    }
}
