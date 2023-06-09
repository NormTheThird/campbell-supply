using System;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply
{
    public partial class OrderPrintOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var orderNumber = Convert.ToString(Request.QueryString["OrderNumber"]);
                    var response = OrderService.GetOrder(new GetOrderRequest { OrderNumber = orderNumber });
                    if (response.IsSuccess)
                    {
                        this.lblOrderNumber.Text = response.Order.Number.Trim();
                        this.lblOrderDate.Text = response.Order.DateCreated.ToShortDateString();
                        this.lblPaid.Text = response.Order.IsPaid == true ? "Yes" : "No";
                        this.lblTransactionDate.Text = response.Order.DatePaid.ToString();
                        this.lblBillingName.Text = response.Order.BillingAddress.FirstName + " " + response.Order.BillingAddress.LastName;
                        this.lblEmail.Text = response.Order.Account.Email;
                        this.lblBillingPhone.Text = response.Order.BillingAddress.PhoneNumber;
                        this.lblBillingAddress.Text = response.Order.BillingAddress.Address1.Trim() + " "
                                                    + response.Order.BillingAddress.Address2.Trim();
                        this.lblBillingAddress2.Text = response.Order.BillingAddress.City.Trim() + ", "
                                                     + response.Order.BillingAddress.State.Trim() + " "
                                                     + response.Order.BillingAddress.ZipCode.Trim();
                        this.lblShippingName.Text = response.Order.ShippingAddress.FirstName + " " + response.Order.ShippingAddress.LastName;
                        this.lblShippingAddress.Text = response.Order.ShippingAddress.Address1.Trim() + " "
                                                   + response.Order.ShippingAddress.Address2.Trim();
                        this.lblShippingAddress2.Text = response.Order.ShippingAddress.City.Trim() + ", "
                                                     + response.Order.ShippingAddress.State.Trim() + " "
                                                     + response.Order.ShippingAddress.ZipCode.Trim();
                        this.lblSubTotal.Text = response.Order.SubTotal.ToString("c");
                        this.lblTransactionID.Text = response.Order.PaymentTransactionId;
                        this.lblCardNumber.Text = "XXXX" + response.Order.PaymentLast4.ToString();
                        this.lblCardExperation.Text = response.Order.PaymentExpiration;

                        var couponAmount = 0.0m;
                        if (response.Order.Coupon.Value == 0) this.lblCouponAmount.Text = "N/A";
                        else if (response.Order.Coupon.IsAmount)
                        {
                            response.Order.Coupon.Value.ToString("c");
                            couponAmount = response.Order.Coupon.Value;
                        }
                        else if (response.Order.Coupon.IsAmount)
                        {
                            response.Order.Coupon.Value.ToString("p");
                            couponAmount = response.Order.SubTotal * (response.Order.Coupon.Value / 100);
                        }

                        this.lblShippingTotal.Text = response.Order.ShippingTotal.ToString("c");
                        this.lblSalesTax.Text = response.Order.SalesTax.ToString("c");

                        var total = (response.Order.SubTotal - couponAmount) + response.Order.ShippingTotal + response.Order.SalesTax;
                        this.lblOrderTotal.Text = total.ToString("c");

                        // Get Order Items
                        foreach (var item in response.Order.OrderItems)
                        {
                            if (string.IsNullOrEmpty(item.Color)) item.Color = "";
                            else if (item.Color.Equals("-1", StringComparison.CurrentCultureIgnoreCase)) item.Color = "";
                            if (string.IsNullOrEmpty(item.Size)) item.Size = "";
                            else if (item.Size.Equals("-1", StringComparison.CurrentCultureIgnoreCase)) item.Size = "";
                        }
                        this.repOrderItems.DataSource = response.Order.OrderItems;
                        this.repOrderItems.DataBind();
                        //}
                    }
                }
                catch (Exception ex)
                {
                    // Write To Error Log And Return Empty DataSet
                    LoggingService.LogError(new LogErrorRequest { Class = "ChangeName", Ex = ex });
                }
            }
        }
    }
}