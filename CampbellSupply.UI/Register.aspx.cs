using System;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;

namespace CampbellSupply
{
    public partial class Register : System.Web.UI.Page
    {
        protected void btnLogin_Register(object sender, EventArgs e)
        {

            if (txtPassword.Text.Length < 8 || txtPasswordConfirm.Text.Length < 8) lblPassError.Text = "*Password must be at least 8 characters long.";
            else
            {
                var user = new RegisterModel { FirstName = txtFname.Text, LastName = txtLname.Text, Email = txtEmail.Text, Password = txtPassword.Text, PhoneNumber = txtPhone.Text };
                var response = AccountService.RegisterUser(new RegisterUserRequest { User = user });
                if (!response.IsSuccess)
                {
                    lblError.Visible = true;
                    lblError.Text = response.ErrorMessage;
                }
                else
                {
                    Session["User"] = response.User;

                    // Check previous page
                    string page = "Home.aspx";
                    if (!string.IsNullOrEmpty(Session["PreviousPage"].ToString().Trim()))
                        page = Session["PreviousPage"].ToString().Trim();

                    // Clear session and redirect
                    Session["PreviousPage"] = "";
                    Response.Redirect(page, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }             
        }
    }
}