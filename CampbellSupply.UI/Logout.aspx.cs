using System;
using System.Web.Security;

namespace CampbellSupply
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session["user"] = null;
            Session["ShoppingCart"] = null;
            Session.Clear();
            this.Session.Clear();
            Session.Abandon();

            Response.Redirect("/Home.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}