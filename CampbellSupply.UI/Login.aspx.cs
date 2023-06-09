using System;
using System.Web.Security;
using System.Web.UI;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Web;
using CampbellSupply.Services;
using CampbellSupply.Common.RequestAndResponses;

namespace CampbellSupply
{
    public partial class Login : Page
    {
        /// <summary>
        /// The page load event.
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.RegisterHiddenField("_EVENTTARGET", btnLogin.UniqueID);
            if (!IsPostBack)
            {
                //var email = Response.Cookies["Username"].Value;
                //var password = Response.Cookies["Password"].Value;
                //if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                //{
                //    var response = AccountService.ValidateUser(new ValidateUserRequest { Email = email, Password = password });
                //    if (response.IsSuccess)
                //    {
                //        Session["user"] = response.User;
                //        if (response.User.IsAdmin)
                //        {
                //            FormsAuthentication.SetAuthCookie("validUser", true);
                //            Response.Redirect("Admin/AdminHome.aspx");
                //        }
                //        else
                //        {
                //            // Check previous page
                //            string page = "UserHome.aspx";
                //            if (Session["PreviousPage"] != null)
                //                if (!string.IsNullOrEmpty(Session["PreviousPage"].ToString().Trim()))
                //                    page = Session["PreviousPage"].ToString().Trim();

                //            // Clear session and redirect
                //            FormsAuthentication.SetAuthCookie("validUser", chkRemember.Checked);
                //            Session["PreviousPage"] = "";
                //            Response.Redirect(page, false);
                //        }
                //    }
                //}
            }

        }

        /// <summary>
        /// Handles the register user button click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Handles the login button click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var response = AccountService.ValidateUser(new ValidateUserRequest { Email = txtEmail.Text, Password = txtPassword.Text });
            if (!response.IsSuccess) lblError.Text = "*Invalid username or password";
            else
            {
                if (!ValidateRecaptcha())
                {
                    lblError.Text = "*Please validate the recaptcha";
                    return;
                }

                //if (chkRemember.Checked)
                //{
                //    var emailCookie = new HttpCookie("Username", txtEmail.Text.Trim());
                //    emailCookie.Expires = DateTime.Now.AddDays(30);
                //    Response.Cookies.Set(emailCookie);
                //    var passwordCookie = new HttpCookie("Password", txtPassword.Text.Trim());
                //    passwordCookie.Expires = DateTime.Now.AddDays(30);
                //    Response.Cookies.Set(passwordCookie);
                //}

                Session["user"] = response.User;
                if (response.User.IsAdmin)
                {
                    FormsAuthentication.SetAuthCookie("validUser", true);
                    Response.Redirect("Admin/AdminHome.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    // Check previous page
                    string page = "UserHome.aspx";
                    if (Session["PreviousPage"] != null)
                        if (!string.IsNullOrEmpty(Session["PreviousPage"].ToString().Trim()))
                            page = Session["PreviousPage"].ToString().Trim();

                    // Clear session and redirect
                    FormsAuthentication.SetAuthCookie("validUser", false);
                    Session["PreviousPage"] = "";
                    Response.Redirect(page, false);
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
        }

        /// <summary>
        /// Handles the guest checkout button click
        /// </summary>
        /// <param name="sender">The object sending the request</param>
        /// <param name="e">The event arg sent by the object</param>
        protected void btnGuestCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Checkout.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Validates the Recaptcha
        /// </summary>
        /// <returns></returns>
        protected bool ValidateRecaptcha()
        {
            var Response = Request["g-recaptcha-response"];
            var req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Lcf2RgUAAAAAJz70QCFvAqUZ7JncSJY55VZVsjM&response=" + Response);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                using (var readStream = new StreamReader(wResponse.GetResponseStream()))
                {
                    var js = new JavaScriptSerializer();
                    var jsonResponse = readStream.ReadToEnd();
                    dynamic data = JObject.Parse(jsonResponse);
                    return Convert.ToBoolean(data["success"]);
                }
            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Account/ValidateRecaptcha", Ex = ex });
                return false;
            }
        }
    }
}