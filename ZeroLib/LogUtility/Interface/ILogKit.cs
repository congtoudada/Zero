/****************************************************
  文件：ILogKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 14:35:40
  功能：
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero.Utility
{
    public interface ILogKit
    {
        /// <summary>
        /// 全局日志开关
        /// </summary>
        /// <param name="isEnable"></param>
        void SetEnable(bool isEnable);

        /// <summary>
        /// 设置日志工厂
        /// </summary>
        /// <param name="factory"></param>
        void SetLoggerFactory(ILoggerFactory factory);

        /// <summary>
        /// 日志黑/白名单过滤开关
        /// </summary>
        /// <param name="isEnalbe"></param>
        void SetListFilter(bool isEnalbe);

        /// <summary>
        /// 日志等级过滤开关 (大于等于该等级的日志才会打印，故Fatal日志始终打印)
        /// </summary>
        /// <param name="level"></param>
        void SetLevelFilter(LogLevel level);

        /// <summary>
        /// 设置日志打印前缀
        /// </summary>
        /// <param name="prefix"></param>
        void SetPrefix(string prefix);

        /// <summary>
        /// 加入白名单：只有白名单内的日志能被打印，白名单元素大于0才生效
        /// </summary>
        /// <param name="type"></param>
        void AddWhiteList(Type type);

        /// <summary>
        /// 移出白名单
        /// </summary>
        /// <param name="type"></param>
        void RemoveWhiteList(Type type);

        /// <summary>
        /// 清空白名单
        /// </summary>
        void ClearWhiteList();

        /// <summary>
        /// 加入黑名单：黑名单内类型不打印日志
        /// </summary>
        /// <param name="type"></param>
        void AddBlackList(Type type);

        /// <summary>
        /// 移出黑名单
        /// </summary>
        /// <param name="type"></param>
        void RemoveBlackList(Type type);

        /// <summary>
        /// 清空黑名单
        /// </summary>
        void ClearBlackList();

        /// <summary>
        /// 分配Logger
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        ILogger AllocateLogger(Type type, string prefix = "");

        /// <summary>
        /// 分配Logger
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        ILogger AllocateLoggerOnce(Type type, string prefix = "");

        /// <summary>
        /// 打印Debug消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void Debug(object message, Type type);

        /// <summary>
        /// 打印Info消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void Info(object message, Type type);

        /// <summary>
        /// 打印Warn消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void Warn(object message, Type type);

        /// <summary>
        /// 打印Error消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void Error(object message, Type type);

        /// <summary>
        /// 打印Fatal信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void Fatal(object message, Type type);

        /// <summary>
        /// 打印指定等级的Log消息（默认Debug）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="level"></param>
        void Log(object message, Type type, LogLevel level = LogLevel.DEBUG);

        /// <summary>
        /// 打印Debug信息一次（内部不缓存）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void DebugOnce(object message, Type type);

        /// <summary>
        /// 打印Info信息一次（内部不缓存）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void InfoOnce(object message, Type type);

        /// <summary>
        /// 打印Warn信息一次（内部不缓存）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void WarnOnce(object message, Type type);

        /// <summary>
        /// 打印Error信息一次（内部不缓存）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void ErrorOnce(object message, Type type);

        /// <summary>
        /// 打印Fatal信息一次（内部不缓存）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        void FatalOnce(object message, Type type);

        /// <summary>
        /// 打印指定等级的Log消息一次（默认Debug，内部不缓存）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="level"></param>
        void LogOnce(object message, Type type, LogLevel level = LogLevel.DEBUG);
    }
}
