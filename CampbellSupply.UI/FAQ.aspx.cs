using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CampbellSupply
{
    public partial class FAQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if this is the first page being visited
            if (Session["SessionID"] == null)
            {
                // Go to home page to get session variables then return
                Session.Add("PreviousPage", "FAQ.aspx");
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}