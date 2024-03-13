/****************************************************
  文件：ExcelTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2024/1/10 17:44:35
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using UnityEngine;

namespace Zero.Utility
{
    /// <summary>
    /// Excel句柄接口，提供对Excel表格的简单操作
    /// </summary>
    public interface IExcelHandler
    {
        //Element
        /// <summary>
        /// 取得当前工作表的最大有效行数
        /// </summary>
        int MaxRow { get; }
        /// <summary>
        /// 取得当前工作表的最大有效列数
        /// </summary>
        int MaxColumn { get; }
        /// <summary>
        /// 根据行列读取值（下标从1开始）
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        object ReadValue(int row, int column);
        /// <summary>
        /// 根据行列读取值（下标从1开始）
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ReadValue<T>(int row, int column);
        /// <summary>
        /// 根据行列写入值（下标从1开始）
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IExcelHandler WriteValue(int row, int column, object val);
        
        //WorkSheet
        /// <summary>
        /// 当前操作的工作表索引
        /// </summary>
        int SheetIdx { get; }
        /// <summary>
        /// 根据索引，打开第sheetIdx张工作表（下标从1开始）
        /// </summary>
        /// <param name="sheetIdx"></param>
        /// <returns></returns>
        IExcelHandler WithWorksheet(int sheetIdx);
        /// <summary>
        /// 根据表名，打开工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IExcelHandler WithWorksheet(string sheetName);
        /// <summary>
        /// 添加工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="isOpenNow"></param>
        /// <returns></returns>
        IExcelHandler AddSheet(string sheetName, bool isOpenNow = true);
        /// <summary>
        /// 根据索引（下标从1开始），删除工作表
        /// </summary>
        /// <param name="sheetIdx"></param>
        /// <returns></returns>
        IExcelHandler RemoveSheet(int sheetIdx);
        /// <summary>
        /// 根据表名，删除工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IExcelHandler RemoveSheet(string sheetName);
    }
    
    /// <summary>
    /// Excel句柄实现类，提供对Excel表格的简单操作。（借助EPPlus实现，建议1张表格对应1个Excel句柄）
    /// </summary>
    public class ExcelHandler : IExcelHandler
    {
        private ExcelPackage excelPackage;
        private ExcelWorksheet worksheet;
        public ExcelHandler(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            excelPackage = new ExcelPackage(fileInfo);
        }

        public int MaxRow => worksheet.Dimension.End.Row;
        public int MaxColumn => worksheet.Dimension.End.Column;

        public int SheetIdx => worksheet.Index;
        
        public object ReadValue(int row, int column)
        {
            if (row > MaxRow || column > MaxColumn) return null;
            return worksheet.GetValue(row, column);
        }

        public T ReadValue<T>(int row, int column)
        {
            if (row > MaxRow || column > MaxColumn) return default(T);
            return worksheet.GetValue<T>(row, column);
        }

        public IExcelHandler WriteValue(int row, int column, object val)
        {
            worksheet.SetValue(row, column, val);
            return this;
        }
        
        public IExcelHandler WithWorksheet(int sheetIdx)
        {
            worksheet = excelPackage.Workbook.Worksheets[sheetIdx];
            return this;
        }
        
        public IExcelHandler WithWorksheet(string sheetName)
        {
            worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(sheet => sheet.Name == sheetName);
            return this;
        }

        public IExcelHandler AddSheet(string sheetName, bool isOpenNow)
        {
            var newSheet = excelPackage.Workbook.Worksheets.Add(sheetName);
            if (isOpenNow)
            {
                worksheet = newSheet;
            }
            return this;
        }
        
        public IExcelHandler RemoveSheet(int sheetIdx)
        {
            if (sheetIdx < 1 || sheetIdx > excelPackage.Workbook.Worksheets.Count) return this;
            excelPackage.Workbook.Worksheets.Delete(sheetIdx);
            return this;
        }
        
        public IExcelHandler RemoveSheet(string sheetName)
        {
            if (excelPackage.Workbook.Worksheets.All(sheet => sheet.Name != sheetName)) return this;
            excelPackage.Workbook.Worksheets.Delete(sheetName);
            return this;
        }

        ~ExcelHandler()
        {
            worksheet?.Dispose();
            excelPackage?.Dispose();
        }
    }
    
    /// <summary>
    /// Excel构造器，用于创建并管理Excel句柄
    /// </summary>
    public class ExcelBuilder
    {
        public IExcelHandler excelHandler;

        public ExcelBuilder Build(string path)
        {
            excelHandler = new ExcelHandler(path);
            return this;
        }

        public ExcelBuilder WithWorksheet(int sheetIdx)
        {
            excelHandler?.WithWorksheet(sheetIdx);
            return this;
        }

        public ExcelBuilder WithWorksheet(string sheetName)
        {
            excelHandler?.WithWorksheet(sheetName);
            return this;
        }

        public ExcelBuilder AddSheet(string sheetName, bool isOpenNow = true)
        {
            excelHandler?.AddSheet(sheetName, isOpenNow);
            return this;
        }

        public ExcelBuilder RemoveSheet(int sheetIdx)
        {
            excelHandler?.RemoveSheet(sheetIdx);
            return this;
        }

        public ExcelBuilder RemoveSheet(string sheetName)
        {
            excelHandler?.RemoveSheet(sheetName);
            return this;
        }

        public IExcelHandler GetHandler()
        {
            return excelHandler;
        }
    }
    
    /// <summary>
    /// 文件模块：Excel工具
    /// </summary>
    public class ExcelTool
    {
        public ExcelBuilder GetExcelBuilder()
        {
            return new ExcelBuilder();
        }
        
        public ExcelBuilder GetExcelBuilder(string path)
        {
            return new ExcelBuilder().Build(path);
        }
        
        public ExcelBuilder GetExcelBuilder(string path, int sheetIdx)
        {
            return new ExcelBuilder().Build(path).WithWorksheet(sheetIdx);
        }
        
        public ExcelBuilder GetExcelBuilder(string path, string sheetName)
        {
            return new ExcelBuilder().Build(path).WithWorksheet(sheetName);
        }
    }
}
