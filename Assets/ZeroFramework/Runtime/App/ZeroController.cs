/****************************************************
  文件：ZeroController.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023-11-30 19:33:26
  功能：Nothing
*****************************************************/
using log4net;
using UnityEngine;
using Zero.Utility;
using ILogger = Zero.Utility.ILogger;

namespace Zero
{
    /// <summary>
    /// 提供自定义Controller接口范例（具体内容根据业务需要扩展）
    /// </summary>
    public interface IZeroController : IController
    {
        
    }
    
    /// <summary>
    /// 提供自定义Controller实现类扩展范例
    /// </summary>
    public static class ZeroControllerExtension
    {
        /// <summary>
        /// 获得内置 ZeroToolKits工具包
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ZeroToolKits GetZeroToolKits(this IZeroController self)
        {
            return self.GetUtility<ZeroToolKits>();
        }
    }
    
    /// <summary>
    /// 提供自定义Mono Controller实现类范例
    /// </summary>
    public class ZeroMonoController<T> : MonoBehaviour, IZeroController where T : ZeroMonoController<T>
    {
        public ILogger logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = this.GetZeroToolKits().UserLog.AllocateLogger(typeof(T));
                }

                return _logger;
            }
        }
        private ILogger _logger;
        
        public IArchitecture GetArchitecture()
        {
            return ZeroArchitecture.Interface;
        }
    }
    
    /// <summary>
    /// 提供自定义Controller实现类范例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ZeroController<T> : IZeroController where T : ZeroController<T>
    {
        public ILogger logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = this.GetZeroToolKits().UserLog.AllocateLogger(typeof(T));
                }

                return _logger;
            }
        }
        private ILogger _logger;
        
        public IArchitecture GetArchitecture()
        {
            return ZeroArchitecture.Interface;
        }
    }
}