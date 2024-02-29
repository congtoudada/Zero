/****************************************************
  文件：UniEventDriver.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-05 21:04:41
  功能：
*****************************************************/

using UnityEngine;

namespace Zero.Utility
{
    [MonoSingletonPath("ZeroFramework/Utility/UniEventDriver")]
    public class UniEventDriver : SingletonMono<UniEventDriver>
    {
        void Update()
        {
            UniEventKit.Instance.Update();
        }
    }
}