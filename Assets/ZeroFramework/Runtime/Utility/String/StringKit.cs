/****************************************************
  文件：StringKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/11 22:22:08
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Zero.Utility
{
    public class StringKit : Singleton<StringKit>
    {
        private StringKit()
        {
        }

        /// <summary>
        /// 正则表达式
        /// </summary>
        private static readonly Regex REGEX = new Regex(@"\{[-+]?[0-9]+\.?[0-9]*\}", RegexOptions.IgnoreCase);
        
        [ThreadStatic]
        private static StringBuilder _cacheBuilder = new StringBuilder(1024);
        public static string Format(string format, object arg0)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException();

            _cacheBuilder.Length = 0;
            _cacheBuilder.AppendFormat(format, arg0);
            return _cacheBuilder.ToString();
        }
        public static string Format(string format, object arg0, object arg1)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException();

            _cacheBuilder.Length = 0;
            _cacheBuilder.AppendFormat(format, arg0, arg1);
            return _cacheBuilder.ToString();
        }
        public static string Format(string format, object arg0, object arg1, object arg2)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException();

            _cacheBuilder.Length = 0;
            _cacheBuilder.AppendFormat(format, arg0, arg1, arg2);
            return _cacheBuilder.ToString();
        }
        public static string Format(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException();

            if (args == null)
                throw new ArgumentNullException();

            _cacheBuilder.Length = 0;
            _cacheBuilder.AppendFormat(format, args);
            return _cacheBuilder.ToString();
        }

        /// <summary>
        /// 字节数组转为字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// 字符串转为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public byte[] StringToBytes(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
        
        /// <summary>
        /// 字符串转换为BOOL
        /// </summary>
        public static bool StringToBool(string str)
        {
            int value = (int)Convert.ChangeType(str, typeof(int));
            return value > 0;
        }
        
        /// <summary>
        /// 字符串转换为数值
        /// </summary>
        public T StringToValue<T>(string str)
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }
        
        /// <summary>
        /// 字符串转换为数值列表
        /// </summary>
        /// <param name="separator">分隔符</param>
        public List<T> StringToValueList<T>(string str, char separator)
        {
            List<T> result = new List<T>();
            if (!String.IsNullOrEmpty(str))
            {
                string[] splits = str.Split(separator);
                foreach (string split in splits)
                {
                    if (!String.IsNullOrEmpty(split))
                    {
                        result.Add((T)Convert.ChangeType(split, typeof(T)));
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// 字符串转为字符串列表
        /// </summary>
        public List<string> StringToStringList(string str, char separator)
        {
            List<string> result = new List<string>();
            if (!String.IsNullOrEmpty(str))
            {
                string[] splits = str.Split(separator);
                foreach (string split in splits)
                {
                    if (!String.IsNullOrEmpty(split))
                    {
                        result.Add(split);
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// 转换为枚举
        /// 枚举索引转换为枚举类型
        /// </summary>
        public T IndexToEnum<T>(string index) where T : IConvertible
        {
            int enumIndex = (int)Convert.ChangeType(index, typeof(int));
            return IndexToEnum<T>(enumIndex);
        }
        
        /// <summary>
        /// 转换为枚举
        /// 枚举索引转换为枚举类型
        /// </summary>
        public T IndexToEnum<T>(int index) where T : IConvertible
        {
            if (Enum.IsDefined(typeof(T), index) == false)
            {
                throw new ArgumentException($"Enum {typeof(T)} is not defined index {index}");
            }
            return (T)Enum.ToObject(typeof(T), index);
        }
        
        /// <summary>
        /// 转换为枚举
        /// 枚举名称转换为枚举类型
        /// </summary>
        public T NameToEnum<T>(string name)
        {
            if (Enum.IsDefined(typeof(T), name) == false)
            {
                throw new ArgumentException($"Enum {typeof(T)} is not defined name {name}");
            }
            return (T)Enum.Parse(typeof(T), name);
        }
        
        /// <summary>
        /// 字符串转换为参数列表
        /// </summary>
        public List<float> StringToParams(string str)
        {
            List<float> result = new List<float>();
            MatchCollection matches = REGEX.Matches(str);
            for (int i = 0; i < matches.Count; i++)
            {
                string value = matches[i].Value.Trim('{', '}');
                result.Add(StringToValue<float>(value));
            }
            return result;
        }
    }
}
