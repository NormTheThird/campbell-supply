using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;

namespace CampbellSupply.Admin
{
    public partial class AdminCoupons : Page
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
                if (!Page.IsPostBack)
                {
                    this.radGridCoupons.DataSource = OrderService.GetCoupons(new GetCouponsRequest()).Coupons;
                    this.radGridCoupons.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminCoupons/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the radgrid ItemCreated event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event args being sent by the object</param>
        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        GridEditableItem item = e.Item as GridEditableItem;
                        GridEditManager manager = item.EditManager;
                        GridTextBoxColumnEditor editor = manager.GetColumnEditor("Id") as GridTextBoxColumnEditor;
                        editor.TextBoxControl.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminCoupons/RadGrid1_ItemCreated", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the radgrid ItemDataBound event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event args being sent by the object</param>
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    // Set column width in edit mode
                    GridEditableItem item = (GridEditableItem)e.Item;
                    ((TextBox)item["Code"].Controls[0]).Width = 100;
                    ((TextBox)item["Description"].Controls[0]).Width = 320;
                    ((TextBox)item["Value"].Controls[0]).Width = 50;

                    if (e.Item.ItemIndex == -1)
                    {
                        // Insert
                        (item["Code"].Controls[0] as TextBox).Enabled = true;
                        (item["Value"].Controls[0] as TextBox).Enabled = true;
                        (item["IsPercent"].Controls[0] as CheckBox).Enabled = true;
                        (item["IsAmount"].Controls[0] as CheckBox).Enabled = true;
                    }
                    else
                    {
                        // Edit
                        (item["Code"].Controls[0] as TextBox).Enabled = false;
                        (item["Value"].Controls[0] as TextBox).Enabled = false;
                        (item["IsPercent"].Controls[0] as CheckBox).Enabled = false;
                        (item["IsAmount"].Controls[0] as CheckBox).Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminCoupons/RadGrid1_ItemDataBound", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the radgrid InsertCommand event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event args being sent by the object</param>
        protected void RadGrid1_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                // Check If Item Is Editable
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    var coupon = this.ConvertGridItemToModel((GridEditableItem)e.Item);
                    if (coupon != null) OrderService.SaveCoupon(new SaveCouponRequest { Coupon = coupon });
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminCoupons/RadGrid1_InsertCommand", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the update command sent from the rad grid
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void rpReviews_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                // Check If Item Is Editable
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    var coupon = this.ConvertGridItemToModel((GridEditableItem)e.Item);
                    if (coupon != null) OrderService.SaveCoupon(new SaveCouponRequest { Coupon = coupon });
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminCoupons/rpReviews_UpdateCommand", Ex = ex });
            }
        }

        /// <summary>
        /// Rebinds the data source for the rad grid
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void rpReviews_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                (sender as RadGrid).DataSource = OrderService.GetCoupons(new GetCouponsRequest()).Coupons;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminCoupons/rpReviews_NeedDataSource", Ex = ex });
            }
        }

        /// <summary>
        /// UConverts the grid item to the AdminCouponModel
        /// </summary>
        /// <param name="editableItem">The gridEditableItem being updated or added</param>
        private AdminCouponModel ConvertGridItemToModel(GridEditableItem editableItem)
        {
            try
            {
                // Declare A New Coupon And Set Values
                var coupon = new AdminCouponModel();
                coupon.Id = string.IsNullOrEmpty(((editableItem["Id"] as TableCell).Controls[0] as TextBox).Text) ? Guid.NewGuid() : Guid.Parse(((editableItem["Id"] as TableCell).Controls[0] as TextBox).Text);
                coupon.Code = ((editableItem["Code"] as TableCell).Controls[0] as TextBox).Text;
                coupon.Description = ((editableItem["Description"] as TableCell).Controls[0] as TextBox).Text;
                coupon.Value = Convert.ToDecimal(((editableItem["Value"] as TableCell).Controls[0] as TextBox).Text);
                coupon.IsAmount = ((editableItem["IsAmount"] as TableCell).Controls[0] as CheckBox).Checked;
                coupon.IsPercent = ((editableItem["IsPercent"] as TableCell).Controls[0] as CheckBox).Checked;
                coupon.IsOneTime = ((editableItem["IsOneTime"] as TableCell).Controls[0] as CheckBox).Checked;
                coupon.IsActive = ((editableItem["IsActive"] as TableCell).Controls[0] as CheckBox).Checked;
                return coupon;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminCoupons/ConvertGridItemToModel", Ex = ex });
                return null;
            }
        }
    }
}