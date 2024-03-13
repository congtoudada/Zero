/****************************************************
  文件：ZeroEditorModel.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/2/28 10:25:33
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utility;

namespace Zero.Editor
{
    public class ZeroEditorModel : AbstractModel, IZeroEditorModel
    {
        public ConfigMenuEntity ConfigMenuEntity { get; }
        
        protected override void OnInit()
        {
            var storage = this.GetUtility<ZeroToolKits>().Storage;

            ConfigMenuEntity.Init(storage);

        }

        
    }
}
