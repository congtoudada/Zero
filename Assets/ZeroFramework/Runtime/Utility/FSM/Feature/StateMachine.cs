/****************************************************
  文件：FSM.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-03 16:43:27
  功能：
*****************************************************/

using System;
using System.Collections.Generic;

namespace Zero.Utility
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public class StateMachine : IStateMachine
    {
        protected Dictionary<string, IStateNode> _nodes = new Dictionary<string, IStateNode>();
        protected Dictionary<string, object> _blackboard = new Dictionary<string, object>();
        public object Owner { get; private set; } = null;
        public IStateNode CurrentState { get; protected set; } = null;
        public IStateNode PreviousState { get; protected set; } = null; //启动和删除前一个节点时会使该值置空
        public long FrameCountOfCurrentState { get; private set; } = 0;
        public float SecondsOfCurrentState { get; private set; } = 0f;
        private Action<IStateNode, IStateNode> mOnStateChanged = null;
        protected ILogger logger;

        public StateMachine(object owner)
        {
            this.Owner = owner;
            logger = ZeroToolKits.Instance.InnerLog.AllocateLogger(typeof(StateMachine));
        }
        
        public IStateMachine AddState<T>() where T : IStateNode
        {
            var nodeType = typeof(T);
            var stateNode = Activator.CreateInstance(nodeType) as IStateNode;
            AddState(stateNode);
            return this;
        }

        public IStateMachine AddState(IStateNode stateNode)
        {
            if (stateNode == null)
            {
                logger.Error($"State node is null!");
                return this;
            }

            var nodeType = stateNode.GetType();
            var nodeName = nodeType.FullName;

            if (!_nodes.ContainsKey(nodeName))
            {
                stateNode.OnCreate(this);
                _nodes.Add(nodeName, stateNode);
            }
            else
            {
                logger.Error($"State node already existed : {nodeName}");
            }
            return this;
        }

        public IStateMachine AddOnStateChanged(Action<IStateNode, IStateNode> onStateChanged)
        {
            mOnStateChanged += onStateChanged;
            return this;
        }

        public IStateMachine RemoveOnStateChanged(Action<IStateNode, IStateNode> onStateChanged)
        {
            mOnStateChanged -= onStateChanged;
            return this;
        }

        public bool RemoveState<T>() where T : IStateNode
        {
            //如果状态正在运行则返回
            string stateName = typeof(T).FullName;
            return RemoveState(stateName);
        }

        public bool RemoveState(IStateNode stateNode)
        {
            return RemoveState(stateNode.GetType().FullName);
        }

        private bool RemoveState(string stateName)
        {
            //如果状态正在运行则返回
            if (CurrentState != null)
            {
                string currentName = CurrentState.GetType().FullName;
                if (currentName != null && currentName.Equals(stateName))
                {
                    return false;
                }
            }
            if (stateName != null && _nodes.ContainsKey(stateName))
            {
                string previousName = PreviousState.GetType().FullName;
                //如果旧状态是被删除的状态，则重置旧id
                if (previousName != null && previousName.Equals(stateName))
                {
                    PreviousState = null;
                }
                _nodes.Remove(stateName);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _blackboard.Clear();
            _blackboard = new Dictionary<string, object>();
            _nodes.Clear();
            _nodes = new Dictionary<string, IStateNode>();
            mOnStateChanged = null;
        }

        public IStateNode TryGetState<T>() where T : IStateNode
        {
            string nodeName = typeof(T).FullName;
            return TryGetState(nodeName);
        }

        public IStateNode TryGetState(string nodeName)
        {
            _nodes.TryGetValue(nodeName, out IStateNode result);
            return result;
        }

        public void Run<T>()
        {
            var nodeType = typeof(T);
            var nodeName = nodeType.FullName;
            Run(nodeName);
        }
        public void Run(Type entryNode)
        {
            var nodeName = entryNode.FullName;
            Run(nodeName);
        }
        public void Run(string entryNode)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
                PreviousState = CurrentState;
            }
            CurrentState = TryGetState(entryNode);
            
            if (CurrentState == null)
                logger.Error($"Not found entry node: {entryNode}");
            
            mOnStateChanged?.Invoke(PreviousState, CurrentState);
            FrameCountOfCurrentState = 1;
            SecondsOfCurrentState = 0.0f;
            CurrentState.OnEnter();
        }

        public void ChangeState<T>()
        {
            //相同状态直接返回
            string nextNodeName = typeof(T).FullName;
            string curNodeName = CurrentState.GetType().FullName;
            if (curNodeName == null || curNodeName.Equals(nextNodeName)) return;
            
            if (_nodes.TryGetValue(nextNodeName, out IStateNode nextNode))
            {
                if (CurrentState != null && CurrentState.OnCondition())
                {
                    CurrentState.OnExit();
                    PreviousState = CurrentState;
                    CurrentState = nextNode;
                    mOnStateChanged?.Invoke(PreviousState, CurrentState);
                    FrameCountOfCurrentState = 1;
                    SecondsOfCurrentState = 0.0f;
                    CurrentState.OnEnter();
                }
            }
        }

        public void Update()
        {
            CurrentState?.OnUpdate();
            FrameCountOfCurrentState++;
            SecondsOfCurrentState += UnityEngine.Time.deltaTime;
        }

        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }

        public void OnGUI()
        {
            CurrentState?.OnGUI();
        }

        public void SetBlackboardValue(string key, object value)
        {
            if (_blackboard.ContainsKey(key))
            {
                _blackboard[key] = value;
            }
            else
            {
                _blackboard.Add(key, value);
            }
            
        }

        public object GetBlackboardValue(string key)
        {
            if (_blackboard.ContainsKey(key))
                return _blackboard[key];
            return null;
        }

        public void ClearBlackboard()
        {
            _blackboard.Clear();
            _blackboard = new Dictionary<string, object>();
        }
    }
}