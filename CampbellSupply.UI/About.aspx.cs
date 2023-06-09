using System;

namespace CampbellSupply
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if this is the first page being visited
            if (Session["SessionID"] == null)
            {
                // Go to home page to get session variables then return
                Session.Add("PreviousPage", "About.aspx");
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();

            }
        }
    }
}