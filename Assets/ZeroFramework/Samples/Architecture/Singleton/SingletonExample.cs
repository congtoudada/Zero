/****************************************************
  文件：SingletonExample.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2023/11/30 21:01:36
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zero.Samples
{
    public class SingletonExample : MonoBehaviour
    {
        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(SingletonMonoExample.Instance.GetID());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene("SingletonExample2");
            }
#endif
        }
    }
}
