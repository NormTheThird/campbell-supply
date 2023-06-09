using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.RequestAndResponses
{
    [DataContract]
    public class BaseRequest
    {
        public BaseRequest()
        {
            this.UserToken = null;
        }

        [DataMember(IsRequired = true)]
        public string UserToken { get; set; }
    }

    [DataContract]
    public class BaseResponse
    {
        public BaseResponse()
        {
            this.IsSuccess = false;
            this.ErrorMessage = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public bool IsSuccess { get; set; }
        [DataMember(IsRequired = true)]
        public string ErrorMessage { get; set; }
    }
}