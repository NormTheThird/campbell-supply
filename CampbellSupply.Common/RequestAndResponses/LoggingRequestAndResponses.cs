using System;
using System.Runtime.Serialization;

namespace CampbellSupply.Common.RequestAndResponses
{
    public class LogErrorRequest : BaseRequest
    {
        public LogErrorRequest()
        {
            this.Class = string.Empty;
            this.Ex = null;
            this.SendEmail = true;
        }

        [DataMember(IsRequired = true)]
        public string Class { get; set; }
        [DataMember(IsRequired = true)]
        public Exception Ex { get; set; }
        [DataMember(IsRequired = true)]
        public bool SendEmail { get; set; }

    }

    public class LogErrorResponse : BaseResponse { }
}