/****************************************************
  文件：IUniEventGroupKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-05 23:47:10
  功能：
*****************************************************/

namespace Zero.Utility
{
    public interface IUniEventGroupKit
    {
        /// <summary>
        /// 同时在Group和UniEvent添加监听
        /// </summary>
        /// <param name="listener"></param>
        /// <typeparam name="TEvent"></typeparam>
        void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage;
        
        /// <summary>
        /// 同时在Group和UniEvent移除所有监听
        /// </summary>
        void RemoveAllListener();
    }
}