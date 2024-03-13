/****************************************************
  文件：Query.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/29 21:22:34
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero
{
    /// <summary>
    /// Query接口
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem,
        ICanSendQuery
    {
        /// <summary>
        /// 执行查询，返回查询结果
        /// </summary>
        /// <returns></returns>
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do() => OnDo();

        protected abstract T OnDo();


        private IArchitecture mArchitecture;

        public IArchitecture GetArchitecture() => mArchitecture;

        public void SetArchitecture(IArchitecture architecture) => mArchitecture = architecture;
    }
}
