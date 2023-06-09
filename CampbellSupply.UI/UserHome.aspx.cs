using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply
{
    public partial class UserHome : Page
    {
        int pageCount = 1;

        /// <summary>
        /// The page load event.
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    // Set Search Place Holder
                    txtOrderSearch.Attributes.Add("placeholder", "Order Search");

                    //Populate the order history page
                    this.GetOrdersPerPage(1);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "UserHome/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the repeater button click event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_Changed(object sender, EventArgs e)
        {
            try
            {
                // Set The New Current Page
                int currentPage = int.Parse((sender as LinkButton).CommandArgument);
                if (currentPage > 0)
                    this.GetOrdersPerPage(currentPage);
                else
                {
                    // Get Current Middle Button And Set New Values
                    LinkButton linkButton = (LinkButton)rptPaging.Items[1].FindControl("btnPage");
                    if (currentPage.Equals(-1))
                        this.GetOrdersPerPage(Convert.ToInt32(linkButton.Text.Trim()) - 1);
                    else if (currentPage.Equals(-2))
                        this.GetOrdersPerPage(Convert.ToInt32(linkButton.Text.Trim()) + 1);
                    else
                        this.GetOrdersPerPage(1);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "UserHome/Page_Changed", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the search button click
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if order search box is populated
                if (string.IsNullOrEmpty(this.txtOrderSearch.Text.Trim()))
                    return;

                // Get Order History
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                var user = (AccountModel)Session["User"];
                var response = OrderService.SearchOrderHistoryDetail(new SearchOrderHistoryDetailRequest { OrderNumber = this.txtOrderSearch.Text.Trim(), AccountId = user.Id });
                if (response.IsSuccess)
                {
                    this.MapOrderHistoryDetails(response.Order);
                    this.divOrderHistorySearch.Visible = false;
                    this.divOrderHistory.Visible = false;
                    this.divOrderHistoryError.Visible = false;
                    this.divOrderDetails.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "UserHome/btnSearch_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the repeater button click event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void btnDetails_Click(object sender, EventArgs e)
        {
            try
            {
                // Get Order History
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                var user = (AccountModel)Session["User"];
                var orderId = Guid.Parse((sender as Button).CommandArgument);
                var response = OrderService.GetOrderHistroyDetail(new GetOrderHistoryDetailRequest { OrderId = orderId, AccountId = user.Id });
                if (response.IsSuccess)
                {
                    this.MapOrderHistoryDetails(response.Order);
                    this.divOrderHistorySearch.Visible = false;
                    this.divOrderHistory.Visible = false;
                    this.divOrderHistoryError.Visible = false;
                    this.divOrderDetails.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "UserHome/btnDetails_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the hide details button click
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void btnHideDetails_Click(object sender, EventArgs e)
        {
            try
            {
                this.divOrderHistorySearch.Visible = true;
                this.divOrderHistory.Visible = true;
                this.divOrderHistoryError.Visible = false;
                this.divOrderDetails.Visible = false;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "UserHome/btnHideDetails_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Maps the order history details
        /// </summary>
        /// <param name="order">The order to map to the details</param>
        private void MapOrderHistoryDetails(FullOrderModel order)
        {
            try
            {
                this.lblFname.Text = order.ShippingAddress.FirstName.Trim();
                this.lblLname.Text = order.ShippingAddress.LastName.Trim();
                this.lblAddress.Text = order.ShippingAddress.Address1.Trim();
                this.lblCity.Text = order.ShippingAddress.City.Trim();
                this.lblState.Text = order.ShippingAddress.State.Trim();
                this.lblZip.Text = order.ShippingAddress.ZipCode.Trim();
                this.lblPhoneNumber.Text = order.ShippingAddress.PhoneNumber.Trim();
                this.lblBillFname.Text = order.BillingAddress.FirstName.Trim();
                this.lblBillLname.Text = order.BillingAddress.LastName.Trim();
                this.lblBillAddress.Text = order.BillingAddress.Address1.Trim();
                this.lblBillCity.Text = order.BillingAddress.City.Trim();
                this.lblBillState.Text = order.BillingAddress.State.Trim();
                this.lblBillZip.Text = order.BillingAddress.ZipCode.Trim();
                this.lblBillPhone.Text = order.BillingAddress.PhoneNumber.Trim();
                this.lblSubTotal.Text = order.SubTotal.ToString("c").Trim();
                this.lblShippingTotal.Text = order.ShippingTotal.ToString("c").Trim();
                this.lblSalesTax.Text = order.SalesTax.ToString("c").Trim();
                this.lblOrderTotal.Text = (order.SubTotal + order.ShippingTotal + order.SalesTax).ToString("c");
                this.repOrderSummary.DataSource = order.OrderItems;
                this.repOrderSummary.DataBind();

                this.repOrderSummary.DataSource = order.OrderItems;
                this.repOrderSummary.DataBind();

            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "UserHome/MapOrderHistoryDetails", Ex = ex });
            }
        }

        /// <summary>
        /// Gets all the Orders to display on the page
        /// </summary>
        /// <param name="currentPage">The current page number</param>
        private void GetOrdersPerPage(int currentPage)
        {
            try
            {
                // Get Order History
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                var user = (AccountModel)Session["User"];
                var response = OrderService.GetOrderHistoryByPage(new GetOrderHistoryByPageRequest { AccountId = user.Id, PageIndex = currentPage });
                this.rpOrderHistory.DataSource = response.Orders;
                this.rpOrderHistory.DataBind();

                // Populate The Pager
                this.GetPageCount(response.RecordCount, currentPage);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "UserHome/GetOrdersPerPage", Ex = ex });
            }

        }

        /// <summary>
        /// Gets the page count then shows the correct repeater
        /// </summary>
        /// <param name="recordCount">The count of records in the table</param>
        /// <param name="currentPage">The current page number</param>
        private void GetPageCount(int recordCount, int currentPage)
        {
            try
            {
                // Get The Total Amount Of Pages
                double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(12));
                this.pageCount = (int)Math.Ceiling(dblPageCount);

                // Hide Repeater Buttons
                this.btnFirst.Visible = false;
                this.btnPrevious.Visible = false;
                this.btnNext.Visible = false;
                this.btnLast.Visible = false;

                // Show Repeater
                if (this.pageCount < 7)
                    ShowStandardRepeater(currentPage);
                else
                    ShowAdvancedRepeater(currentPage);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Shows the standard repeater
        /// </summary>
        /// <param name="currentPage">The current page number</param>
        private void ShowStandardRepeater(int currentPage)
        {
            try
            {
                // Get The Buttons For Each Page
                List<ListItem> pages = new List<ListItem>();
                this.btnLast.CommandArgument = pageCount.ToString();
                if (pageCount > 0)
                    for (int i = 1; i <= pageCount; i++)
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));

                // Bind Data To The Repeater
                rptPaging.DataSource = pages;
                rptPaging.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Shows the advanced repeater
        /// </summary>
        /// <param name="currentPage">The current page number</param>
        private void ShowAdvancedRepeater(int currentPage)
        {
            try
            {
                // Get The Buttons For Each Page
                List<ListItem> pages = new List<ListItem>();
                this.btnLast.CommandArgument = pageCount.ToString();

                // Check Current Page
                if (currentPage.Equals(1) || currentPage.Equals(2))
                {
                    // Show The First Three Pages
                    this.btnNext.Visible = true;
                    this.btnLast.Visible = true;
                    for (int i = 1; i <= 3; i++)
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                else if (currentPage.Equals(pageCount) || currentPage.Equals(pageCount - 1))
                {
                    // Show The Last Three Pages
                    this.btnFirst.Visible = true;
                    this.btnPrevious.Visible = true;
                    for (int i = pageCount - 2; i <= pageCount; i++)
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                else
                {
                    // Show All Buttons
                    this.btnNext.Visible = true;
                    this.btnLast.Visible = true;
                    this.btnFirst.Visible = true;
                    this.btnPrevious.Visible = true;

                    // Add Current Page Plus One On Either Side
                    pages.Add(new ListItem((currentPage - 1).ToString(), (currentPage - 1).ToString(), true));
                    pages.Add(new ListItem(currentPage.ToString(), currentPage.ToString(), false));
                    pages.Add(new ListItem((currentPage + 1).ToString(), (currentPage + 1).ToString(), true));
                }

                // Bind Data To The Repeater
                rptPaging.DataSource = pages;
                rptPaging.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}