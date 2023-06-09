using System;
using System.Configuration;
using System.Web;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Helpers;
using System.Linq;
using System.Web.UI;
using System.IO;
using System.Collections.Generic;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;

namespace CampbellSupply
{
    public partial class Receipt : Page
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
                    var order = (OrderModel)Session["Order"];
                    this.SetShippingAddress(order.ShippingAddress);
                    this.SetBillingAddress(order.BillingAddress);
                    this.lblOrderNumber.Text = order.Number;
                    this.repOrderItems.DataSource = order.OrderItems;
                    this.repOrderItems.DataBind();
                    this.TotalOrder(order, order.ShippingAddress.State, order.BillingAddress.State);

                    // Email order and receipt
                    this.EmailOrder(order.Number);
                    if (!string.IsNullOrEmpty(order.Email))
                        this.EmailReceipt(order.Number, order.ShippingAddress.FirstName, order.ShippingAddress.LastName, order.Email);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/Page_Load", Ex = ex });
            }
        }

        public void EmailReceipt(string orderNumber, string firstName, string lastName, string email)
        {
            byte[] attatchmentArray = null;

            try
            {
                attatchmentArray = Pdf.UrlToPdf("http://" + HttpContext.Current.Request.Url.Authority + "/EmailReceipt.aspx?OrderNumber=" + orderNumber);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/EmailReceipt: Create PDF", Ex = ex });
            }

            try
            {
                var request = new SendEmailRequest
                {
                    Recipients = new List<string> { email },
                    From = "no-reply@campbellsupply.net",
                    Subject = "Thank you for your Order!",
                    Body = "Your order has been placed #: " + orderNumber
                };

                if (attatchmentArray != null)
                {
                    request.AttachmentName = orderNumber + ".pdf";
                    request.Attachment = attatchmentArray;
                }

                var response = MessagingService.SendEmail(request);
                if (!response.IsSuccess) throw new ApplicationException(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/EmailReceipt: Send Email", Ex = ex });
            }
        }

        /// <summary>
        /// Converts the order to a printable pdf and sends it via email
        /// </summary>
        /// <param name="_orderID">The order id to convert and send</param>
        private void EmailOrder(string orderNumber)
        {
            byte[] attatchmentArray = null;

            try
            {
                attatchmentArray = Pdf.UrlToPdf("http://" + HttpContext.Current.Request.Url.Authority + "/OrderPrintOut.aspx?OrderNumber=" + orderNumber);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/EmailOrder: Create PDF", Ex = ex });
            }

            try
            {
                var toEmail = ConfigurationManager.AppSettings["OrderToEmail"];
                var request = new SendEmailRequest
                {
                    Recipients = new List<string> { toEmail },
                    From = "receipt@CampbellSupply.net",
                    Subject = "A new order has been placed #: " + orderNumber,
                    Body = "A new order has been placed #: " + orderNumber
                };

                if (attatchmentArray != null)
                {
                    request.AttachmentName = orderNumber + ".pdf";
                    request.Attachment = attatchmentArray;
                }

                var response = MessagingService.SendEmail(request);
                if (!response.IsSuccess) throw new ApplicationException(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/EmailOrder: Send Email", Ex = ex });
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
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/TotalOrder", Ex = ex });
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
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/SetShippingAddress", Ex = ex });
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
                LoggingService.LogError(new LogErrorRequest { Class = "Receipt/SetBillingAddress", Ex = ex });
            }
        }
    }
}