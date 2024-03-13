/****************************************************
  文件：IPathKit.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024-02-28 21:09:14
  功能：
*****************************************************/

namespace Zero.Utility
{
    public interface IPathKit
    {
        /// <summary>
        /// 获取ZeroFrameowrk的绝对路径
        /// </summary>
        /// <returns></returns>
        string GetZeroFolderAbsolute();

        /// <summary>
        /// 返回ZeroFramework相对于Asstes的路径（含Assets）
        /// </summary>
        /// <returns></returns>
        string GetZeroFolderRelative();
        
        /// <summary>
        /// 将Assets相对路径转换为绝对路径 (Assets/xxx --> E:/yyy/Assets/xxx)
        /// </summary>
        /// <param name="assetRelativePath"></param>
        /// <returns></returns>
        string AssetsRelativeToAbsolute(string assetRelativePath);
    }
}