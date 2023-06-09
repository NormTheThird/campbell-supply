using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.Helpers;
using CampbellSupply.Services;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace CampbellSupply
{
    public partial class EmailReceipt : Page
    {
        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    var orderNumber = Convert.ToString(Request.QueryString["OrderNumber"]);
                    var response = OrderService.GetOrder(new GetOrderRequest { OrderNumber = orderNumber });
                    if (response.IsSuccess)
                    {
                        this.SetShippingAddress(response.Order.ShippingAddress);
                        this.SetBillingAddress(response.Order.BillingAddress);
                        this.lblOrderNumber.Text = response.Order.Number;
                        this.repOrderItems.DataSource = response.Order.OrderItems;
                        this.repOrderItems.DataBind();
                        this.TotalOrder(response.Order, response.Order.ShippingAddress.State, response.Order.BillingAddress.State);
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "EmailReceipt/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Totals up the order
        /// </summary>
        /// <param name="cart">The current shopping cart</param>
        private void TotalOrder(OrderModel order, string shippingState, string billingState)
        {
            try
            {
                this.divCoupon.Visible = false;
                var subTotal = order.OrderItems.Select(o => o.Price).DefaultIfEmpty(0).Sum();

                var couponAmount = 0.0m;
                if (Session["Coupon"] != null)
                {
                    var coupon = (CouponModel)Session["Coupon"];
                    if (coupon.IsAmount)
                    {
                        couponAmount = coupon.Value;
                        this.lblCouponText.Text = "Coupon";
                        this.lblCouponAmount.Text = "- " + coupon.Value.ToString("c");
                    }
                    else if (coupon.IsPercent)
                    {
                        couponAmount = subTotal * (coupon.Value / 100);
                        this.lblCouponText.Text = "Coupon " + (coupon.Value / 100).ToString("P1");
                        this.lblCouponAmount.Text = "- " + couponAmount.ToString("c");
                    }
                    this.divCoupon.Visible = true;
                }

                var shippingTotal = order.OrderItems.Select(o => o.ShippingPrice).DefaultIfEmpty(0).Sum();
                var salesTax = SalesTax.Calculate(order.OrderItems, shippingState);            
                var orderTotal = (subTotal - couponAmount) + shippingTotal + salesTax;

                this.lblSubTotal.Text = subTotal.ToString("c");
                this.lblShippingTotal.Text = shippingTotal.ToString("c");
                this.lblSalesTax.Text = salesTax.ToString("c");
                this.lblOrderTotal.Text = orderTotal.ToString("c");
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "EmailReceipt/TotalOrder", Ex = ex });
            }
        }

        /// <summary>
        /// Sets the user information to the shipping address
        /// </summary>
        /// <param name="user">The user to set the information to</param>
        private void SetShippingAddress(AddressModel address)
        {
            try
            {
                this.lblFname.Text = address.FirstName.Trim();
                this.lblLname.Text = address.LastName.Trim();
                this.lblPhoneNumber.Text = address.PhoneNumber.Trim();
                this.lblAddress1.Text = address.Address1.Trim();
                this.lblAddress2.Text = address.Address2.Trim();
                this.lblCity.Text = address.City.Trim();
                this.lblState.Text = address.State.Trim();
                this.lblZip.Text = address.ZipCode.Trim();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "EmailReceipt/SetShippingAddress", Ex = ex });
            }
        }

        /// <summary>
        /// Sets the user information to the billing address
        /// </summary>
        /// <param name="user">The user to set the information to</param>
        private void SetBillingAddress(AddressModel address)
        {
            try
            {
                this.lblBillFname.Text = address.FirstName.Trim();
                this.lblBillLname.Text = address.LastName.Trim();
                this.lblBillPhone.Text = address.PhoneNumber.Trim();
                this.lblBillAddress1.Text = address.Address1.Trim();
                this.lblBillAddress2.Text = address.Address2.Trim();
                this.lblBillCity.Text = address.City.Trim();
                this.lblBillState.Text = address.State.Trim();
                this.lblBillZip.Text = address.ZipCode.Trim();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "EmailReceipt/SetBillingAddress", Ex = ex });
            }
        }
    }
}