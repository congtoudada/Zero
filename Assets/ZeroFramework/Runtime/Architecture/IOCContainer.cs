/****************************************************
  文件：IOCContainer.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/29 16:19:34
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zero
{
    /// <summary>
    /// IOC容器类
    /// </summary>
    public class IOCTypeContainer
    {
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        public void Register<T>(T instance) where T : class
        {
            var key = typeof(T);

            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        public void UnRegister<T>()
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                mInstances.Remove(key);
            }
        }

        public bool Containes<T>() where T : class
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (mInstances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }
            return null;
        }

        public IEnumerable<T> GetInstancesByType<T>()
        {
            var type = typeof(T);
            return mInstances.Values.Where(instance => type.IsInstanceOfType(instance)).Cast<T>();
        }

        public void Clear() => mInstances.Clear();
    }

    public class IOCStrContainer
    {
        private Dictionary<string, object> mInstances = new Dictionary<string, object>();

        public void Register<T>(string key, T instance) where T : class
        {
            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        public void UnRegister(string key)
        {
            if (mInstances.ContainsKey(key))
            {
                mInstances.Remove(key);
            }
        }
        
        public bool Containes(string key)
        {
            if (mInstances.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        public T Get<T>(string key) where T : class
        {
            if (mInstances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }
            return null;
        }

        public void Clear() => mInstances.Clear();
    }
    
    public class IOCTypeContainer<TValue> where TValue : class
    {
        private Dictionary<Type, TValue> mInstances = new Dictionary<Type, TValue>();

        public void Register<T>(TValue instance) where T : class
        {
            var key = typeof(T);

            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        public void UnRegister<T>()
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                mInstances.Remove(key);
            }
        }
        
        public bool Containes<T>() where T : class
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        public TValue Get<T>() where T : class
        {
            var key = typeof(T);
            if (mInstances.TryGetValue(key, out var retInstance))
            {
                return retInstance;
            }
            return null;
        }

        public IEnumerable<TValue> GetInstancesByType<T>()
        {
            var type = typeof(T);
            return mInstances.Values.Where(instance => type.IsInstanceOfType(instance));
        }

        public void Clear() => mInstances.Clear();
    }
}
