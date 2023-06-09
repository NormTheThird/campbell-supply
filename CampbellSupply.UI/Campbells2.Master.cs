using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using System.Linq;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply
{
    public partial class Campbells2 : System.Web.UI.MasterPage
    {
        public string menuString;

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
                    if (Session["user"] == null)
                    {
                        divLogin.Visible = true;
                        divLogout.Visible = false;
                    }
                    else
                    {
                        divLogin.Visible = false;
                        divLogout.Visible = true;
                        var user = (AccountModel)Session["user"];
                        lblName.Text = user.FirstName + " " + user.LastName;
                        if (user.IsAdmin) lsAdmin.Visible = true;
                        else lsAdmin.Visible = false;
                    }

                    List<WeeklyAdModel> ads = PageContentService.GetWeeklyAds(new GetWeeklyAdRequest()).WeeklyAds.Where(wa => wa.EffectiveDate < DateTime.Now && (wa.EndDate > DateTime.Now || wa.EndDate == null)).ToList();
                    if (ads.Count > 0)
                    {
                        this.WeekylAdLink.NavigateUrl = ads[0].FileStoragePath;
                        this.NavWeeklyAdLink.NavigateUrl = ads[0].FileStoragePath;
                    }
                    else
                    {
                        this.WeekylAdLink.NavigateUrl = "/content/CampbellsDefault.pdf";
                        this.NavWeeklyAdLink.NavigateUrl = "/Content/CampbellsDefault.pdf";
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Campbells2/Load_Page", Ex = ex, SendEmail = false });
            }
        }

        /// <summary>
        /// Handles the page prerender event to update the page content with new values
        /// </summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event being sent by the object</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Campbells/Page_PreRender", Ex = ex });
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
                var productId = Guid.Parse(((HtmlAnchor)sender).Name.ToString().Trim());
                var request = new SaveShoppingCartItemRequest { SessionId = Session.SessionID.ToString(), ProductId = productId, Quantity = 1 };
                var response = OrderService.SaveShoppingCartItem(request);
                if (response.IsSuccess) Session["ShoppingCart"] = response.Items;
                Response.Redirect("Cart.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Campbells2/AddToCart_Click", Ex = ex });
            }
        }
    }
}