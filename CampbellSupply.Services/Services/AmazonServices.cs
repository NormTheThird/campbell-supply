using System;
using Amazon.S3;
using Amazon.S3.Model;
using CampbellSupply.Common.RequestAndResponses;
using System.IO;
using Amazon;

namespace CampbellSupply.Services.Services
{
    public static class AmazonServices
    {
        public static UploadResponse UploadFile(UploadRequest _request)
        {
            try
            {
                var response = new UploadResponse();
                var config = new AmazonS3Config { RegionEndpoint = RegionEndpoint.USEast1 };
                using (var client = new AmazonS3Client("AKIAJPTRE54O5LYUN2CA", "zm46q6a8PpMOMNGmoRgmvbH8Gw27OXusB9AI8Mxi", config))
                { 
                    var request = new PutObjectRequest { BucketName = _request.BucketName, CannedACL = S3CannedACL.PublicRead, Key = _request.FileName };
                    using (var ms = new MemoryStream(_request.File))
                    {
                        request.InputStream = ms;
                        client.PutObject(request);
                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "UploadFile", Ex = ex });
                return new UploadResponse { ErrorMessage = ex.Message };
            }
        }
    }
}
