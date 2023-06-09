using System;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;
using System.Configuration;
using System.Linq;

namespace CampbellSupply
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if this is the first page being visited
            if (Session["SessionID"] == null)
            {
                // Go to home page to get session variables then return
                Session.Add("PreviousPage", "Contact.aspx");
                Response.Redirect("Home.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var body = "A website question has been submitted." + Environment.NewLine;
                body += "Name:" + fullname.Value + Environment.NewLine;
                body += "Email:" + email.Value + Environment.NewLine;
                body += "Message:" + message.Value + Environment.NewLine;
                body += "---------------------------------------" + Environment.NewLine;

                var request = new SendEmailRequest
                {
                    Recipients = ConfigurationManager.AppSettings["ContactUsToEmail"].Split('|').ToList(),
                    From = email.Value,
                    Subject = "A Website Comment has been Sent.",
                    Body = body.Trim()
                };

                var response = MessagingService.SendEmail(request);
                if (!response.IsSuccess) throw new ApplicationException(response.ErrorMessage);

                lblSuccess.Text = "Thanks!  Your message has been sent.";
                fullname.Value = "";
                email.Value = "";
                subject.Value = "";
                message.Value = "";

            }
            catch (Exception ex)
            {
                // Write To Error Log
                LoggingService.LogError(new LogErrorRequest { Class = "Contact/btnSubmit_Click", Ex = ex });
            }
        }
    }
}