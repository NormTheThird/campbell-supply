using System;
using System.Web;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.Helpers;

namespace CampbellSupply
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get Session Id
                Session.Add("SessionID", HttpContext.Current.Session.SessionID);

                // Get New Sessions That Dont Already Exist
                if (Session["PreviousPage"] == null) Session.Add("PreviousPage", "");
                if (Session["WishListCount"] == null)  Session.Add("WishListCount", 0);
                if (Session["Featured"] == null) Session.Add("Featured", ProductService.GetFeatured(new GetFeaturedRequest()).Featured);

                // Get Lookup Tables
                Session.Add("Manufacturer", MenuService.GetManufacturers(new GetManufacturersRequest()).Manufacturers);
                Session.Add("States", new StateModel().GetStates());
            }
        }
    }
}