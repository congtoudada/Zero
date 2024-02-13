/****************************************************
  文件：LogKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/22 14:23:54
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using log4net;
using UnityEngine;

namespace Zero.Utility
{
    public class LogKit : ILogKit
    {
        private Dictionary<string, ILogger> _cache;
        private List<string> _blackList;
        private List<string> _whiteList;
        private ILogger _noneLog;
        private readonly int _CACHE_CAPACITY;
        private ILoggerFactory _loggerFactory;
        private bool _isEnable;
        private bool _isListFilter;
        private LogLevel _limitLevel;

        public LogKit(ILoggerFactory factory, bool isFilter = false, 
            LogLevel logLevel = LogLevel.DEBUG,
            bool isEnable = true,
            int cache_capacity = 31)
        {
            _CACHE_CAPACITY = cache_capacity;
            if (_CACHE_CAPACITY < 1)
                _cache = new Dictionary<string, ILogger>();
            else
                _cache = new Dictionary<string, ILogger>(_CACHE_CAPACITY);

            _noneLog = new NoneLog();
            _loggerFactory = factory;
            SetListFilter(isFilter);
            SetLevelFilter(logLevel);
            SetEnable(isEnable);
        }
        public void SetEnable(bool _isEnable)
        {
            this._isEnable = _isEnable;
            _cache.Clear();
        }

        public void SetLoggerFactory(ILoggerFactory factory)
        {
            this._loggerFactory = factory;
        }

        public ILogger AllocateLogger(Type type, string prefix = "")
        {
            var logger = AllocateLoggerOnce(type, prefix);
            if (!_cache.ContainsKey(type.FullName))
            {
                _cache.Add(type.FullName, logger);
            }
            return logger;
        }

        public ILogger AllocateLoggerOnce(Type type, string prefix = "")
        {
            //日志开关和安全性检查
            if (!_isEnable || type == null)
                return _noneLog;

            //规则过滤
            if (!CheckFilter(type))
                return _noneLog;

            //缓存检查
            if (_cache.ContainsKey(type.FullName))
            {
                _cache[type.FullName].SetLevelLimit(_limitLevel);
                return _cache[type.FullName];
            }
            var logger = _loggerFactory.Create(type);
            logger.SetLevelLimit(_limitLevel);
            return logger;
        }

        private bool CheckFilter(Type type)
        {
            //规则过滤
            if (_isListFilter)
            {
                //白名单数量大于0才有效，否则只启用黑名单过滤
                if (_whiteList.Count > 0 && !_whiteList.Contains(type.FullName))
                {
                    return false;
                }
                if (_blackList.Contains(type.FullName))
                {
                    return false;
                }
            }
            return true;
        }

        public void Debug(object message, Type type)
        {
            AllocateLogger(type).Debug(message);
        }

        public void Info(object message, Type type)
        {
            AllocateLogger(type).Info(message);
        }

        public void Warn(object message, Type type)
        {
            AllocateLogger(type).Warn(message);
        }

        public void Error(object message, Type type)
        {
            AllocateLogger(type).Error(message);
        }

        public void Fatal(object message, Type type)
        {
            AllocateLogger(type).Fatal(message);
        }

        public void DebugOnce(object message, Type type)
        {
            AllocateLoggerOnce(type).Debug(message);
        }

        public void InfoOnce(object message, Type type)
        {
            AllocateLoggerOnce(type).Info(message);
        }

        public void WarnOnce(object message, Type type)
        {
            AllocateLoggerOnce(type).Warn(message);
        }

        public void ErrorOnce(object message, Type type)
        {
            AllocateLoggerOnce(type).Error(message);
        }

        public void FatalOnce(object message, Type type)
        {
            AllocateLoggerOnce(type).Fatal(message);
        }
        public void SetListFilter(bool isEnalbe)
        {
            _isListFilter = isEnalbe;
            if (_isListFilter)
            {
                if (_whiteList == null) _whiteList = new List<string>();
                if (_blackList == null) _blackList = new List<string>();
            }
        }

        public void AddWhiteList(Type type)
        {
            if (!_isListFilter) return;
            if (_whiteList.Contains(type.FullName)) return;
            _whiteList.Add(type.FullName);
        }

        public void RemoveWhiteList(Type type)
        {
            if (!_isListFilter) return;
            if (_whiteList.Contains(type.FullName))
                _whiteList.Remove(type.FullName);
        }

        public void ClearWhiteList()
        {
            if (!_isListFilter) return;
            _whiteList.Clear();
        }

        public void AddBlackList(Type type)
        {
            if (!_isListFilter) return;
            if (_blackList.Contains(type.FullName)) return;
            _blackList.Add(type.FullName);
        }

        public void RemoveBlackList(Type type)
        {
            if (!_isListFilter) return;
            if (_blackList.Contains(type.FullName))
                _blackList.Remove(type.FullName);
        }

        public void ClearBlackList()
        {
            if (!_isListFilter) return;
            _blackList.Clear();
        }

        public void SetPrefix(string prefix)
        {
            _loggerFactory.SetPrefix(prefix);
        }

        public void Log(object message, Type type, LogLevel level)
        {
            AllocateLogger(type).Log(message, level);
        }

        public void LogOnce(object message, Type type, LogLevel level)
        {
            AllocateLoggerOnce(type).Log(message, level);
        }

        public void SetLevelFilter(LogLevel level = LogLevel.DEBUG)
        {
            _limitLevel = level;
        }
    }
}
