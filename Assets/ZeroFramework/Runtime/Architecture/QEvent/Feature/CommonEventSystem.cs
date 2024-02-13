/****************************************************
  文件：CommonEventKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-12-28 21:16:51
  功能：
*****************************************************/
using System;
using System.Collections.Generic;

namespace Zero
{
    /// <summary>
    /// 通用事件系统，操作都需要使用自定义Key寻址
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class CommonEventSystem<TKey> : ICommonEventSystem<TKey>
    {
        private Dictionary<TKey, IQEventCommon> _typeContainer = new();
        
        public IUnRegister Register<T>(TKey key, Action<T> onEvent)
        {
            if (_typeContainer.ContainsKey(key))
            {
                if (_typeContainer[key] is IQEvent<T> qEvent)
                {
                    qEvent.Register(onEvent);
                }
            }
            else
            {
                IQEvent<T> qEvent = new QEvent<T>();
                _typeContainer.Add(key, qEvent);
                qEvent.Register(onEvent);
            }
            var unregister = new UnRegisterHandler(() => { UnRegister(key, onEvent); });
            return unregister;
        }

        public void UnRegister<T>(TKey key, Action<T> onEvent)
        {
            if (_typeContainer.ContainsKey(key))
            {
                if (_typeContainer[key] is IQEvent<T> qEvent)
                {
                    qEvent.UnRegister(onEvent);
                    if (qEvent.GetInvocationList() == 0)
                    {
                        qEvent.Clear();
                        _typeContainer.Remove(key);
                    }
                }
            }
        }

        public void UnRegister(TKey key)
        {
            if (_typeContainer.ContainsKey(key))
            {
                _typeContainer[key].Clear();
                _typeContainer.Remove(key);
            }
        }

        public void Send<T>(TKey key) where T : new()
        {
            if (_typeContainer.ContainsKey(key))
            {
                if (_typeContainer[key] is IQEvent<T> qEvent)
                {
                    qEvent.Trigger(new T());
                }
            }
        }

        public void Send<T>(TKey key, T e)
        {
            if (_typeContainer.ContainsKey(key))
            {
                if (_typeContainer[key] is IQEvent<T> qEvent)
                {
                    qEvent.Trigger(e);
                }
            }
        }

        public void Clear()
        {
            _typeContainer.Clear();
            _typeContainer = new Dictionary<TKey, IQEventCommon>();
        }
    }
}