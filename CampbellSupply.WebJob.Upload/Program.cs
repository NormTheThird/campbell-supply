using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;
using System;

namespace CampbellSupply.WebJob.Upload
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Models.Upload().UploadFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LoggingService.LogError(new LogErrorRequest { Class = "Upload/Main", Ex = ex });
            }
        }
    }
}