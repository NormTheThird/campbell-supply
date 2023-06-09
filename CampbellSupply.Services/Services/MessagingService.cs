using CampbellSupply.Common.RequestAndResponses;
using System;
using SendGrid;
using System.Configuration;
using SendGrid.Helpers.Mail;
using System.Linq;

namespace CampbellSupply.Services
{
    public static class MessagingService
    {
        public static SendEmailResponse SendEmail(SendEmailRequest request)
        {
            try
            {
                var response = new SendEmailResponse();
                var client = GetSendGridClient();
                var sendGridMessage = new SendGridMessage();
                sendGridMessage.From = new EmailAddress(request.From);
                sendGridMessage.AddTos(request.Recipients.Select(_ => new EmailAddress { Email = _ }).ToList());
                sendGridMessage.Subject = request.Subject;
                sendGridMessage.HtmlContent = request.Body;

                if (!string.IsNullOrEmpty(request.BCC))
                    sendGridMessage.AddBcc(request.BCC);

                if (request.Attachment != null)
                    sendGridMessage.AddAttachment(request.AttachmentName, Convert.ToBase64String(request.Attachment));

                var sendEmailResponse = client.SendEmailAsync(sendGridMessage).Result;
                if (sendEmailResponse.StatusCode != System.Net.HttpStatusCode.Accepted)
                    throw new ApplicationException($"Status Code: {sendEmailResponse.StatusCode}");
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SendEmail", Ex = ex });
                return new SendEmailResponse { ErrorMessage = ex.Message };

            }
        }

        private static SendGridClient GetSendGridClient()
        {
            return new SendGridClient(ConfigurationManager.AppSettings["SendGridAPIKey"]);
        }
    }
}