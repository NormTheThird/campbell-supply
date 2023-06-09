using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Data;
using System;
using System.Collections.Generic;

namespace CampbellSupply.Services
{
    public static class LoggingService
    {
        public static LogErrorResponse LogError(LogErrorRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                using (var context = new CampbellSupplyEntities())
                {
                    var message = request.Ex.Message;
                    var stackTrace = request.Ex.StackTrace;
                    if(request.Ex.InnerException != null)
                    {
                        message += " INNER: " + request.Ex.InnerException.Message;
                        stackTrace += " INNER: " + request.Ex.InnerException.StackTrace;
                    }
                    
                    var error = new Error
                    {
                        Id = Guid.NewGuid(),
                        Class = request.Class,
                        Message = message,
                        StackTrace = stackTrace,
                        IsFixed = false,
                        IsReviewed = false,
                        DateFixed = null,
                        DateReviewed = null,
                        DateCreated = now
                    };
                    context.Errors.Add(error);
                    context.SaveChanges();
                    return new LogErrorResponse();
                }
            }
            catch (Exception) { return new LogErrorResponse(); }
        }
    }
}