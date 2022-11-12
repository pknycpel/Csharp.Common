// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelFileImporter.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2019-10-01 10:09 </creationDate>
// <summary>
//      Defines the ExcelFileImporter type to import data from excel files to application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FormsExtensions.Excel
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using ExcelDataReader;
    using JetBrains.Annotations;

    /// <summary>
    ///     The Excel File Importer.
    /// </summary>
    [PublicAPI]
    public class ExcelFileImporter : IExcelFileImporter
    {
        /// <summary>
        ///     The file name.
        /// </summary>
        private readonly string filePath;

        /// <summary>
        ///     The Excel result.
        /// </summary>
        private DataSet xlsResult;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExcelFileImporter" /> class.
        /// </summary>
        /// <param name="filePath"> The imported file path. </param>
        public ExcelFileImporter(string filePath)
        {
            this.filePath = filePath;
            this.ImportFromFile();
        }

        /// <summary>
        ///     Gets the sheets names list.
        /// </summary>
        public List<string> SheetsNamesList =>
            this.xlsResult.Tables.Cast<DataTable>().Where(p => p.TableName != "Definicje").Select(p => p.TableName)
                .ToList();

        /// <inheritdoc />
        public void Dispose()
        {
            this.xlsResult?.Dispose();
        }

        /// <summary>
        ///     The sheet data table.
        /// </summary>
        /// <param name="sheetName"> The excel sheet name. </param>
        /// <returns> The <see cref="System.Data.DataTable" />. </returns>
        public DataTable SheetDataTable(string sheetName) =>
            this.xlsResult.Tables.Cast<DataTable>().FirstOrDefault(p => p.TableName == sheetName);

        /// <summary>
        ///     The import data from excel file.
        /// </summary>
        private void ImportFromFile()
        {
            try
            {
                var stream = File.Open(this.filePath, FileMode.Open, FileAccess.Read);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var readerConfiguration = new ExcelDataSetConfiguration
                    {
                        UseColumnDataType = false,
                        ConfigureDataTable = _ =>
                            new ExcelDataTableConfiguration
                            {
                                EmptyColumnNamePrefix = "Column", UseHeaderRow = true,
                            },
                    };

                    this.xlsResult = reader.AsDataSet(readerConfiguration);
                    reader.Close();
                }

                stream.Close();
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}