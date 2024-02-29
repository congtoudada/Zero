/****************************************************
  文件：CLASS.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 10:27:23
  功能：
*****************************************************/

using Zero.Utility;

namespace Zero.Editor
{
    public interface IZeroEditorModel : IModel
    {
        ConfigMenuEntity ConfigMenuEntity { get; }
    }
}