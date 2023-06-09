using System;
using CampbellSupply.Models;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.Helpers;
using CampbellSupply.DataLayer.DataContracts.Models;
using System.Web.UI;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;

namespace CampbellSupply
{
    public partial class Payment : System.Web.UI.Page
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
                    // Get List Of States 
                    this.ddlBillStates.DataSource = (List<StateModel>)Session["States"];
                    this.ddlBillStates.DataTextField = "Name";
                    this.ddlBillStates.DataValueField = "Name";
                    this.ddlBillStates.DataBind();

                    for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++) ddYear.Items.Add(new ListItem(i.ToString()));
                    if (Session["Order"] != null) this.SetBillingAddress(((OrderModel)Session["Order"]).BillingAddress);

                    // Get Shopping Cart Info
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

                    this.TotalOrder(shoppingCart, this.ddlBillStates.SelectedValue);
                }
                else
                {
                    WebControl wcICausedPostBack = (WebControl)GetControlThatCausedPostBack(sender as System.Web.UI.Page);
                    int indx = wcICausedPostBack.TabIndex;
                    var ctrl = from control in wcICausedPostBack.Parent.Controls.OfType<WebControl>()
                               where control.TabIndex > indx
                               select control;
                    ctrl.DefaultIfEmpty(wcICausedPostBack).First().Focus();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Enables the billing address to be changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.lnkEdit.Text = "";
                this.txtBillFname.ReadOnly = false;
                this.txtBillLname.ReadOnly = false;
                this.txtBillAddress1.ReadOnly = false;
                this.txtBillAddress1.ReadOnly = false;
                this.txtBillCity.ReadOnly = false;
                this.ddlBillStates.Enabled = false;
                this.txtBillZip.ReadOnly = false;
                this.txtBillPhone.ReadOnly = false;
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/lnkEdit_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the place order button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Order"] == null)
                {
                    Response.Redirect("Cart.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                //ScriptManager.RegisterStartupScript(this, GetType(), "showWaitContainer", "ShowWaitContainer()", true);
                var order = (OrderModel)Session["Order"];
                order.BillingAddress = this.GetBillingAddress();

                var request = new GetPlaceOrderRequest
                {
                    Order = order,
                    CCNumber = txtCCnumber.Text,
                    ExpMonth = Convert.ToInt32(ddExperation.SelectedValue),
                    ExpYear = Convert.ToInt32(ddYear.SelectedValue),
                    SessionId = Session.SessionID.ToString(),
                    CCV = Convert.ToInt32(txtCVV.Text)
                };
                var response = OrderService.PlaceOrder(request);
                if (!response.IsSuccess)
                {
                    lblErrorProcessing.Text = response.ErrorMessage;
                    this.HideWaitContainer();
                }
                else
                {
                    Session.Remove("ShoppingCart");
                    Session["Order"] = order;
                    Response.Redirect("Receipt.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/btnContinue_Click", Ex = ex });
                this.HideWaitContainer();
            }
        }

        /// <summary>
        /// Sub for keeping tab index
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected System.Web.UI.Control GetControlThatCausedPostBack(System.Web.UI.Page page)
        {
            System.Web.UI.Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    System.Web.UI.Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button || c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        /// <summary>
        /// Handles the text changed event of the credit card number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCCnumber_TextChanged(object sender, EventArgs e)
        {
            if (txtCCnumber.Text.Length < 16) lblCCnumber.Text = "Card number is missing numbers";
            else
            {
                lblCCnumber.Text = "";
                ValidateCreditCard.CreditCardTypeType? cardType = ValidateCreditCard.GetCardTypeFromNumber(txtCCnumber.Text);
                switch (cardType)
                {
                    case ValidateCreditCard.CreditCardTypeType.Discover:
                        imgVisa.Visible = false;
                        imgGrayVisa.Visible = true;
                        imgMasterCard.Visible = false;
                        imgGrayMasterCard.Visible = true;
                        imgDiscover.Visible = true;
                        imgGrayDiscover.Visible = false;
                        break;
                    case ValidateCreditCard.CreditCardTypeType.MasterCard:
                        imgVisa.Visible = false;
                        imgGrayVisa.Visible = true;
                        imgMasterCard.Visible = true;
                        imgGrayMasterCard.Visible = false;
                        imgDiscover.Visible = false;
                        imgGrayDiscover.Visible = true;
                        break;
                    case ValidateCreditCard.CreditCardTypeType.Visa:
                        imgVisa.Visible = true;
                        imgGrayVisa.Visible = false;
                        imgMasterCard.Visible = false;
                        imgGrayMasterCard.Visible = true;
                        imgDiscover.Visible = false;
                        imgGrayDiscover.Visible = true;
                        break;
                    case ValidateCreditCard.CreditCardTypeType.Amex:
                        lblCCnumber.Text = "Invalid card number, please check for mistakes";
                        break;
                }
                bool valid = ValidateCreditCard.IsValidNumber(txtCCnumber.Text, cardType);
                if (!valid) lblCCnumber.Text = "Invalid card number, please check for mistakes";
                else lblCCnumber.Text = "";
            }
            GetControlThatCausedPostBack(Page);
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
                this.TotalOrder(shoppingCart, this.ddlBillStates.SelectedValue);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/ddlBillStates_SelectedIndexChanged", Ex = ex });
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
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/SetBillingAddress", Ex = ex });
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
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/GetBillingAddress", Ex = ex });
                return new AddressModel();
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
                // Write To Error Log And Return Empty String
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/StripPhoneNumber", Ex = ex });
                return "";
            }
        }

        /// <summary>
        /// Totals up the order
        /// </summary>
        /// <param name="cart">The current shopping cart</param>
        private void TotalOrder(List<ShoppingCartModel> cart, string billingState)
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

                var shippiingState = "";
                if (Session["Order"] != null)
                {
                    var order = ((OrderModel)Session["Order"]);
                    if (order.ShippingAddress != null)
                        shippiingState = order.ShippingAddress.State;
                }

                var salesTax = SalesTax.Calculate(cart.Where(c => c.IsTaxable).ToList(), shippiingState);
                var orderTotal = (subTotal - couponAmount) + shippingTotal + salesTax;

                this.lblShippingTotal.Text = shippingTotal.ToString("c");
                this.lblSalesTax.Text = salesTax.ToString("c");
                this.lblSubTotal.Text = subTotal.ToString("c");
                this.lblOrderTotal.Text = orderTotal.ToString("c");
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Payment/TotalOrder", Ex = ex });
            }
        }

        /// <summary>
        /// Hide the wait container
        /// </summary>
        private void HideWaitContainer()
        {
            try
            {
                var waitDiv = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("m2_wait_container");
                if (waitDiv != null) waitDiv.Style.Add("display", "none");
            }
            catch (Exception) { }
        }
    }
}