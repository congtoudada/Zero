/****************************************************
  文件：UniEvent.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-05 21:02:59
  功能：
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
	public class UniEventKit : Singleton<UniEventKit>, IUniEventKit
	{
		private class PostWrapper
		{
			public int PostFrame;
			public EventKey EventID;
			public IEventMessage Message;

			public void OnRelease()
			{
				PostFrame = 0;
				EventID = null;
				Message = null;
			}
		}

		private bool _isInitialize = false;
		// private static GameObject _driver = null;
		private readonly Dictionary<EventKey, LinkedList<Action<IEventMessage>>> _listeners = new Dictionary<EventKey, LinkedList<Action<IEventMessage>>>(1000);
		private readonly List<PostWrapper> _postingList = new List<PostWrapper>(100);
		private IZeroObjectPool<EventKey> _pool = new SimpleObjectPool<EventKey>(() => new EventKey(), maxSize: 100);

		private UniEventKit() {}

		// public override void OnSingletonInit()
		// {
		// 	base.OnSingletonInit();
		// 	Initalize();
		// }

		public override void Dispose()
		{
			base.Dispose();
			Destroy();
		}

		/// <summary>
		/// 初始化事件系统
		/// </summary>
		public void Initalize()
		{
			if (_isInitialize)
			{
				UniLogger.Error($"{nameof(UniEventKit)} is initialized !");
				return;
			}
			// 创建驱动器
			_isInitialize = true;

			// _driver = UniEventDriver.Instance.gameObject;
			// _driver.AddComponent<UniEventDriver>();
			// UnityEngine.Object.DontDestroyOnLoad(_driver);
			UniEventDriver.Instance.enabled = true;
			UniLogger.Log($"{nameof(UniEventKit)} initalize !");
		}

		/// <summary>
		/// 销毁事件系统
		/// </summary>
		private void Destroy()
		{
			if (_isInitialize)
			{
				ClearAll();

				_isInitialize = false;
				// if (_driver != null)
				// 	GameObject.Destroy(_driver);
				UniEventDriver.Instance.enabled = false;
				UniLogger.Log($"{nameof(UniEventKit)} destroy all !");
			}
		}

		/// <summary>
		/// 更新事件系统
		/// </summary>
		internal void Update()
		{
			for (int i = _postingList.Count - 1; i >= 0; i--)
			{
				var wrapper = _postingList[i];
				if (UnityEngine.Time.frameCount > wrapper.PostFrame)
				{
					SendMessage(wrapper.EventID, wrapper.Message);
					_pool.Release(wrapper.EventID); //延迟回收
					_postingList.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// 清空所有监听
		/// </summary>
		public void ClearAll()
		{
			foreach (EventKey eventId in _listeners.Keys)
			{
				_listeners[eventId].Clear();
			}
			_listeners.Clear();
			_postingList.Clear();
		}

		/// <summary>
		/// 添加监听
		/// </summary>
		public void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
		{
			EventKey eventId = new EventKey(typeof(TEvent).Name, typeof(TEvent).GetHashCode());
			AddListener(eventId, listener);
		}

		/// <summary>
		/// 添加监听
		/// </summary>
		public void AddListener(System.Type eventType, System.Action<IEventMessage> listener)
		{
			EventKey eventId = new EventKey(eventType.Name, eventType.GetHashCode());
			AddListener(eventId, listener);
		}

		/// <summary>
		/// 添加监听
		/// </summary>
		public void AddListener(EventKey eventId, System.Action<IEventMessage> listener)
		{
			if (_listeners.ContainsKey(eventId) == false)
				_listeners.Add(eventId, new LinkedList<Action<IEventMessage>>());
			if (_listeners[eventId].Contains(listener) == false)
				_listeners[eventId].AddLast(listener);
		}


		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
		{
			EventKey eventId = new EventKey(typeof(TEvent).Name, typeof(TEvent).GetHashCode());
			RemoveListener(eventId, listener);
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener(System.Type eventType, System.Action<IEventMessage> listener)
		{
			EventKey eventId = new EventKey(eventType.Name, eventType.GetHashCode());
			RemoveListener(eventId, listener);
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		public void RemoveListener(EventKey eventId, System.Action<IEventMessage> listener)
		{
			if (_listeners.ContainsKey(eventId))
			{
				if (_listeners[eventId].Contains(listener))
					_listeners[eventId].Remove(listener);
			}
		}


		/// <summary>
		/// 实时广播事件
		/// </summary>
		public void SendMessage(IEventMessage message)
		{
			EventKey eventId = _pool.Get();
			eventId.Init(message.GetType().Name, message.GetType().GetHashCode());
			SendMessage(eventId, message);
			_pool.Release(eventId); //立即回收
		}

		/// <summary>
		/// 实时广播事件
		/// </summary>
		public void SendMessage(EventKey eventId, IEventMessage message)
		{
			if (!_listeners.ContainsKey(eventId))
				return;

			LinkedList<Action<IEventMessage>> listeners = _listeners[eventId];
			if (listeners.Count > 0)
			{
				var currentNode = listeners.Last;
				while (currentNode != null)
				{
					currentNode.Value.Invoke(message);
					currentNode = currentNode.Previous;
				}
			}
		}

		/// <summary>
		/// 延迟广播事件
		/// </summary>
		public void PostMessage(IEventMessage message)
		{
			EventKey eventId = _pool.Get();
			eventId.Init(message.GetType().Name, message.GetType().GetHashCode());
			PostMessage(eventId, message);
		}

		/// <summary>
		/// 延迟广播事件
		/// </summary>
		public void PostMessage(EventKey eventId, IEventMessage message)
		{
			var wrapper = new PostWrapper();
			wrapper.PostFrame = UnityEngine.Time.frameCount;
			wrapper.EventID = eventId;
			wrapper.Message = message;
			_postingList.Add(wrapper);
		}
	}
}