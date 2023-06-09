using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Data;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.Common.Models;
using System;
using System.Linq;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Services.Services;
using CampbellSupply.Common.RequestAndResponses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CampbellSupply.Services;

namespace CampbellSupply.DataLayer.DataServices
{
    public static class PageContentService
    {
        public static GetPageContentResponse GetPageContent(GetPageContentRequest request)
        {
            try
            {
                var response = new GetPageContentResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var content = context.PageContents.AsNoTracking().FirstOrDefault(p => p.IsActive && p.Name.Equals(request.PageName, StringComparison.CurrentCultureIgnoreCase));
                    if (content != null) response.Content = content.Content;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetPageContent", Ex = ex });
                return new GetPageContentResponse { ErrorMessage = "Unable to get page content." };
            }
        }

        public static SavePageContentResponse SavePageContent(SavePageContentRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SavePageContentResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var content = context.PageContents.FirstOrDefault(p => p.Name.Equals(request.PageName, StringComparison.CurrentCultureIgnoreCase));
                    if (content == null)
                    {
                        content = new PageContent { Id = Guid.NewGuid(), Name = request.PageName, IsActive = true, DateActiveChanged = now, DateCreated = now };
                        context.PageContents.Add(content);
                    }
                    content.Content = request.Content;
                    content.DateUpdated = now;
                    content.Location = "";
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SavePageContent", Ex = ex });
                return new SavePageContentResponse { ErrorMessage = "Unable to save page content." };
            }
        }

        public static GetWeeklyAdResponse GetWeeklyAds(GetWeeklyAdRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new GetWeeklyAdResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var ads = context.WeeklyAds.AsNoTracking().Select(w => new WeeklyAdModel
                    {
                        Id = w.Id,
                        Title = w.Title,
                        ImageName = w.ImageName,
                        ImageStoragePath = w.ImageStoragePath,
                        ImageStorageType = (StorageType)w.ImageStorageType,
                        FileName = w.FileName,
                        FileStoragePath = w.FileStoragePath,
                        FileStorageType = (StorageType)w.FileStorageType,
                        EffectiveDate = w.EffectiveDate,
                        EndDate = w.EndDate
                    });
                    response.WeeklyAds = ads.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetWeeklyAds", Ex = ex });
                return new GetWeeklyAdResponse { ErrorMessage = "Unable to save weekly ad." };
            }
        }

        public static GetWeeklyAdResponse GetWeeklyAd(GetWeeklyAdRequest request)
        {
            try
            {
                var response = new GetWeeklyAdResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var ads = context.WeeklyAds.AsNoTracking().Where(w => w.Id == request.Id)
                                                              .Select(w => new WeeklyAdModel
                    {
                        Id = w.Id,
                        Title = w.Title,
                        ImageName = w.ImageName,
                        ImageStoragePath = w.ImageStoragePath,
                        ImageStorageType = (StorageType)w.ImageStorageType,
                        FileName = w.FileName,
                        FileStoragePath = w.FileStoragePath,
                        FileStorageType = (StorageType)w.FileStorageType,
                        EffectiveDate = w.EffectiveDate,
                        EndDate = w.EndDate
                    });
                    response.WeeklyAds = ads.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetWeeklyAds", Ex = ex });
                return new GetWeeklyAdResponse { ErrorMessage = "Unable to save weekly ad." };
            }
        }

        public static SaveWeeklyAdResponse SaveWeeklyAd(SaveWeeklyAdRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveWeeklyAdResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var endDate = request.WeeklyAd.EndDate == DateTime.MinValue ? (DateTime?)null : request.WeeklyAd.EndDate;
                    var ad = context.WeeklyAds.FirstOrDefault(w => w.Id == request.WeeklyAd.Id);
                    if (ad == null)
                    {
                        ad = new Data.WeeklyAd { Id = Guid.NewGuid(), DateCreated = now };
                        context.WeeklyAds.Add(ad);
                    }
                    ad.Title = request.WeeklyAd.Title;                  
                    ad.EffectiveDate = request.WeeklyAd.EffectiveDate;
                    ad.EndDate = endDate;

                    if (request.WeeklyAd.File.Length > 0)
                    {
                        ad.FileName = request.WeeklyAd.FileName;
                        ad.FileStoragePath = @"https://s3-us-west-2.amazonaws.com/campbellsp/WeeklyAds/" + ad.FileName;
                        ad.FileSizeInBytes = request.WeeklyAd.File.Length;
                        ad.FileStorageType = (int)request.WeeklyAd.FileStorageType;

                        var fileUploadRequest = new UploadRequest { BucketName = "campbellsp", FileName = "WeeklyAds/" + ad.FileName, File = request.WeeklyAd.File };
                        var fileUploadResponse = AmazonServices.UploadFile(fileUploadRequest);
                        if (!fileUploadResponse.IsSuccess) return new SaveWeeklyAdResponse { ErrorMessage = "Unable to save weekly ad." };
                    }
                    if (request.WeeklyAd.Image.Length > 0)
                    {
                        ad.ImageName = request.WeeklyAd.ImageName;
                        ad.ImageStoragePath = @"https://s3-us-west-2.amazonaws.com/campbellsp/WeeklyAds/" + ad.ImageName;
                        ad.ImageSizeInBytes = request.WeeklyAd.Image.Length;
                        ad.ImageStorageType = (int)request.WeeklyAd.ImageStorageType;

                        var imageUploadRequest = new UploadRequest { BucketName = "campbellsp", FileName = "WeeklyAds/" + ad.ImageName, File = request.WeeklyAd.Image };
                        var imageUploadResponse = AmazonServices.UploadFile(imageUploadRequest);
                        if (!imageUploadResponse.IsSuccess) return new SaveWeeklyAdResponse { ErrorMessage = "Unable to save weekly ad." };
                    }
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SaveWeeklyAd", Ex = ex });
                return new SaveWeeklyAdResponse { ErrorMessage = "Unable to save weekly ad." };
            }
        }
    }
}