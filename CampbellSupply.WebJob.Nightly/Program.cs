using System;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Services;

namespace CampbellSupply.WebJob.Nightly
{
    class Program
    {
        static void Main()
        {
            try
            {
                new Models.Nightly().CreateFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LoggingService.LogError(new LogErrorRequest { Class = "Nightly/Main", Ex = ex });
            }
        }
    }
}