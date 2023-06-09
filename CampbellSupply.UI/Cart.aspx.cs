using System;
using System.Web.UI.HtmlControls;
using CampbellSupply.Models;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using System.Collections.Generic;
using System.Linq;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;

namespace CampbellSupply
{
    public partial class Cart : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Get The Shopping Cart
                    var shoppingCart = (List<ShoppingCartModel>)Session["ShoppingCart"];
                    this.repShoppingCartItems.DataSource = shoppingCart;
                    this.repShoppingCartItems.DataBind();
                    lblCartEmpty.Visible = false;
                    lblOrderCancel.Visible = false;

                    if (Session["CancelOrder"] != null)
                    {
                        if (Convert.ToBoolean(Session["CancelOrder"]))
                        {
                            lblOrderCancel.Visible = true;
                            Session["CancelOrder"] = false;
                        }
                    }
                    else if (shoppingCart == null )
                    {
                        lblCartEmpty.Visible = true;
                    }
                    else if (shoppingCart.Count > 0)
                    {
                        var subtotal = shoppingCart.Select(s => s.Price).DefaultIfEmpty(0).Sum();

                        this.lblSubTotal.Text = subtotal.ToString("c");
                        this.ApplyCouponCode(subtotal);
                        this.TotalOrder(shoppingCart);
                    }
                    else
                    {
                        lblCartEmpty.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log And Return Empty DataSet
                LoggingService.LogError(new LogErrorRequest { Class = "shoppingCart/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the checkout button click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["user"] == null)
                {
                    Session["PreviousPage"] = "Checkout.aspx";
                    Response.Redirect("Login.aspx", false);
                }
                else Response.Redirect("Checkout.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/btnCheckOut_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the continue shopping link click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnContShopping_Click(object sender, EventArgs e)
        {
            try
            {
                // Send To The Home Page
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/btnContShopping_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Adds a count to the quantity of the item
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void AddCountToItem(object sender, EventArgs e)
        {
            try
            {
                var id = Guid.Parse(((HtmlAnchor)sender).Name.ToString().Trim());
                this.AdjustCartItemQuantity(id, 1);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/AddCountToItem", Ex = ex });
            }
        }

        /// <summary>
        /// Removes a count to the quantity of the item
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void RemoveCountToItem(object sender, EventArgs e)
        {
            try
            {
                var id = Guid.Parse(((HtmlAnchor)sender).Name.ToString().Trim());
                this.AdjustCartItemQuantity(id, -1);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/RemoveCountToItem", Ex = ex });
            }
        }

        /// <summary>
        /// Deletes the item from the cart
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void DeleteItemFromCart(object sender, EventArgs e)
        {
            try
            {
                var id = Guid.Parse(((HtmlAnchor)sender).Name.ToString().Trim());
                if (Session["ShoppingCart"] != null)
                {
                    var cart = (List<ShoppingCartModel>)Session["ShoppingCart"];
                    var item = cart.FirstOrDefault(c => c.Id == id);
                    if (item != null) this.AdjustCartItemQuantity(id, -item.Quantity);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/DeleteItemFromCart", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the coupon apply button click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // Get Coupon Code
                var response = OrderService.GetCoupon(new GetCouponRequest { Code = this.couponCode.Value });
                if (response.IsSuccess) Session["Coupon"] = response.Coupon;
                else Session["Coupon"] = null;
                var shoppingCart = ((List<ShoppingCartModel>)Session["ShoppingCart"]);
                var subtotal = shoppingCart.Select(s => s.Price).DefaultIfEmpty(0).Sum();
                this.ApplyCouponCode(subtotal);
                this.TotalOrder(shoppingCart);
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/btnApply_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Enables And Disables The Coupon Fields
        /// </summary>
        private void ApplyCouponCode(decimal subtotal)
        {
            try
            {
                this.divCoupon.Visible = false;
                if (subtotal == 0.0m) Session["Coupon"] = null;
                if (Session["Coupon"] != null)
                {
                    var coupon = (CouponModel)Session["Coupon"];
                    this.couponCode.Value = coupon.Code.Trim();
                    if (coupon.IsAmount)
                    {
                        this.lblCouponText.Text = "Coupon";
                        this.lblCouponAmount.Text = "- " + coupon.Value.ToString("c");
                        this.divCoupon.Visible = true;
                    }
                    else if (coupon.IsPercent)
                    {
                        this.lblCouponText.Text = "Coupon " + (coupon.Value / 100).ToString("P1");
                        this.lblCouponAmount.Text = "- " + (subtotal * (coupon.Value / 100)).ToString("c");
                        this.divCoupon.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/ChangeName", Ex = ex });
            }
        }

        /// <summary>
        /// Totals up the order
        /// </summary>
        /// <param name="cart">The current shopping cart</param>
        private void TotalOrder(List<ShoppingCartModel> cart)
        {
            try
            {
                this.divCoupon.Visible = false;
                var subTotal = cart.Select(c => c.Price).DefaultIfEmpty(0).Sum();
                var taxableSubTotal = cart.Where(c => !c.Name.ToUpper().Trim().Equals("GIFT CARD")).Select(c => c.Price).DefaultIfEmpty(0).Sum();
                this.lblSubTotal.Text = subTotal.ToString("c");

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
                this.lblShippingTotal.Text = shippingTotal.ToString("c");

                var orderTotal = (subTotal - couponAmount) + shippingTotal;
                this.lblOrderTotal.Text = orderTotal.ToString("c");

            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/TotalOrder", Ex = ex });
            }
        }

        /// <summary>
        /// Adjust the cart item quantity
        /// </summary>
        /// <param name="id">The id of the cart item</param>
        /// <param name="quantity">The quantity to adjust</param>
        private void AdjustCartItemQuantity(Guid id, int quantity)
        {
            try
            {
                var request = new AdjustShoppingCartItemRequest { SessionId = Session.SessionID.ToString(), Id = id, Quantity = quantity };
                var response = OrderService.AdjustShoppingCartItem(request);
                if (response.IsSuccess) Session["ShoppingCart"] = response.Items;
                Response.Redirect("Cart.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Cart/AdjustCartItemQuantity", Ex = ex });
            }
        }
    }
}