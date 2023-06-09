using System;
using System.Configuration;
using System.Net;

namespace CampbellSupply.Helpers
{
    public static class Pdf
    {
        public static byte[] UrlToPdf(string url)
        {
            try
            {
                using (var client = new WebClient())
                {

                    var stringUrl = $"http://{ConfigurationManager.AppSettings["EC2Instance"]}/pdf/urltopdf?url={url}";
                    var arr = client.DownloadData(stringUrl);
                    return arr;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}