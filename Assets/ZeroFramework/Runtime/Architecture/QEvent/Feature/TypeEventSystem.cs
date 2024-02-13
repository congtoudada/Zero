/****************************************************
  文件：TypeEventSystem.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 22:38:11
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Zero
{
    /// <summary>
    /// 使用上与类型事件系统一致，实现上使用泛型类型名(string)作为key
    /// </summary>
    public class TypeEventSystem : ITypeEventSystem
    {
        private Dictionary<EventKey, IQEventCommon> _typeContainer = new();
        private ObjectPool<EventKey> _pool;

        public TypeEventSystem()
        {
            _pool = new ObjectPool<EventKey>(() => new EventKey());
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            EventKey t = new EventKey(typeof(T).Name, typeof(T).GetHashCode());
            if (_typeContainer.ContainsKey(t))
            {
                if (_typeContainer[t] is IQEvent<T> qEvent)
                {
                    qEvent.Register(onEvent);
                }
            }
            else
            {
                IQEvent<T> qEvent = new QEvent<T>();
                _typeContainer.Add(t, qEvent);
                qEvent.Register(onEvent);
            }
            var unregister = new UnRegisterHandler(() => { UnRegister(onEvent); });
            return unregister;
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            EventKey t = new EventKey(typeof(T).Name, typeof(T).GetHashCode());
            if (_typeContainer.ContainsKey(t))
            {
                if (_typeContainer[t] is IQEvent<T> qEvent)
                {
                    qEvent.UnRegister(onEvent);
                    if (qEvent.GetInvocationList() == 0)
                    {
                        qEvent.Clear();
                        _typeContainer.Remove(t);
                    }
                }
            }
        }

        public void UnRegister<T>()
        {
            EventKey t = new EventKey(typeof(T).Name, typeof(T).GetHashCode());
            if (_typeContainer.ContainsKey(t))
            {
                _typeContainer[t].Clear();
                _typeContainer.Remove(t);
            }
        }

        public void Send<T>() where T : new()
        {
            EventKey t = _pool.Get();
            t.Init(typeof(T).Name, typeof(T).GetHashCode());
            if (_typeContainer.ContainsKey(t))
            {
                if (_typeContainer[t] is IQEvent<T> easyEvent)
                {
                    easyEvent.Trigger(new T());
                }
            }
            _pool.Release(t);
        }

        public void Send<T>(T e)
        {
            EventKey t = _pool.Get();
            t.Init(typeof(T).Name, typeof(T).GetHashCode());
            if (_typeContainer.ContainsKey(t))
            {
                if (_typeContainer[t] is IQEvent<T> easyEvent)
                {
                    easyEvent.Trigger(e);
                }
            }
            _pool.Release(t);
        }

        public void Clear()
        {
            _typeContainer.Clear();
            _pool.Clear();
            _typeContainer = new Dictionary<EventKey, IQEventCommon>();
        }
    }
}