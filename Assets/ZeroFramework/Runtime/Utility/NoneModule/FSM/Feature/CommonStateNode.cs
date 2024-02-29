/****************************************************
  文件：CommonStateNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-03 16:36:44
  功能：
*****************************************************/

using System;

namespace Zero.Utility
{
    /// <summary>
    /// 有限状态机的状态类
    /// </summary>
    public class CommonStateNode : IStateNode
    {
        private Func<bool> mOnCondition = () => true;
        private Action mOnCreate;
        private Action mOnEnter;
        private Action mOnUpdate;
        private Action mOnFixedUpdate;
        private Action mOnGUI;
        private Action mOnExit;
        public IStateMachine FSM { get; private set; } = null;

        public CommonStateNode OnCondition(Func<bool> onCondition)
        {
            mOnCondition = onCondition;
            return this;
        }

        public CommonStateNode OnCreate(Action OnCreate)
        {
            mOnCreate = OnCreate;
            return this;
        }
        
        
        public CommonStateNode OnEnter(Action onEnter)
        {
            mOnEnter = onEnter;
            return this;
        }

        
        public CommonStateNode OnUpdate(Action onUpdate)
        {
            mOnUpdate = onUpdate;
            return this;
        }
        
        public CommonStateNode OnFixedUpdate(Action onFixedUpdate)
        {
            mOnFixedUpdate = onFixedUpdate;
            return this;
        }
        
        public CommonStateNode OnGUI(Action onGUI)
        {
            mOnGUI = onGUI;
            return this;
        }
        
        public CommonStateNode OnExit(Action onExit)
        {
            mOnExit = onExit;
            return this;
        }

        public bool OnCondition()
        {
            var result = mOnCondition?.Invoke();
            return result == null || result.Value;
        }

        public void OnCreate(IStateMachine stateMachine)
        {
            FSM = stateMachine;
            mOnCreate?.Invoke();
        }

        public void OnEnter()
        {
            mOnEnter?.Invoke();
        }

        public void OnUpdate()
        {
            mOnUpdate?.Invoke();
        }

        public void OnFixedUpdate()
        {
            mOnFixedUpdate?.Invoke();
        }

        public void OnGUI()
        {
            mOnGUI?.Invoke();
        }

        public void OnExit()
        {
            mOnExit?.Invoke();
        }
    }
}