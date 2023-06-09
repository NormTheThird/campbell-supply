using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CampbellSupply.Common.RequestAndResponses
{
    public class SendEmailRequest : BaseRequest
    {
        public SendEmailRequest()
        {
            this.Recipients = new List<string>();
            this.BCC = string.Empty;
            this.From = string.Empty;
            this.Subject = string.Empty;
            this.Body = string.Empty;
            this.AttachmentName = string.Empty;
            this.Attachment = null;
        }

        [DataMember(IsRequired = true)]
        public List<string> Recipients { get; set; }
        [DataMember(IsRequired = true)]
        public string BCC { get; set; }
        [DataMember(IsRequired = true)]
        public string From { get; set; }
        [DataMember(IsRequired = true)]
        public string Subject { get; set; }
        [DataMember(IsRequired = true)]
        public string Body { get; set; }
        [DataMember(IsRequired = true)]
        public string AttachmentName { get; set; }
        [DataMember(IsRequired = true)]
        public byte[] Attachment { get; set; }
    }

    public class SendEmailResponse : BaseResponse { }
}
