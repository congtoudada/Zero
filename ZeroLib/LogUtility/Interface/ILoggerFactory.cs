/****************************************************
  文件：ILoggerFactory.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 14:35:26
  功能：
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero.Utility
{
    public interface ILoggerFactory
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 创建Log对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        ILogger Create(Type type, string prefix = "");

        /// <summary>
        /// 设置日志打印前缀
        /// </summary>
        /// <param name="prefix"></param>
        void SetPrefix(string prefix);
    }
}
