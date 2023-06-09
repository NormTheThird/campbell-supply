using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.Services;
using System;
using System.Web.UI;

namespace CampbellSupply.Admin
{
    public partial class Admin : MasterPage
    {
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
                        Response.Redirect("../Login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        divLogin.Visible = false;
                        divLogout.Visible = true;
                        var user = (AccountModel)Session["user"];
                        lblName.Text = user.FirstName + " " + user.LastName;
                        if (!user.IsAdmin) Response.Redirect("../Home.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Admin/Page_Load", Ex = ex });
            }
        }
    }
}