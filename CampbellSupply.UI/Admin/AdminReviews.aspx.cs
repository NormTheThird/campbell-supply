using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply.Admin
{
    public partial class AdminReviews : Page
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
                    this.rpReviews.DataSource = DataLayer.DataServices.ProductService.GetRatedAndReviewed(new GetRatedAndReviewedRequest()).RatedAndReviewed;
                    this.rpReviews.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
               LoggingService.LogError(new LogErrorRequest { Class = "AdminReviews/Page_Load", Ex = ex });
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
               LoggingService.LogError(new LogErrorRequest { Class = "AdminReviews/RadGrid1_ItemCreated", Ex = ex });
            }


        }

        /// <summary>
        /// Handles the radgrid ItemDataBound event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void rpReviews_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    // Set column properties here if need be
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminReviews/rpReviews_ItemDataBound", Ex = ex });
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
                if (e.Item is GridEditableItem)
                {
                    var ratedAndReviewed = this.ConvertGridItemToModel((GridEditableItem)e.Item);
                    if (ratedAndReviewed != null) DataLayer.DataServices.ProductService.SaveAdminRatedAndReviewed(new SaveRatedAndReviewedRequest { RatedAndReviewed = ratedAndReviewed });
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
               LoggingService.LogError(new LogErrorRequest { Class = "AdminReviews/rpReviews_UpdateCommand", Ex = ex });
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
                (sender as RadGrid).DataSource = DataLayer.DataServices.ProductService.GetRatedAndReviewed(new GetRatedAndReviewedRequest()).RatedAndReviewed;
            }
            catch (Exception ex)
            {
                // Write To Error Log
               LoggingService.LogError(new LogErrorRequest { Class = "AdminReviews/rpReviews_NeedDataSource", Ex = ex });
            }
        }

        /// <summary>
        /// UConverts the grid item to the AdminCouponModel
        /// </summary>
        /// <param name="editableItem">The gridEditableItem being updated or added</param>
        private AdminRatedReviewedModel ConvertGridItemToModel(GridEditableItem editableItem)
        {
            try
            {
                // Declare A New Coupon And Set Values
                var ratedAndReviewed = new AdminRatedReviewedModel();
                ratedAndReviewed.Id = Guid.Parse(((editableItem["Id"] as TableCell).Controls[0] as TextBox).Text);
                ratedAndReviewed.IsAuthorized = ((editableItem["IsAuthorized"] as TableCell).Controls[0] as CheckBox).Checked;
                ratedAndReviewed.IsActive = ((editableItem["IsActive"] as TableCell).Controls[0] as CheckBox).Checked;
                return ratedAndReviewed;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "AdminReviews/ConvertGridItemToModel", Ex = ex });
                return null;
            }
        }
    }
}