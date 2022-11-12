// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2019-10-01 10:09 </creationDate>
// <summary>
//      Defines the ExcelExtensions type to share additional extensions and functions for EXCEL application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FormsExtensions.Excel
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using FormsExtensions.Properties;
    using JetBrains.Annotations;
    using Microsoft.Office.Interop.Excel;
    using Application = System.Windows.Forms.Application;
    using DataTable = System.Data.DataTable;

    /// <summary>
    ///     The Excel extensions class.
    /// </summary>
    public static class ExcelExtensions
    {
        /// <summary>
        ///     Export dataTable to Excel file.
        /// </summary>
        /// <param name="dataTable"> Source dataTable. </param>
        /// <param name="filePath"> Path to result file name. </param>
        [PublicAPI]
        public static void ExportToExcel(this DataTable dataTable, string filePath = null)
        {
            try
            {
                var columnsCount = ColumnsCount(dataTable);
                var excel = ExcelInstance();
                _Worksheet worksheet = excel.ActiveSheet;

                ConfigureHeader(worksheet, dataTable);
                CopyToWorksheet(dataTable, columnsCount, worksheet);
                SaveExcelFile(filePath, worksheet, excel);
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        /// <summary>
        ///     Get columns count from data table.
        /// </summary>
        /// <param name="dataTable"> The data table. </param>
        /// <returns> The columns count value. </returns>
        /// <exception cref="System.Exception"> Throw when input data table is empty. </exception>
        private static int ColumnsCount(DataTable dataTable)
        {
            var columnsCount = dataTable.Columns.Count;

            if (columnsCount == 0)
            {
                throw new Exception("ExportToExcel: Empty input table !!!");
            }

            return columnsCount;
        }

        /// <summary>
        ///     Configure header range in Excel file.
        /// </summary>
        /// <param name="worksheet"> The excel worksheet. </param>
        /// <param name="dataTable"> The data table. </param>
        private static void ConfigureHeader(_Worksheet worksheet, DataTable dataTable)
        {
            var columnsCount = dataTable.Columns.Count;
            var header = new object[columnsCount];

            for (var i = 0; i < columnsCount; i++)
            {
                header[i] = dataTable.Columns[i].ColumnName;
            }

            var headerRange = worksheet.Range[(Range)worksheet.Cells[1, 1], (Range)worksheet.Cells[1, columnsCount]];
            headerRange.Value = header;
            headerRange.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
            headerRange.Font.Bold = true;
        }

        /// <summary>
        ///     Copy data to worksheet.
        /// </summary>
        /// <param name="dataTable"> The data table to copy. </param>
        /// <param name="columnsCount"> The columns count. </param>
        /// <param name="worksheet"> The worksheet. </param>
        private static void CopyToWorksheet(DataTable dataTable, int columnsCount, _Worksheet worksheet)
        {
            var rowsCount = dataTable.Rows.Count;
            var cells = new object[rowsCount, columnsCount];

            for (var j = 0; j < rowsCount; j++)
            {
                for (var i = 0; i < columnsCount; i++)
                {
                    cells[j, i] = dataTable.Rows[j][i];
                }
            }

            worksheet.Range[(Range)worksheet.Cells[2, 1], (Range)worksheet.Cells[rowsCount + 1, columnsCount]].Value =
                cells;
        }

        /// <summary>
        ///     Get new excel instance.
        /// </summary>
        /// <returns> The new excel instance. </returns>
        private static Microsoft.Office.Interop.Excel.Application ExcelInstance()
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Workbooks.Add();
            return excel;
        }

        /// <summary>
        ///     Save the excel file in current path.
        /// </summary>
        /// <param name="filePath"> The path to save file. </param>
        /// <param name="worksheet"> The worksheet. </param>
        /// <param name="excel"> The excel application. </param>
        /// <exception cref="System.Exception"> Throw when SaveAs method will fail. </exception>
        private static void SaveExcelFile(string filePath, _Worksheet worksheet, _Application excel)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    worksheet.SaveAs(filePath);
                    excel.Quit();
                    MessageBox.Show(Resources.Extensions_SaveExcelFile_MessageBox_Text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath. \n" + ex.Message);
                }
            }
            else
            {
                excel.Visible = true;
            }
        }
    }
}