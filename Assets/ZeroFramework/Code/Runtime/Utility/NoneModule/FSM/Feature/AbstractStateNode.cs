/****************************************************
  文件：AbstractStateNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-05 11:45:15
  功能：
*****************************************************/

namespace Zero.Utility
{
    public abstract class AbstractStateNode : IStateNode
    {
        public IStateMachine FSM { get; private set; } = null;

        public virtual bool OnCondition()
        {
            return true;
        }

        public virtual void OnCreate(IStateMachine stateMachine)
        {
            FSM = stateMachine;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnGUI()
        {

        }

        public virtual void OnExit()
        {

        }
    }
}