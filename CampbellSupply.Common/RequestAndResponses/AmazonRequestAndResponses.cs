using System.Runtime.Serialization;

namespace CampbellSupply.Common.RequestAndResponses
{
    public class UploadRequest : BaseRequest
    {
        public UploadRequest()
        {
            this.BucketName = string.Empty;
            this.FileName = string.Empty;
            this.File = null;
        }

        [DataMember(IsRequired = true)]
        public string BucketName { get; set; }
        [DataMember(IsRequired = true)]
        public string FileName { get; set; }
        [DataMember(IsRequired = true)]
        public byte[] File { get; set; }

    }

    public class UploadResponse : BaseResponse { }
}
