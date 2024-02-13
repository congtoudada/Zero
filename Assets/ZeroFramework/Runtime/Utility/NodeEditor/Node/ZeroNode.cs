/****************************************************
  文件：ZeroNode.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/8 15:12:01
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero.Utility;

namespace Zero.Utility
{
    /// <summary>
    /// 节点抽象基类
    /// </summary>
    public abstract class ZeroNode : ScriptableObject
    {
        public enum State{ Waiting, Running, Success, Failure }
        public enum Type
        {
            Single2Single, //一对一
            Many2Many, //多对多
            Single2Many, //一对多
            Many2Single //多对一
        }
        [ReadOnly] public ZeroNode asset;
        [ReadOnly] public Type type;
        [ReadOnly, NonSerialized, FoldoutGroup("Debug"), ShowInInspector] public bool started = false; //是否初始化
        [ReadOnly, NonSerialized, FoldoutGroup("Debug"), ShowInInspector] public State currentState = State.Waiting; //当前节点状态
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        public Action OnChildrenChangeInInspector; //当孩子节点通过Inspector设置时触发

        public string description; //节点描述
                
        public ZeroNode Update()
        {
            if (!started)
            {
                currentState = State.Running;
                started = true;
                OnStart();
            }

            ZeroNode node = OnUpdate();

            if (currentState != State.Running)
            {
                started = false;
                OnStop();
            }
            return node;
        }

        protected abstract void OnStart();
        protected abstract ZeroNode OnUpdate();
        protected abstract void OnStop();

#if UNITY_EDITOR
        public abstract void AddChild(ZeroNode child);
        public abstract void RemoveChild(ZeroNode child);
        public abstract List<ZeroNode> GetChildren();
#endif
    }
}
