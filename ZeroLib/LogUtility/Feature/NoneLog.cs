/****************************************************
  文件：NoneLog.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 14:33:46
  功能：Nothing
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Zero.Utility
{
    class NoneLog : BaseLog
    {
        public NoneLog(string prefix = "") : base(prefix) { }

        public override void Debug(object message)
        {
        }

        public override void Error(object message)
        {
        }

        public override void Fatal(object message)
        {
        }

        public override void Info(object message)
        {
        }

        public override void Warn(object message)
        {
        }
    }
}
