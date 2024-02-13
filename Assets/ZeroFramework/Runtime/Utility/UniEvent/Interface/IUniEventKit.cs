/****************************************************
  文件：IUniEventKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-06 01:06:06
  功能：
*****************************************************/

namespace Zero.Utility
{
    public interface IUniEventKit
    {
        /// <summary>
        /// 清空所有监听
        /// </summary>
        void ClearAll();

        /// <summary>
        /// 添加监听
        /// </summary>
        void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage;

        /// <summary>
        /// 添加监听
        /// </summary>
        void AddListener(System.Type eventType, System.Action<IEventMessage> listener);

        /// <summary>
        /// 添加监听
        /// </summary>
        void AddListener(EventKey eventId, System.Action<IEventMessage> listener);


        /// <summary>
        /// 移除监听
        /// </summary>
        void RemoveListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage;

        /// <summary>
        /// 移除监听
        /// </summary>
        void RemoveListener(System.Type eventType, System.Action<IEventMessage> listener);

        /// <summary>
        /// 移除监听
        /// </summary>
        void RemoveListener(EventKey eventId, System.Action<IEventMessage> listener);


        /// <summary>
        /// 实时广播事件
        /// </summary>
        void SendMessage(IEventMessage message);

        /// <summary>
        /// 实时广播事件
        /// </summary>
        void SendMessage(EventKey eventId, IEventMessage message);

        /// <summary>
        /// 延迟广播事件
        /// </summary>
        void PostMessage(IEventMessage message);

        /// <summary>
        /// 延迟广播事件
        /// </summary>
        void PostMessage(EventKey eventId, IEventMessage message);
    }
}