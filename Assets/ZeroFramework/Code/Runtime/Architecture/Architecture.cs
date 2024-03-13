/****************************************************
  文件：IArchitecture.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/29 16:30:11
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zero.Utility;

namespace Zero
{
    /// <summary>
    /// Zero工厂接口，用于注册创建对象的方法到IOC容器（懒加载，当Get该对象时如果没有则创建）
    /// </summary>
    public interface IZeroFactory
    {
        object Create();
    }
    
    /// <summary>
    /// 架构接口
    /// </summary>
    public interface IArchitecture
    {
        /// <summary>
        /// 注册System模块到架构
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T RegisterSystem<T>(T system) where T : class, ISystem;
        /// <summary>
        /// 注册System工厂模块到架构
        /// </summary>
        /// <param name="systemFactory"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterSystem<T>(IZeroFactory systemFactory) where T : class, ISystem;
        /// <summary>
        /// 注册Model模块到架构
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T RegisterModel<T>(T model) where T : class, IModel;
        /// <summary>
        /// 注册Model工厂模块到架构
        /// </summary>
        /// <param name="modelFactory"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterModel<T>(IZeroFactory modelFactory) where T : class, IModel;
        /// <summary>
        /// 注册Utility模块到架构
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T RegisterUtility<T>(T utility) where T : class, IUtility;
        /// <summary>
        /// 注册Utility工厂模块到架构
        /// </summary>
        /// <param name="utilityFactory"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterUtility<T>(IZeroFactory utilityFactory) where T : class, IUtility;
        
        /// <summary>
        /// 从容器解绑System模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UnRegisterSystem<T>() where T : ISystem;
        /// <summary>
        /// 从容器解绑Model模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UnRegisterModel<T>() where T : IModel;
        /// <summary>
        /// 从容器解绑Utility模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UnRegisterUtility<T>() where T : IUtility;
        
        /// <summary>
        /// 从容器获取System模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetSystem<T>() where T : class, ISystem;
        /// <summary>
        /// 从容器获取Model模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModel<T>() where T : class, IModel;
        /// <summary>
        /// 从容器获取Utility模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetUtility<T>() where T : class, IUtility;
        
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="T"></typeparam>
        void SendCommand<T>(T command) where T : ICommand;
        /// <summary>
        /// 发送带返回值的命令
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult SendCommand<TResult>(ICommand<TResult> command);
        
        /// <summary>
        /// 发送查询（带返回值）
        /// </summary>
        /// <param name="query"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult SendQuery<TResult>(IQuery<TResult> query);
        
        /// <summary>
        /// 发送无参事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void SendEvent<T>() where T : new();
        /// <summary>
        /// 发送含参事件
        /// </summary>
        /// <param name="e"></param>
        /// <typeparam name="T"></typeparam>
        void SendEvent<T>(T e);
        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IUnRegister RegisterEvent<T>(Action<T> onEvent);
        /// <summary>
        /// 解绑指定类型的指定事件
        /// </summary>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        void UnRegisterEvent<T>(Action<T> onEvent);
        /// <summary>
        /// 解绑指定类型的所有事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UnRegisterEvent<T>();
        
        /// <summary>
        /// 销毁架构
        /// </summary>
        void Deinit();
        
        /// <summary>
        /// 架构是否初始化
        /// </summary>
        bool Initialized { get; set; }
    }
    
    /// <summary>
    /// 架构实现类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        public bool Initialized { get; set; }
        private IOCTypeContainer mTypeContainer = new IOCTypeContainer();
        private IOCTypeContainer<Func<object>> mTypeFuncContainer = new IOCTypeContainer<Func<object>>();
        private ITypeEventSystem mTypeEventKit = new TypeEventSystem();
        
        // 类似单例实现架构类
        private static T mArchitecture;
        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null) MakeSureArchitecture();
                return mArchitecture;
            }
        }
    
        #region Init
        private static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                foreach (var model in mArchitecture.mTypeContainer.GetInstancesByType<IModel>().Where(m=>!m.Initialized))
                {
                    model.Init();
                    model.Initialized = true;
                }
                
                foreach (var system in mArchitecture.mTypeContainer.GetInstancesByType<ISystem>().Where(m=>!m.Initialized))
                {
                    system.Init();
                    system.Initialized = true;
                }
                
                mArchitecture.Initialized = true;
            }
        }

        protected abstract void Init();

        public void Deinit()
        {
            OnDeinit();
            foreach (var system in mTypeContainer.GetInstancesByType<ISystem>().Where(s=>s.Initialized)) system.Deinit();
            foreach (var model in mTypeContainer.GetInstancesByType<IModel>().Where(m=>m.Initialized)) model.Deinit();
            mTypeContainer.Clear();
            mTypeFuncContainer.Clear();
            mArchitecture = null;
            Initialized = false;
        }

        protected virtual void OnDeinit() { }
        #endregion

        #region Register
        public TSystem RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            if (system == null) return null;
            system.SetArchitecture(this);
            mTypeContainer.Register<TSystem>(system);

            if (Initialized)
            {
                system.Init();
                system.Initialized = true;
            }
            return system;
        }

        public void RegisterSystem<TSystem>(IZeroFactory systemFactory) where TSystem : class, ISystem
        {
            mTypeFuncContainer.Register<TSystem>(systemFactory.Create);
        }

        public TModel RegisterModel<TModel>(TModel model) where TModel : class, IModel
        {
            if (model == null) return null;
            model.SetArchitecture(this);
            mTypeContainer.Register<TModel>(model);

            if (Initialized)
            {
                model.Init();
                model.Initialized = true;
            }
            return model;
        }

        public void RegisterModel<TModel>(IZeroFactory modelFactory) where TModel : class, IModel
        {
            mTypeFuncContainer.Register<TModel>(modelFactory.Create);
        }

        public TUtility RegisterUtility<TUtility>(TUtility utility) where TUtility : class, IUtility
        {
            if (utility == null) return null;
            mTypeContainer.Register<TUtility>(utility);
            return utility;
        }

        public void RegisterUtility<TUtility>(IZeroFactory utilityFactory) where TUtility : class, IUtility
        {
            mTypeFuncContainer.Register<TUtility>(utilityFactory.Create);
        }

        #endregion
        
        #region UnRegister
        public void UnRegisterSystem<TSystem>() where TSystem : ISystem
        {
            mTypeContainer.UnRegister<TSystem>();
            mTypeFuncContainer.UnRegister<TSystem>();
        }

        public void UnRegisterModel<TModel>() where TModel : IModel
        {
            mTypeContainer.UnRegister<TModel>();
            mTypeFuncContainer.UnRegister<TModel>();
        }

        public void UnRegisterUtility<TUtility>() where TUtility : IUtility
        {
            mTypeContainer.UnRegister<TUtility>();
            mTypeFuncContainer.UnRegister<TUtility>();
        }

        #endregion
        
        #region Get

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            TSystem result = mTypeContainer.Get<TSystem>();
            if (result == null)
            {
                var creator = mTypeFuncContainer.Get<TSystem>();
                if (creator != null)
                {
                    result = this.RegisterSystem<TSystem>(creator() as TSystem);
                }
            }
            return result;
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            TModel result = mTypeContainer.Get<TModel>();
            if (result == null)
            {
                var creator = mTypeFuncContainer.Get<TModel>();
                if (creator != null)
                {
                    result = this.RegisterModel<TModel>(creator() as TModel);
                }
            }
            return result;
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            TUtility result = mTypeContainer.Get<TUtility>();
            if (result == null)
            {
                var creator = mTypeFuncContainer.Get<TUtility>();
                if (creator != null)
                {
                    result = this.RegisterUtility<TUtility>(creator() as TUtility);
                }
            }
            return result;
        }
        
        #endregion
        
        #region Command,Query,Event
        public TResult SendCommand<TResult>(ICommand<TResult> command) => ExecuteCommand(command);

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand => ExecuteCommand(command);

        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.SetArchitecture(this);
            return command.Execute();
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query) => DoQuery<TResult>(query);

        protected virtual TResult DoQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        public void SendEvent<TEvent>() where TEvent : new() => mTypeEventKit.Send<TEvent>();

        public void SendEvent<TEvent>(TEvent e) => mTypeEventKit.Send<TEvent>(e);

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent) => mTypeEventKit.Register<TEvent>(onEvent);

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent) => mTypeEventKit.UnRegister<TEvent>(onEvent);
        
        public void UnRegisterEvent<TEvent>() => mTypeEventKit.UnRegister<TEvent>();
        
        #endregion
    }
}
