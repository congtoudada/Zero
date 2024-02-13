/****************************************************
  文件：EventKey.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-06 00:02:00
  功能：
*****************************************************/

using System;

namespace Zero
{
    public class EventKey
    {
        public string name;
        public int hashCode;

        public EventKey()
        {
        }

        public EventKey(string name, int hashCode)
        {
            this.name = name;
            this.hashCode = hashCode;
        }

        public void Init(string name, int hashCode)
        {
            this.name = name;
            this.hashCode = hashCode;
        }
        
        public override bool Equals(object key)
        {
            if (key is EventKey target)
            {
                return hashCode == target.hashCode && name == target.name;
            }
            return false;
        }

        protected bool Equals(EventKey other)
        {
            return name == other.name && hashCode == other.hashCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, hashCode);
        }
    }
}