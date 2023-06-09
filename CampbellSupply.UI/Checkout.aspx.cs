using System;
using System.Web.UI;
using CampbellSupply.Models;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using System.Collections.Generic;
using CampbellSupply.Helpers;
using CampbellSupply.DataLayer.DataContracts.Models;
using System.Linq;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply
{
    public partial class Checkout : Page
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
                    // Get List Of States For Shipping Address
                    this.ddlStates.DataSource = (List<StateModel>)Session["States"];
                    this.ddlStates.DataTextField = "Name";
                    this.ddlStates.DataValueField = "Name";
                    this.ddlStates.DataBind();

                    // Get List Of States For Billing Address
                    this.ddlBillStates.DataSource = (List<StateModel>)Session["States"];
                    this.ddlBillStates.DataTextField = "Name";
                    this.ddlBillStates.DataValueField = "Name";
                    this.ddlBillStates.DataBind();

                    if (Session["ShoppingCart"] == null)
                    {
                        Response.Redirect("Cart.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    var shoppingCart = (List<ShoppingCartModel>)Session["ShoppingCart"];
                    if (!shoppingCart.Any())
                    {
                        Response.Redirect("Cart.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    this.repShoppingCartItems.DataSource = shoppingCart;
                    this.repShoppingCartItems.DataBind();

                    if (Session["Order"] != null)
                    {
                        var order = (OrderModel)Session["Order"];
                        this.SetShippingAddress(order.ShippingAddress);
                        this.SetBillingAddress(order.BillingAddress);
                    }
                    else if (Session["user"] != null)
                    {
                        var user = (AccountModel)Session["user"];
                        var response = OrderService.GetPreviousAddress(new GetPreviousAddressRequest { AccountId = user.Id });
                        if (response.IsSuccess)
                        {
                            this.SetShippingAddress(response.PrevShippingAddress);
                            this.SetBillingAddress(response.PrevBillingAddress);
                        }
                    }

                    this.TotalOrder(shoppingCart, ddlStates.SelectedValue, ddlBillStates.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the continue to payment button click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ShoppingCart"] == null)
                {
                    Response.Redirect("Cart.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    var shoppingCart = (List<ShoppingCartModel>)Session["ShoppingCart"];

                    AccountModel user = null;
                    if (Session["User"] != null) user = (AccountModel)Session["User"];
                    CouponModel coupon = null;
                    if (Session["Coupon"] != null) coupon = (CouponModel)Session["Coupon"];

                    var order = new OrderModel();
                    order.Id = Guid.NewGuid();
                    order.AccountId = user == null ? (Guid?)null : user.Id;
                    order.Email = this.txtEmail.Text.Trim();
                    order.ShippingAddress = this.GetShippingAddress();
                    order.BillingAddress = this.GetBillingAddress();
                    order.CouponId = coupon == null ? (Guid?)null : coupon.Id;
                    order.Number = RandomString.Generate(10);
                    order.SubTotal = Convert.ToDecimal(lblSubTotal.Text.Trim('$'));
                    order.ShippingTotal = Convert.ToDecimal(lblShippingTotal.Text.Trim('$'));
                    order.SalesTax = Convert.ToDecimal(lblSalesTax.Text.Trim('$'));
                    order.OrderItems = new List<OrderItemModel>();

                    foreach (var item in shoppingCart)
                    {
                        order.OrderItems.Add(new OrderItemModel
                        {
                            Id = Guid.NewGuid(),
                            ProductId = item.ProductId,
                            Name = item.Name,
                            Sku = item.Sku,
                            Description = item.Description,
                            Manufacturer = item.Manufacturer,
                            Brand = item.Brand,
                            PartNumber = item.PartNumber,
                            Color = item.Color,
                            Size = item.Size,
                            Price = item.Price,
                            ShippingPrice = item.ShippingPrice,
                            URL = item.URL,
                            IsTaxable = item.IsTaxable,
                            Quantity = item.Quantity
                        });
                    }

                    Session["Order"] = order;
                    Response.Redirect("Payment.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/btnPayment_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handels the cancle order button click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnCancleOrder_Click(object sender, EventArgs e)
        {
            Session["CancelOrder"] = true;
            Response.Redirect("cart.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Updates the billing address to match the shipping address
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void chkBillShip_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkBillShip.Checked) this.SetBillingAddress(this.GetShippingAddress());
                else this.SetBillingAddress(new AddressModel());
                if (Session["ShoppingCart"] == null)
                {
                    Response.Redirect("Cart.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                var shoppingCart = (List<ShoppingCartModel>)Session["ShoppingCart"];
                this.TotalOrder(shoppingCart, this.ddlStates.SelectedValue, this.ddlBillStates.SelectedValue);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/chkBillShip_CheckedChanged", Ex = ex });
            }
        }

        /// <summary>
        /// Handels the state for the billing address when changed.
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void ddlBillStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["ShoppingCart"] == null)
                {
                    Response.Redirect("Cart.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                var shoppingCart = (List<ShoppingCartModel>)Session["ShoppingCart"];
                this.TotalOrder(shoppingCart, this.ddlStates.SelectedValue,  this.ddlBillStates.SelectedValue);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/ddlBillStates_SelectedIndexChanged", Ex = ex });
            }
        }

        /// <summary>
        /// Gets the shipping address to save
        /// </summary>
        /// <returns>The shipping address</returns>
        private AddressModel GetShippingAddress()
        {
            try
            {
                var address = new AddressModel
                {
                    FirstName = this.txtFname.Text.Trim(),
                    LastName = this.txtLname.Text.Trim(),
                    PhoneNumber = this.StripPhoneNumber(this.txtPhone.Text.Trim()),
                    Address1 = this.txtAddress1.Text.Trim(),
                    Address2 = this.txtAddress2.Text.Trim(),
                    City = this.txtCity.Text.Trim(),
                    State = this.ddlStates.SelectedValue.Trim(),
                    ZipCode = this.txtZip.Text.Trim()
                };
                return address;
            }
            catch (Exception ex)
            {
                // Write To Error Log And Return New Shipping Address
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/GetShippingAddress", Ex = ex });
                return new AddressModel();
            }
        }

        /// <summary>
        /// Gets the billing address to save
        /// </summary>
        /// <returns>The billing address</returns>
        private AddressModel GetBillingAddress()
        {
            try
            {
                var address = new AddressModel
                {
                    FirstName = this.txtBillFname.Text.Trim(),
                    LastName = this.txtBillLname.Text.Trim(),
                    PhoneNumber = this.StripPhoneNumber(this.txtBillPhone.Text.Trim()),
                    Address1 = this.txtBillAddress1.Text.Trim(),
                    Address2 = this.txtBillAddress2.Text.Trim(),
                    City = this.txtBillCity.Text.Trim(),
                    State = this.ddlBillStates.SelectedValue.Trim(),
                    ZipCode = this.txtBillZip.Text.Trim()
                };
                return address;
            }
            catch (Exception ex)
            {
                // Write To Error Log And Return New Shipping Address
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/GetBillingAddress", Ex = ex });
                return new AddressModel();
            }
        }

        /// <summary>
        /// Totals up the order
        /// </summary>
        /// <param name="cart">The current shopping cart</param>
        private void TotalOrder(List<ShoppingCartModel> cart, string shippingState, string billingState)
        {
            try
            {
                this.divCoupon.Visible = false;
                var subTotal = cart.Select(c => c.Price).DefaultIfEmpty(0).Sum();

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

                var shippingTotal = cart.Select(c => c.ShippingPrice).DefaultIfEmpty(0).Sum();
                var salesTax = SalesTax.Calculate(cart.ToList(), shippingState);            
                var orderTotal = (subTotal - couponAmount) + shippingTotal + salesTax;

                this.lblShippingTotal.Text = shippingTotal.ToString("c");
                this.lblSalesTax.Text = salesTax.ToString("c");
                this.lblSubTotal.Text = subTotal.ToString("c");
                this.lblOrderTotal.Text = orderTotal.ToString("c");
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/TotalOrder", Ex = ex });
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
                this.txtFname.Text = address.FirstName.Trim();
                this.txtLname.Text = address.LastName.Trim();
                this.txtPhone.Text = address.PhoneNumber.Trim();
                this.txtAddress1.Text = address.Address1.Trim();
                this.txtAddress2.Text = address.Address2.Trim();
                this.txtCity.Text = address.City.Trim();
                this.ddlStates.SelectedValue = string.IsNullOrEmpty(address.State) ? "Alabama" : address.State.Trim();
                this.txtZip.Text = address.ZipCode.Trim();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/SetShippingAddress", Ex = ex });
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
                this.txtBillFname.Text = address.FirstName.Trim();
                this.txtBillLname.Text = address.LastName.Trim();
                this.txtBillPhone.Text = address.PhoneNumber.Trim();
                this.txtBillAddress1.Text = address.Address1.Trim();
                this.txtBillAddress2.Text = address.Address2.Trim();
                this.txtBillCity.Text = address.City.Trim();
                this.ddlBillStates.SelectedValue = string.IsNullOrEmpty(address.State) ? "Alabama" : address.State.Trim();
                this.txtBillZip.Text = address.ZipCode.Trim();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/SetBillingAddress", Ex = ex });
            }
        }

        /// <summary>
        /// Strips the mask from the phone number
        /// </summary>
        /// <param name="_phoneNumber">The phone number to remove the mask from</param>
        /// <returns>A 10 character phone number</returns>
        private string StripPhoneNumber(string _phoneNumber)
        {
            try
            {
                // Remove the "("
                if (_phoneNumber.Substring(0, 1).Equals("("))
                    _phoneNumber = _phoneNumber.Remove(0, 1);

                // Remove the "("
                if (_phoneNumber.Substring(3, 1).Equals(")"))
                    _phoneNumber = _phoneNumber.Remove(3, 1);

                // Remove the " "
                if (_phoneNumber.Substring(3, 1).Equals(" "))
                    _phoneNumber = _phoneNumber.Remove(3, 1);

                // Remove the "-"
                if (_phoneNumber.Substring(6, 1).Equals("-"))
                    _phoneNumber = _phoneNumber.Remove(6, 1);

                // Return 
                return _phoneNumber.Trim();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Checkout/StripPhoneNumber", Ex = ex });
                return "";
            }
        }
    }
}