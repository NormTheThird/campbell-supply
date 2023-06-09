using System;
using System.Configuration;
using System.Web.UI;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.Helpers;
using CampbellSupply.Common.RequestAndResponses;
using System.Collections.Generic;
using CampbellSupply.Services;

namespace CampbellSupply
{
    public partial class PasswordReset : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblMessage.Visible = false;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var resetId = Guid.NewGuid();
                var response = AccountService.PasswordReset(new PasswordResetRequest { Email = txtEmailAddress.Text, ResetId = resetId });
                if (!response.IsSuccess)
                {
                    lblError.Visible = true;
                    lblError.Text = response.ErrorMessage;
                }
                else
                {
                    string linkAddress = Request.Url.GetLeftPart(UriPartial.Authority).Trim() + "/passwordresetfinalize.aspx?id=" + resetId;
                    var body = "You have had a password reset request from Campbells Supply.  This link will expire in 15 minutes.<br><br> Please copy and paste the entire link below to finish resetting your password:<br>";
                    body += linkAddress;
                    body += " or <a href=" + linkAddress + ">Click Here.</a>";

                    var request = new SendEmailRequest
                    {
                        Recipients = new List<string> { txtEmailAddress.Text },
                        From = "no-reply@campbellsupply.net",
                        Subject = "Campbell Supply Password Reset",
                        Body = body.Trim()
                    };

                    var emailResponse = MessagingService.SendEmail(request);
                    if (!emailResponse.IsSuccess) throw new ApplicationException(response.ErrorMessage);
                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "PasswordReset/btnReset_Click", Ex = ex });
                lblError.Visible = true;
                lblError.Text = "Unable to reset password. Please contact us at info@campbellsupply.net";
            }
        }
    }
}