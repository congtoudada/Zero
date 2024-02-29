/****************************************************
  文件：BindableProperty.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/29 22:37:14
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// 可绑定属性：通过绑定属性及其回调，实现值变更触发回调函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindableProperty<T> : IBindableProperty<T>
    {
        private T mValue;
        
        // ①:old ②:new
        private Action<T, T> mOnValueChanged;

        public BindableProperty(T defaultValue = default) => mValue = defaultValue;
        
        protected virtual T GetValue() => mValue;
        
        protected virtual void SetValue(T newValue) => mValue = newValue;
        
        public static Func<T, T, bool> Comparer { get; set; } = (a, b) => a.Equals(b);
        
        public BindableProperty<T> WithComparer(Func<T, T, bool> comparer)
        {
            Comparer = comparer;
            return this;
        }
        
        public T Value
        {
            get => GetValue();
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && Comparer(value, mValue)) return;
                
                mOnValueChanged?.Invoke(mValue, value);
                SetValue(value);
            }
        }
        
        public void SetValueWithoutEvent(T newValue) => mValue = newValue;
        
        public IUnRegister Register(Action<T, T> onValueChanged)
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>(this, onValueChanged);
        }
        
        public IUnRegister RegisterWithInitValue(Action<T, T> onValueChanged)
        {
            onValueChanged(mValue, mValue);
            return Register(onValueChanged);
        }
        
        public void UnRegister(Action<T, T> onValueChanged) => mOnValueChanged -= onValueChanged;

        public void Clear()
        {
            mOnValueChanged = null;
        }

        public override string ToString() => Value.ToString();
    }
    
    public class BindablePropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T, T> OnValueChanged { get; set; }
        
        public BindablePropertyUnRegister(BindableProperty<T> bindableProperty, Action<T, T> onValueChanged)
        {
            BindableProperty = bindableProperty;
            OnValueChanged = onValueChanged;
        }

        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
    
    internal class ComparerAutoRegister
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoRegister()
        {
            BindableProperty<int>.Comparer = (a, b) => a == b;
            BindableProperty<float>.Comparer = (a, b) => a == b;
            BindableProperty<double>.Comparer = (a, b) => a == b;
            BindableProperty<string>.Comparer = (a, b) => a == b;
            BindableProperty<long>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector4>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color32>.Comparer =
                (a, b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
            BindableProperty<UnityEngine.Bounds>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Rect>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Quaternion>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.BoundsInt>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.RangeInt>.Comparer = (a, b) => a.start == b.start && a.length == b.length;
            BindableProperty<UnityEngine.RectInt>.Comparer = (a, b) => a.Equals(b);
        }
    }
}