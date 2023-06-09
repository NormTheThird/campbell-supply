using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.Models;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace CampbellSupply.WebJob.Upload.Models
{
    public class Upload
    {
        public string logMessage { get; set; }
        public DropboxHelper DropboxHelper { get; set; }  

        public void UploadFiles()
        {
            try
            {
                // Declare new dropbox helper and check if any files exists
                this.Log("Campbells Upload File Started" + Environment.NewLine + Environment.NewLine);
                this.Log("Getting all files." + Environment.NewLine);

                var nl = Environment.NewLine;
                this.DropboxHelper = new DropboxHelper("/Websites/CampbellsWebsite/UpdateFile/");
                var files = this.DropboxHelper.GetAllFilesInFolder();
                if (files.Count == 0)
                {
                    this.Log("No files found. Ending" + Environment.NewLine);
                    return;
                }
                this.Log(files.Count.ToString() + " files found." + Environment.NewLine);

                // Upload files
                foreach (var file in files)
                {
                    DateTime _beginTime = DateTime.Now;
                    this.Log("Processing File: " + file.Item1.Trim() + nl);
                    this.Log("File Start Time: " + _beginTime.ToLongTimeString() + nl);
                    var dataTable = this.CreateFileTable();

                    StringBuilder sb = new StringBuilder();
                    List<string> list = new List<string>();
                    using (StreamReader sr = new StreamReader(new MemoryStream(file.Item2), Encoding.UTF8))
                        while (sr.Peek() >= 0)
                            list.Add(sr.ReadLine());

                    //ignore the fist line (title line).
                    this.Log(list.Count.ToString() + " records that need to be changed or updated.");
                    var numOfRecords = list.Count - 1;
                    for (int i = 1; i < list.Count; i++)
                    {
                        string[] strlist = list[i].Split('|');
                        dataTable.Rows.Add(strlist[0], strlist[1], strlist[2], strlist[3], strlist[4], strlist[5],
                                           strlist[6], strlist[7], strlist[8], strlist[9], strlist[10], strlist[11],
                                           strlist[12], strlist[13], strlist[14], strlist[15], strlist[16], strlist[17],
                                           strlist[18], strlist[19], strlist[20], strlist[21], strlist[22], strlist[23],
                                           strlist[24], strlist[25], strlist[26], strlist[27], strlist[28]);
                    }

                    foreach (DataRow row in dataTable.Rows)
                        UpdateProductTxt(row);

                    this.Log("File End Time: " + DateTime.Now.ToLongTimeString() + nl);
                    this.Log("Process Complete: " + numOfRecords.ToString().Trim() + " Records Processed" + nl + nl + nl);
                    this.DropboxHelper.MoveFileToNewFolder(file.Item1, "ProcessedFiles/");
                }

                var request = new SendEmailRequest
                {
                    From = "upload@CampbellSupply.net",
                    Subject = "A new file has been uploaded",
                    Body = this.logMessage
                };

                var adEmail = ConfigurationManager.AppSettings["AdvertisingToEmail"];
                request.Recipients = new List<string> { adEmail };
                MessagingService.SendEmail(request);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "Upload/UploadFiles", Ex = ex });
            }
        }

        /// <summary>
        /// Creates a data table for the given structure
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable CreateFileTable()
        {
            try
            {
                //According to the first line in the text file, create a dataTable
                DataTable data = new DataTable();
                data.Columns.AddRange(new DataColumn[29] {
                    new DataColumn("MFUPC"),
                    new DataColumn("ShortDescription"),
                    new DataColumn("PartNumber"),
                    new DataColumn("Manufacturer"),
                    new DataColumn("Unknown"),
                    new DataColumn("Price"),
                    new DataColumn("imgURL"),
                    new DataColumn("Status"),
                    new DataColumn("Visibility"),
                    new DataColumn("WebPartNumber"),
                    new DataColumn("SKU"),
                    new DataColumn("ProductName"),
                    new DataColumn("Section"),
                    new DataColumn("Category"),
                    new DataColumn("Subcategory"),
                    new DataColumn("Description"),
                    new DataColumn("oPrice"),
                    new DataColumn("ShipCost"),
                    new DataColumn("ShipValid"),
                    new DataColumn("Color"),
                    new DataColumn("Size"),
                    new DataColumn("Weight"),
                    new DataColumn("DateOn"),
                    new DataColumn("DateOff"),
                    new DataColumn("Featured"),
                    new DataColumn("Brand"),
                    new DataColumn("Market"),
                    new DataColumn("Group"),
                    new DataColumn("Mirror")
                });

                data.Columns[4].DefaultValue = "0";
                data.Columns["Price"].DefaultValue = "0";
                data.Columns["Visibility"].DefaultValue = "-1";
                data.Columns["oPrice"].DefaultValue = "0";
                data.Columns["ShipCost"].DefaultValue = "0";
                data.Columns["ShipValid"].DefaultValue = "false";
                data.Columns["Color"].DefaultValue = "-1";
                data.Columns["Size"].DefaultValue = "-1";
                data.Columns["Weight"].DefaultValue = "0";
                data.Columns["DateOn"].DefaultValue = "1/1/1753 12:00:00 AM";
                data.Columns["DateOff"].DefaultValue = "1/1/1753 12:00:00 AM";
                data.Columns["Featured"].DefaultValue = "false";
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the product and sends it to the table
        /// </summary>
        /// <param name="row">The row in the DataTable to update</param>
        public void UpdateProductTxt(DataRow dr)
        {
            try
            {
                // Populate new product
                var product = new UploadModel();
                product.ManufacturerUPC = Convert.ToString(dr[0] ?? "");
                product.DescriptionShort = Convert.ToString(dr[1] ?? "");
                product.PartNumber = Convert.ToString(dr[2] ?? "");
                product.ManufacturerName = Convert.ToString(dr[3] ?? "");
                product.Unknown = Convert.ToString(dr[4] ?? "");
                product.PurchasePrice = Convert.ToString(dr[5] ?? "");
                product.ImageUrl = Convert.ToString(dr[6] ?? "");
                product.Status = Convert.ToString(dr[7] ?? "");
                product.VisibilityId = Convert.ToString(dr[8] ?? "");
                product.WebPartNumber = Convert.ToString(dr[9] ?? "");
                product.Sku = Convert.ToString(dr[10] ?? "");
                product.Name = Convert.ToString(dr[11] ?? "");
                product.DepartmentName = Convert.ToString(dr[12] ?? "");
                product.CategoryName = Convert.ToString(dr[13] ?? "");
                product.SubCategoryName = Convert.ToString(dr[14] ?? "");
                product.DescriptionLong = Convert.ToString(dr[15] ?? "");
                product.OverridePrice = Convert.ToString(dr[16] ?? "");
                product.ShippingPrice = Convert.ToString(dr[17] ?? "");
                product.IsShippingValid = Convert.ToString(dr[18] ?? "");
                product.Color = Convert.ToString(dr[19] ?? "");
                product.Size = Convert.ToString(dr[20] ?? "");
                product.Weight = Convert.ToString(dr[21] ?? "");
                product.DateOn = Convert.ToString(dr[22] ?? "");
                product.DateOff = Convert.ToString(dr[23] ?? "");
                product.IsFeatured = Convert.ToString(dr[24] ?? "");
                product.Brand = Convert.ToString(dr[25] ?? "");
                product.Market = Convert.ToString(dr[26] ?? "");
                product.Group = Convert.ToString(dr[27] ?? "");
                product.Mirror = Convert.ToString(dr[28] ?? "");

                var response = ProductService.UpdateProduct(new UpdateProductRequest { Product = product });
                if (!response.IsSuccess) throw new ApplicationException(response.ErrorMessage);
                this.Log("Processed Manufacturer UPC: " + product.ManufacturerUPC + Environment.NewLine);
            }
            catch (Exception ex)
            {
                this.Log("Error message for Manufacturer UPC: " +  Convert.ToString(dr[0]) + Environment.NewLine);
                this.Log("Error message :" + ex.Message + Environment.NewLine);
                LoggingService.LogError(new LogErrorRequest { Class = "Upload/UpdateProductTxt", Ex = ex });
            }
        }

        private void Log(string value)
        {
            Console.WriteLine(value);
            this.logMessage += value;
        }
    }
}