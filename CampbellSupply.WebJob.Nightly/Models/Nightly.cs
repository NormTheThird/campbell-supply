using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CampbellSupply.WebJob.Nightly.Models
{
    public class Nightly
    {
        public string logMessage { get; set; } = "";
        public DropboxHelper DropboxHelper { get; set; }

        /// <summary>
        /// Creates the new nighlty files and puts them in dropbox
        /// </summary>
        public void CreateFiles()
        {
            try
            {
                // Declare new dropbox helper and delete old nightly files
                this.Log("Campbells Nightly File Started" + Environment.NewLine + Environment.NewLine);
                this.DropboxHelper = new DropboxHelper("/Websites/CampbellsWebsite/NightlyFile/");
                this.Log("Deleting old nightly file." + Environment.NewLine);
                this.DropboxHelper.DeleteAllFilesInFolder();
                  
                // Get data from the products table
                var nightlyTable = this.GetProductsData();
                //if (!this.CreateExcelFromDataTable(nightlyTable)) LoggingServices.LogError(new LogErrorRequest { Class = "Nightly/CreateFile", Ex = new ApplicationException("Excel file failed to create.") });
                if (!this.CreateDelimitedFileFromDataTable(nightlyTable, "|")) LoggingService.LogError(new LogErrorRequest { Class = "Nightly/CreateFile", Ex = new ApplicationException("Delimited file failed to create.") });
            }
            catch (Exception ex)
            {
                this.Log(ex.Message);
                LoggingService.LogError(new LogErrorRequest { Class = "Nightly/CreateFile", Ex = ex });
            }
        }

        /// <summary>
        /// Creates a new table with the proper columns for the nightly file
        /// </summary>
        /// <returns>An empty DataTable with columns for the nightly file</returns>
        private DataTable CreateNightlyDataTable()
        {
            try
            {
                this.Log("Create nightly data table");
                // Create table and populate columns
                DataTable dt = new DataTable();
                dt.Columns.Add("MFUPC");
                dt.Columns.Add("ShortDescription");
                dt.Columns.Add("PartNumber");
                dt.Columns.Add("Manufacturer");
                dt.Columns.Add("Unknown");
                dt.Columns.Add("Price");
                dt.Columns.Add("imgURL");
                dt.Columns.Add("Status");
                dt.Columns.Add("Visibility");
                dt.Columns.Add("WebPartNumber");
                dt.Columns.Add("SKU");
                dt.Columns.Add("ProductName");
                dt.Columns.Add("Section");
                dt.Columns.Add("Category");
                dt.Columns.Add("Subcatagory");
                dt.Columns.Add("Description");
                dt.Columns.Add("oPrice");
                dt.Columns.Add("ShipCost");
                dt.Columns.Add("ShipValid");
                dt.Columns.Add("Color");
                dt.Columns.Add("Size");
                dt.Columns.Add("Weight");
                dt.Columns.Add("DateOn");
                dt.Columns.Add("DateOff");
                dt.Columns.Add("Featured");
                dt.Columns.Add("Brand");
                dt.Columns.Add("Market");
                dt.Columns.Add("Group");
                dt.Columns.Add("Mirror");
                return dt;
            }
            catch (Exception ex)
            {
                this.Log(ex.Message.Trim() + Environment.NewLine);
                return new DataTable();
            }
        }

        /// <summary>
        /// Adds data to the DataTable
        /// </summary>
        /// <param name="_dataTable">The DataTable to populate</param>
        /// <returns>A DataTable with the populated data</returns>
        private DataTable GetProductsData()
        {
            try
            {
                DataTable newTable = this.CreateNightlyDataTable();
                this.Log("Getting product data." + Environment.NewLine);
                var response = ProductService.GetProducts(new GetProductsRequest());
                if (!response.IsSuccess) throw new ApplicationException(response.ErrorMessage);
                foreach (var product in response.Products)
                {
                    this.Log("Getting product info for [" + product.Name + "][" + product.Sku + "]" + Environment.NewLine);
                    DataRow dr = newTable.NewRow();
                    dr["MFUPC"] = product.ManufacturerUPC;
                    dr["ShortDescription"] = product.DescriptionShort;
                    dr["PartNumber"] = product.PartNumber;
                    dr["Manufacturer"] = string.IsNullOrEmpty(product.ManufacturerName) ? "" : product.ManufacturerName;
                    dr["Unknown"] = product.Unkonwn;
                    dr["Price"] = product.PurchasePrice;
                    dr["imgURL"] = product.ImageUrl;
                    dr["Status"] = product.Status;

                    if (product.IsActive && !product.IsDeleted) dr["Visibility"] = "ON";
                    else dr["Visibility"] = "OFF";

                    dr["WebPartNumber"] = product.WebPartNumber;
                    dr["SKU"] = product.Sku;
                    dr["ProductName"] = product.Name;
                    dr["Section"] = string.IsNullOrEmpty(product.DepartmentName) ? "" : product.DepartmentName;
                    dr["Category"] = string.IsNullOrEmpty(product.CategoryName) ? "" : product.CategoryName;
                    dr["Subcatagory"] = string.IsNullOrEmpty(product.SubCategoryName) ? "" : product.SubCategoryName;
                    dr["Description"] = product.DescriptionLong;
                    dr["oPrice"] = product.OverridePrice.ToString();
                    dr["ShipCost"] = product.ShippingPrice.ToString();
                    dr["ShipValid"] = product.IsShippingValid ? "TRUE" : "FALSE";
                    dr["Color"] = product.Color == null ? "-1" : product.Color;
                    dr["Size"] = product.Size == null ? "-1" : product.Size;
                    dr["Weight"] = product.Weight.ToString();
                    dr["DateOn"] = product.DateOn.ToString();
                    dr["DateOff"] = product.DateOff.ToString();
                    dr["Featured"] = product.IsFeatured ? "TRUE" : "FALSE";
                    dr["Brand"] = product.Brand;
                    dr["Market"] = product.Market;
                    dr["Group"] = product.Group;
                    dr["Mirror"] = product.Mirror;
                    newTable.Rows.Add(dr);
                }
                return newTable;
            }
            catch (Exception ex)
            {
                this.Log(ex.Message.Trim() + Environment.NewLine);
                return new DataTable();
            }
        }

        /// <summary>
        /// Creates an Excel file from a DataTable
        /// </summary>
        /// <param name="_dataTable">The DataTable to create the excel file from</param>
        /// <param name="_filePath">The path to save the file to</param>
        /// <returns>A bool value if the file creation was successful</returns>
        private bool CreateExcelFromDataTable(DataTable _dataTable)
        {
            try
            {
                // Open Excel file to read column headers
                DateTime _beginTime = DateTime.Now;
                this.Log("File Start Time: " + _beginTime.ToLongTimeString() + Environment.NewLine);
                var application = new Microsoft.Office.Interop.Excel.Application();
                var workbook = application.Workbooks.Add(Type.Missing);
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;

                // Write column names to worksheet
                for (int i = 1; i <= _dataTable.Columns.Count; i++)
                    worksheet.Cells[1, i] = _dataTable.Columns[i - 1].ColumnName;

                // Write data to worksheet
                for (int i = 2; i <= _dataTable.Rows.Count + 1; i++)
                {
                    worksheet.Cells[i, 1] = _dataTable.Rows[i - 2][0];
                    worksheet.Cells[i, 2] = _dataTable.Rows[i - 2][1];
                    worksheet.Cells[i, 3] = _dataTable.Rows[i - 2][2];
                    worksheet.Cells[i, 4] = _dataTable.Rows[i - 2][3];
                    worksheet.Cells[i, 5] = _dataTable.Rows[i - 2][4];
                    worksheet.Cells[i, 6] = _dataTable.Rows[i - 2][5];
                    worksheet.Cells[i, 7] = _dataTable.Rows[i - 2][6];
                    worksheet.Cells[i, 8] = _dataTable.Rows[i - 2][7];
                    worksheet.Cells[i, 9] = _dataTable.Rows[i - 2][8];
                    worksheet.Cells[i, 10] = _dataTable.Rows[i - 2][9];
                    worksheet.Cells[i, 11] = _dataTable.Rows[i - 2][10];
                    worksheet.Cells[i, 12] = _dataTable.Rows[i - 2][11];
                    worksheet.Cells[i, 13] = _dataTable.Rows[i - 2][12];
                    worksheet.Cells[i, 14] = _dataTable.Rows[i - 2][13];
                    worksheet.Cells[i, 15] = _dataTable.Rows[i - 2][14];
                    worksheet.Cells[i, 16] = _dataTable.Rows[i - 2][15];
                    worksheet.Cells[i, 17] = _dataTable.Rows[i - 2][16];
                    worksheet.Cells[i, 18] = _dataTable.Rows[i - 2][17];
                    worksheet.Cells[i, 19] = _dataTable.Rows[i - 2][18];
                    worksheet.Cells[i, 20] = _dataTable.Rows[i - 2][19];
                    worksheet.Cells[i, 21] = _dataTable.Rows[i - 2][20];
                    worksheet.Cells[i, 22] = _dataTable.Rows[i - 2][21];
                    worksheet.Cells[i, 23] = _dataTable.Rows[i - 2][22];
                    worksheet.Cells[i, 24] = _dataTable.Rows[i - 2][23];
                    worksheet.Cells[i, 25] = _dataTable.Rows[i - 2][24];
                    worksheet.Cells[i, 26] = _dataTable.Rows[i - 2][25];
                    worksheet.Cells[i, 27] = _dataTable.Rows[i - 2][26];
                    worksheet.Cells[i, 28] = _dataTable.Rows[i - 2][27];
                    worksheet.Cells[i, 29] = _dataTable.Rows[i - 2][28];
                }

                // Write End Time And File Count
                this.Log("Process Complete Nightly File Created" + Environment.NewLine);
                this.Log("File End Time: " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
                this.Log("Time To Process File: " + DateTime.Now.Subtract(_beginTime).TotalMinutes.ToString() + " Minutes");

                // Save worksheet and close Excel
                workbook.Saved = true;
                this.DropboxHelper.UploadFile("NightlyFile_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx", this.GetActiveWorkbook(application));
                workbook.Close();
                application.Quit();
                Marshal.ReleaseComObject(application);
                GC.Collect();

                // Return success
                return true;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Nightly/CreateExcelFromDataTable", Ex = ex });
                return false;
            }
        }

        /// <summary>
        /// Creates a delimited file from a DataTable
        /// </summary>
        /// <param name="_dataTable">The DataTable to create the delimited file from</param>
        /// <param name="_filePath">The path to save the file to</param>
        /// <param name="_delimitedValue">The string value used to seperate the file</param>
        /// <returns>A bool value if the file creation was successful</returns>
        private bool CreateDelimitedFileFromDataTable(DataTable _dataTable, string _delimitedValue)
        {
            try
            {
                // Loop through each column header
                StringBuilder stringBuilder = new StringBuilder();
                this.Log("Creating delimited file header.");
                foreach (DataColumn headerColumn in _dataTable.Columns)
                {
                    stringBuilder.Append(headerColumn.ColumnName.ToString() + _delimitedValue.Trim());
                    if (headerColumn.Ordinal == _dataTable.Columns.Count - 1)
                        stringBuilder.Append("~|");
                }

                // Loop through each datarow
                this.Log("Creating delimited file rows.");
                foreach (DataRow dataRow in _dataTable.Rows)
                {
                    // Loop through each column in datarow
                    foreach (DataColumn dataColumn in _dataTable.Columns)
                    {
                        stringBuilder.Append(dataRow[dataColumn].ToString() + _delimitedValue.Trim());
                        if (dataColumn.Ordinal == _dataTable.Columns.Count - 1)
                            stringBuilder.Append("~|");
                    }
                }

                // Save the file to dropbox
                this.DropboxHelper.UploadFile("NightlyFile_" + DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime).ToString("yyyyMMdd") + ".txt", Encoding.UTF8.GetBytes(stringBuilder.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Nightly/CreateDelimitedFileFromDataTable", Ex = ex });
                return false;
            }
        }

        /// <summary>
        /// Returns the active workbook as a byte[]
        /// </summary>
        /// <param name="app">The current excel application</param>
        /// <returns>byte[]</returns>
        private byte[] GetActiveWorkbook(Microsoft.Office.Interop.Excel.Application app)
        {
            string path = Path.GetTempFileName();
            try
            {
                app.ActiveWorkbook.SaveCopyAs(path);
                return File.ReadAllBytes(path);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        private void Log(string value)
        {
            Console.WriteLine(value);
            this.logMessage += value;
        }
    }
}