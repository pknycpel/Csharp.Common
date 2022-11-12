// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExcelFileImporter.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2019-10-01 10:09 </creationDate>
// <summary>
//      Defines the IExcelFileImporter interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FormsExtensions.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using JetBrains.Annotations;

    /// <summary>
    ///     The ExcelFileImporter interface.
    /// </summary>
    public interface IExcelFileImporter : IDisposable
    {
        /// <summary>
        ///     Gets sheets names list.
        /// </summary>
        [PublicAPI]
        List<string> SheetsNamesList { get; }

        /// <summary>
        ///     The sheet data table.
        /// </summary>
        /// <param name="sheetName"> The excel sheet name. </param>
        /// <returns> The <see cref="System.Data.DataTable" />. </returns>
        [PublicAPI]
        DataTable SheetDataTable(string sheetName);
    }
}