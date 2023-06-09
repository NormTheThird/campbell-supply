using CampbellSupply.DataLayer.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.RequestAndResponses
{
    public class GetPageContentRequest : BaseRequest
    {
        public GetPageContentRequest()
        {
            this.PageName = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string PageName { get; set; }
    }

    public class GetPageContentResponse : BaseResponse
    {
        public GetPageContentResponse()
        {
            this.Content = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Content { get; set; }
    }


    public class SavePageContentRequest : BaseRequest
    {
        public SavePageContentRequest()
        {
            this.Content = string.Empty;
            this.PageName = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Content { get; set; }
        [DataMember(IsRequired = true)]
        public string PageName { get; set; }
    }

    public class SavePageContentResponse : BaseResponse { }

    public class GetWeeklyAdRequest : BaseRequest
    {
        public GetWeeklyAdRequest()
        {
            this.Id = Guid.Empty;
        }
        [DataMember(IsRequired = false)]
        public Guid Id { get; set; }
    }

    public class GetWeeklyAdResponse : BaseResponse
    {
        public GetWeeklyAdResponse()
        {
            this.WeeklyAds = new List<WeeklyAdModel>();
        }
        [DataMember(IsRequired = true)]
        public List<WeeklyAdModel> WeeklyAds { get; set; }
    }


    public class SaveWeeklyAdRequest : BaseRequest
    {
        public SaveWeeklyAdRequest()
        {
            this.WeeklyAd = new WeeklyAdModel();
        }
        [DataMember(IsRequired = true)]
        public WeeklyAdModel WeeklyAd { get; set; }
    }

    public class SaveWeeklyAdResponse : BaseResponse { }

}