using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampbellSupply.Models;
using Telerik.Web.UI;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;

namespace CampbellSupply.Admin
{
    public partial class AdminProducts : Page
    {
        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event args being sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Set Search Placeholder
                    this.txtSearch.Attributes.Add("placeholder", "Search By Part Number Or Upc");

                    // Get List Of Departments
                    this.ddlDepartment.DataSource = MenuService.GetDepartments(new GetDepartmentsRequest()).Departments;
                    this.ddlDepartment.DataTextField = "Name";
                    this.ddlDepartment.DataValueField = "Name";
                    this.ddlDepartment.DataBind();

                    this.GetCategories();
                    this.GetRadGridProducts(false);
                }
                else
                {
                    // Get What Control Caused Postback And Get Categories
                    Control ctrl = this.GetControlThatCausedPostBack();
                    if (ctrl.ID != null)
                        if (ctrl.ID.Equals("ddlDepartment"))
                        {
                            this.GetCategories();
                            this.GetRadGridProducts(false);
                        }
                        else if (ctrl.ID.Equals("ddlCategory"))
                            this.GetRadGridProducts(false);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the radgrid ItemDataBound event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void radGridProducts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem item = e.Item as GridEditableItem;

                    // Populate The Visibility Drop Down
                    //this.listVisibility = item.FindControl("lstVisibility") as DropDownList;
                    //this.listVisibility.DataSource = (List<clsVisibility>)Session["Visibility"];
                    //this.listVisibility.DataTextField = "value";
                    //this.listVisibility.DataValueField = "id";
                    //this.listVisibility.DataBind();
                    //this.listVisibility.SelectedValue = ((item as GridDataItem).DataItem as DataRowView).Row["VisibilityID"].ToString().Trim();

                    // Populate The Department Drop Down
                    //this.listDepartment = item.FindControl("lstDepartment") as DropDownList;
                    //this.listDepartment.DataSource = new clsDepartments().SelectAll();
                    //this.listDepartment.DataTextField = "name";
                    //this.listDepartment.DataValueField = "id";
                    //this.listDepartment.DataBind();
                    //this.listDepartment.SelectedValue = ((item as GridDataItem).DataItem as DataRowView).Row["departmentID"].ToString().Trim();

                    // Populate The Category Drop Down
                    //this.listCategory = item.FindControl("lstCategory") as DropDownList;
                    ////this.listCategory.DataSource = new clsDepartmentMenu().SelectDepartmentCategories(Convert.ToInt32(this.listDepartment.SelectedValue));
                    //this.listCategory.DataSource = MenuServices.GetCategories(new GetCategoriesRequest { Department = this.listDepartment.SelectedValue.Trim() }).Categories;
                    //this.listCategory.DataTextField = "name";
                    //this.listCategory.DataValueField = "id";
                    //this.listCategory.DataBind();
                    //this.listCategory.SelectedValue = ((item as GridDataItem).DataItem as DataRowView).Row["categoryID"].ToString().Trim();

                    // Popluate The Manufacrurer Drop Down
                    //this.listManufacturer = item.FindControl("lstManufacturer") as DropDownList;
                    //this.listManufacturer.DataSource = (List<ManufacturerModel>)Session["Manufacturer"];
                    //this.listManufacturer.DataTextField = "Name";
                    //this.listManufacturer.DataValueField = "Id";
                    //this.listManufacturer.DataBind();
                    //this.listManufacturer.SelectedValue = ((item as GridDataItem).DataItem as DataRowView).Row["manufactureID"].ToString().Trim();

                    // Set column width in edit mode
                    GridEditableItem edititem = (GridEditableItem)e.Item;
                    ((TextBox)edititem["ManufacturerUPC"].Controls[0]).Width = 105;
                    ((TextBox)edititem["Name"].Controls[0]).Width = 185;
                    ((TextBox)edititem["DescriptionShort"].Controls[0]).Width = 185;
                    ((TextBox)edititem["PartNumber"].Controls[0]).Width = 85;
                    ((TextBox)edititem["WebPartNumber"].Controls[0]).Width = 85;
                    ((TextBox)edititem["Sku"].Controls[0]).Width = 105;
                    ((RadNumericTextBox)edititem["PurchasePrice"].Controls[0]).Width = 45;
                    ((RadNumericTextBox)edititem["ShippingPrice"].Controls[0]).Width = 45;
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/radGridProducts_ItemDataBound", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the edit command sent from the rad grid
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void radGridProducts_EditCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                // Set Edit Properties
                this.radGridProducts.MasterTableView.GetColumn("csMultipleRow").Visible = true;
                this.radGridProducts.ClientSettings.Scrolling.FrozenColumnsCount = 4;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/radGridProducts_EditCommand", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the update command sent from the rad grid
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void radGridProducts_UpdateCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                // Check If Item Is Editable
                if (e.Item is GridEditableItem)
                {
                    GridEditableItem item = ((GridEditableItem)e.Item);
                    var product = this.ConvertGridItemToModel(item);
                    DataLayer.DataServices.ProductService.SaveAdminProduct(new SaveAdminProductRequest { Product = product });
                    //product.departmentID = Convert.ToInt32((editableItem.FindControl("lstDepartment") as DropDownList).SelectedValue);
                    //product.categoryID = Convert.ToInt32((editableItem.FindControl("lstCategory") as DropDownList).SelectedValue);

                    // Check If IsShippingValid Changed
                    //bool isShippingValidChanged = false;
                    //if (product.isShippingValid != Convert.ToBoolean((e.Item as GridDataItem).SavedOldValues["isShippingValid"]))
                    //    isShippingValidChanged = true;


                    // Loop Through All Checked Rows
                    foreach (GridDataItem gridItem in radGridProducts.MasterTableView.Items)
                    {
                        if (((gridItem["csMultipleRow"] as TableCell).Controls[0] as CheckBox).Checked)
                        {

                        }
                    }

                    // Set Edit Properties
                    this.radGridProducts.MasterTableView.GetColumn("csMultipleRow").Visible = false;
                    this.radGridProducts.ClientSettings.Scrolling.FrozenColumnsCount = 3;
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/radGridProducts_UpdateCommand", Ex = ex });
            }
        }

        /// <summary>
        /// Handes the cancel command sent from the rad grid
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void radGridProducts_CancelCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                // Set Edit Properties
                this.radGridProducts.MasterTableView.GetColumn("csMultipleRow").Visible = false;
                this.radGridProducts.ClientSettings.Scrolling.FrozenColumnsCount = 3;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/radGridProducts_CancelCommand", Ex = ex });
            }
        }

        /// <summary>
        /// Rebinds the data source for the rad grid
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void radGridProducts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtSearch.Text))
                {
                    var request = new SearchAdminProductsRequest { SearchValue = this.txtSearch.Text, IncludeInactive = false, IncludeDeleted = false };
                    (sender as RadGrid).DataSource = DataLayer.DataServices.ProductService.SerachAdminProducts(request).Products;
                }
                else
                {
                    var request = new GetAdminProductsRequest
                    {
                        Department = this.ddlDepartment.SelectedValue.Trim(),
                        Category = this.ddlCategory.SelectedValue.Trim(),
                        IncludeInactive = false,
                        IncludeDeleted = false
                    };
                    (sender as RadGrid).DataSource = DataLayer.DataServices.ProductService.GetAdminProducts(request).Products;
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/radGridProducts_NeedDataSource", Ex = ex });
            }
        }

        /// <summary>
        /// Handes the department selection change when in edit mode
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void lstDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //// Get List Of Categories For Selected Department
                //GridDataItem item = (GridDataItem)(sender as DropDownList).NamingContainer;
                //this.listCategory = (DropDownList)item.FindControl("lstCategory");
                ////this.listCategory.DataSource = new clsDepartmentMenu().SelectDepartmentCategories(Convert.ToInt32((sender as DropDownList).SelectedValue));
                //this.listCategory.DataSource = MenuServices.GetCategories(new GetCategoriesRequest { Department = (sender as DropDownList).SelectedValue.Trim() }).Categories;
                //this.listCategory.DataTextField = "name";
                //this.listCategory.DataValueField = "id";
                //this.listCategory.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Handles the search button click to search by part number or upc
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtSearch.Text)) this.GetRadGridProducts(true);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/btnClear_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the clear button and clears the search text
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtSearch.Text = "";
                this.radGridProducts.DataSource = "";
                this.radGridProducts.DataBind();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/btnClear_Click", Ex = ex });
            }
        }

        #region Private Methods

        /// <summary>
        /// Retrieves the control that caused the postback.
        /// </summary>
        /// <returns>The control that caused the postback</returns>
        private Control GetControlThatCausedPostBack()
        {
            try
            {
                // Get the event target name and return the control
                string ctrlName = Page.Request.Params.Get("__EVENTTARGET");
                if (!String.IsNullOrEmpty(ctrlName))
                    return (Control)Page.FindControl(ctrlName);

                // No control found 
                throw new SystemException("No Postback Contol Found");
            }
            catch (Exception)
            {
                return new Control();
            }
        }

        /// <summary>
        /// Get list of categories for a selected department
        /// </summary>
        private void GetCategories()
        {
            try
            {
                // Get List Of Categories For Selected Department
                this.ddlCategory.DataSource = MenuService.GetCategories(new GetCategoriesRequest { Department = this.ddlDepartment.SelectedValue.Trim() }).Categories;
                this.ddlCategory.DataTextField = "Name";
                this.ddlCategory.DataValueField = "Name";
                this.ddlCategory.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Gets products for the rad grid
        /// </summary>
        private void GetRadGridProducts(bool isSearch)
        {
            try
            {
                if (isSearch)
                {
                    var request = new SearchAdminProductsRequest { SearchValue = this.txtSearch.Text, IncludeInactive = false, IncludeDeleted = false };
                    this.radGridProducts.DataSource = DataLayer.DataServices.ProductService.SerachAdminProducts(request).Products;
                }
                else
                {
                    var request = new GetAdminProductsRequest
                    {
                        Department = this.ddlDepartment.SelectedValue.Trim(),
                        Category = this.ddlCategory.SelectedValue.Trim(),
                        IncludeInactive = false,
                        IncludeDeleted = false
                    };
                    this.radGridProducts.DataSource = DataLayer.DataServices.ProductService.GetAdminProducts(request).Products;
                }
                this.radGridProducts.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// UConverts the grid item to the AdminCouponModel
        /// </summary>
        /// <param name="editableItem">The gridEditableItem being updated or added</param>
        private ProductModel ConvertGridItemToModel(GridEditableItem editableItem)
        {
            try
            {
                // Declare A New Product And Set Values
                var product = new ProductModel();
                product.Id = Guid.Parse(((editableItem["Id"] as TableCell).Controls[0] as TextBox).Text);
                product.ManufacturerUPC = ((editableItem["ManufacturerUPC"] as TableCell).Controls[0] as TextBox).Text;
                product.Name = ((editableItem["Name"] as TableCell).Controls[0] as TextBox).Text;
                product.DescriptionShort = ((editableItem["DescriptionShort"] as TableCell).Controls[0] as TextBox).Text;
                product.PartNumber = ((editableItem["PartNumber"] as TableCell).Controls[0] as TextBox).Text;
                product.WebPartNumber = ((editableItem["WebPartNumber"] as TableCell).Controls[0] as TextBox).Text;
                product.Sku = ((editableItem["Sku"] as TableCell).Controls[0] as TextBox).Text;
                product.PurchasePrice = Convert.ToDecimal(((editableItem["purchasePrice"] as TableCell).Controls[0] as RadNumericTextBox).Text);
                product.OverridePrice = Convert.ToDecimal(((editableItem["OverridePrice"] as TableCell).Controls[0] as RadNumericTextBox).Text);
                product.SalePrice = Convert.ToDecimal(((editableItem["SalePrice"] as TableCell).Controls[0] as RadNumericTextBox).Text);
                product.ShippingPrice = Convert.ToDecimal(((editableItem["ShippingPrice"] as TableCell).Controls[0] as RadNumericTextBox).Text);
                product.IsFeatured = ((editableItem["IsFeatured"] as TableCell).Controls[0] as CheckBox).Checked;
                product.IsOnSale = ((editableItem["IsOnSale"] as TableCell).Controls[0] as CheckBox).Checked;
                product.IsShippingValid = ((editableItem["IsShippingValid"] as TableCell).Controls[0] as CheckBox).Checked;
                product.IsActive = ((editableItem["IsActive"] as TableCell).Controls[0] as CheckBox).Checked;
                product.IsDeleted = ((editableItem["IsDeleted"] as TableCell).Controls[0] as CheckBox).Checked;
                product.Unkonwn = ((editableItem["Unkonwn"] as TableCell).Controls[0] as TextBox).Text;
                product.Color = ((editableItem["Color"] as TableCell).Controls[0] as TextBox).Text;
                product.Size = ((editableItem["Size"] as TableCell).Controls[0] as TextBox).Text;
                product.Brand = ((editableItem["Brand"] as TableCell).Controls[0] as TextBox).Text;
                product.ImageUrl = ((editableItem["ImageUrl"] as TableCell).Controls[0] as TextBox).Text;
                product.Status = ((editableItem["Status"] as TableCell).Controls[0] as TextBox).Text;
                product.Market = ((editableItem["Market"] as TableCell).Controls[0] as TextBox).Text;
                product.Group = ((editableItem["Group"] as TableCell).Controls[0] as TextBox).Text;
                product.Mirror = ((editableItem["Mirror"] as TableCell).Controls[0] as TextBox).Text;
                product.Weight = Convert.ToDecimal(((editableItem["Weight"] as TableCell).Controls[0] as RadNumericTextBox).Text);
                //product.DateOn = DateTime.Parse(((editableItem["DateOn"] as TableCell).Controls[0] as TextBox).Text);
                //product.DateOff = DateTime.Parse(((editableItem["DateOff"] as TableCell).Controls[0] as TextBox).Text);
                return product;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminProducts/ConvertGridItemToModel", Ex = ex });
                return null;
            }
        }

        #endregion
    }
}