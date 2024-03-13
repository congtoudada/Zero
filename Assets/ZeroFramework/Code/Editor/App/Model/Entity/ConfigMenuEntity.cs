/****************************************************
  文件：ConfigMenuEntity.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 12:27:56
  功能：
*****************************************************/

using System;
using Sirenix.OdinInspector;
using Zero.Utility;

namespace Zero.Editor
{
    public class ConfigMenuEntity
    {
        public BindableProperty<string> ScriptOutput { get; }
        
        public void Init(ISimpleStorageKit storage)
        {
            ScriptOutput.SetValueWithoutEvent(storage.LoadString(nameof(ScriptOutput)));
            ScriptOutput.Register((oldVal, newVal) =>
            {
                storage.SaveString(nameof(ScriptOutput), newVal);
            });
            
            
        }
    }
}