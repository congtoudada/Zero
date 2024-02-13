/****************************************************
  文件：SingletonMonoExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/30 20:56:05
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zero.Samples
{
    [MonoSingletonPath("GameRoot/Singleton/SingletonMonoExample")]
    public class SingletonMonoExample : SingletonMono<SingletonMonoExample>
    {
        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            Debug.Log("OnSingletonInit");
            SceneManager.sceneUnloaded += Unload;
        }

        private void Unload(Scene scene)
        {
            SceneManager.sceneUnloaded -= Unload;
            Debug.Log("scene unload");
            Dispose();
        }
        
        public override void Dispose()
        {
            Debug.Log("Dispose");
            base.Dispose();
        }
    }
}
