/****************************************************
  文件：QEventExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/12/28 22:14:50
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero.Samples
{
    struct EventExampleStruct
    {
        public string str;
    }
    public class QEventExample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TypeEventSystem typeEventSystem = new TypeEventSystem();
            typeEventSystem.Register<EventExampleStruct>(data => Debug.Log(data.str));
            EventExampleStruct data = new EventExampleStruct();
            data.str = "hello world";
            typeEventSystem.Send(data);
            typeEventSystem.UnRegister<EventExampleStruct>();
            typeEventSystem.Send(data);
            Debug.Log(typeof(EventExampleStruct).Name);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
