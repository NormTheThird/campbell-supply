using System;
using System.Web.UI.HtmlControls;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply
{
    public partial class WishList : System.Web.UI.Page
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
                    if (Session["User"] == null)
                    {
                        Session["PreviousPage"] = "WishList.aspx";
                        Response.Redirect("Login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else this.GetWishList();
                }           
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "WishList/Page_Load", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the page prerender event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                this.GetWishList();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "WishList/Page_PreRender", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the add to cart button click event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void AddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    var user = (AccountModel)Session["User"];
                    var productId = Guid.Parse((sender as HtmlAnchor).Name.ToString().Trim());
                    AccountService.RemoveWishListItem(new SaveWishListItemRequest { AccountId = user.Id, ProductId = productId });

                    var request = new SaveShoppingCartItemRequest { SessionId = Session.SessionID.ToString(), ProductId = productId, Quantity = 1 };
                    var response = OrderService.SaveShoppingCartItem(request);
                    if (response.IsSuccess) Session["ShoppingCart"] = response.Items;
                    Response.Redirect("Cart.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "ChangeName", Ex = ex });
            }
        }

        /// <summary>
        /// Handles the delete from wish list click event
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void DeleteFromWishList_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    var user = (AccountModel)Session["User"];
                    var productId = Guid.Parse((sender as HtmlAnchor).Name.ToString().Trim());
                    AccountService.RemoveWishListItem(new SaveWishListItemRequest { AccountId = user.Id, ProductId = productId });
                    this.GetWishList();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "WishList/DeleteFromWishList_Click", Ex = ex });
            }
        }

        /// <summary>
        /// Repopulates the wish list
        /// </summary>
        private void GetWishList()
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    var user = (AccountModel)Session["User"];
                    this.repWishList.DataSource = AccountService.GetWishList(new GetWishListRequest { AccountId = user.Id }).WishList;
                    this.repWishList.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "WishList/GetWishList", Ex = ex });
            }
        }
    }
}