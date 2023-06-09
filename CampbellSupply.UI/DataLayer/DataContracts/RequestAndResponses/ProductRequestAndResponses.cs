using System.Runtime.Serialization;
using CampbellSupply.DataLayer.DataContracts.Models;
using System.Collections.Generic;
using System;

namespace CampbellSupply.DataLayer.DataContracts.RequestAndResponses
{
    public class GetProductRequest : BaseRequest
    {
        public GetProductRequest()
        {
            this.ProductId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ProductId { get; set; }
    }

    public class GetProductResponse : BaseResponse
    {
        public GetProductResponse()
        {
            this.Product = new ProductModel();
        }

        [DataMember(IsRequired = true)]
        public ProductModel Product { get; set; }
    }

    public class GetProductsByPageRequest : BaseRequest
    {
        public GetProductsByPageRequest()
        {
            this.Department = string.Empty;
            this.Category = string.Empty;
            this.PageIndex = 0;
        }

        [DataMember(IsRequired = true)]
        public string Department { get; set; }
        [DataMember(IsRequired = true)]
        public string Category { get; set; }
        [DataMember(IsRequired = true)]
        public int PageIndex { get; set; }
    }

    public class GetProductsByPageResponse : BaseResponse
    {
        public GetProductsByPageResponse()
        {
            this.Products = new List<ProductsByPageModel>();
            this.RecordCount = 0;
        }

        [DataMember(IsRequired = true)]
        public List<ProductsByPageModel> Products { get; set; }
        [DataMember(IsRequired = true)]
        public int RecordCount { get; set; }
    }

    public class GetAdminProductsRequest : BaseRequest
    {
        public GetAdminProductsRequest()
        {
            this.Department = string.Empty;
            this.Category = string.Empty;
            this.IncludeInactive = false;
            this.IncludeDeleted = false;
        }

        [DataMember(IsRequired = true)]
        public string Department { get; set; }
        [DataMember(IsRequired = true)]
        public string Category { get; set; }
        [DataMember(IsRequired = true)]
        public bool IncludeInactive { get; set; }
        [DataMember(IsRequired = true)]
        public bool IncludeDeleted { get; set; }
    }

    public class GetAdminProductsResponse : BaseResponse
    {
        public GetAdminProductsResponse()
        {
            this.Products = new List<ProductModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ProductModel> Products { get; set; }
    }


    public class SearchProductsRequest : BaseRequest
    {
        public SearchProductsRequest()
        {
            this.SearchValue = string.Empty;
            this.PageIndex = 0;
        }

        [DataMember(IsRequired = true)]
        public string SearchValue { get; set; }
        [DataMember(IsRequired = true)]
        public int PageIndex { get; set; }
    }

    public class SearchProductsResponse : BaseResponse
    {
        public SearchProductsResponse()
        {
            this.Products = new List<ProductsByPageModel>();
            this.RecordCount = 0;
        }

        [DataMember(IsRequired = true)]
        public List<ProductsByPageModel> Products { get; set; }
        [DataMember(IsRequired = true)]
        public int RecordCount { get; set; }
    }

    public class SearchAdminProductsRequest : BaseRequest
    {
        public SearchAdminProductsRequest()
        {
            this.SearchValue = string.Empty;
            this.IncludeInactive = false;
            this.IncludeDeleted = false;
        }

        [DataMember(IsRequired = true)]
        public string SearchValue { get; set; }
        [DataMember(IsRequired = true)]
        public bool IncludeInactive { get; set; }
        [DataMember(IsRequired = true)]
        public bool IncludeDeleted { get; set; }
    }

    public class SearchAdminProductsResponse : BaseResponse
    {
        public SearchAdminProductsResponse()
        {
            this.Products = new List<ProductModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ProductModel> Products { get; set; }
    }


    public class SaveAdminProductRequest : BaseRequest
    {
        public SaveAdminProductRequest()
        {
            this.Product = new ProductModel();
        }

        [DataMember(IsRequired = true)]
        public ProductModel Product { get; set; }
    }

    public class SaveAdminProductResponse : BaseResponse { }


    public class GetTopRatedRequest : BaseRequest { }

    public class GetTopRatedResponse : BaseResponse
    {
        public GetTopRatedResponse()
        {
            this.TopRated = new List<TopRatedModel>();
        }

        [DataMember(IsRequired = true)]
        public List<TopRatedModel> TopRated { get; set; }
    }

    public class GetRatedAndReviewedRequest : BaseRequest { }

    public class GetRatedAndReviewedResponse : BaseResponse
    {
        public GetRatedAndReviewedResponse()
        {
            this.RatedAndReviewed = new List<AdminRatedReviewedModel>();
        }

        [DataMember(IsRequired = true)]
        public List<AdminRatedReviewedModel> RatedAndReviewed { get; set; }
    }

    public class SaveRatedAndReviewedRequest : BaseRequest
    {
        public SaveRatedAndReviewedRequest()
        {
            this.RatedAndReviewed = new AdminRatedReviewedModel();
        }

        [DataMember(IsRequired = true)]
        public AdminRatedReviewedModel RatedAndReviewed { get; set; }
    }

    public class SaveRatedAndReviewedResponse : BaseResponse { }


    public class GetFeaturedRequest : BaseRequest { }

    public class GetFeaturedResponse : BaseResponse
    {
        public GetFeaturedResponse()
        {
            this.Featured = new List<FeaturedModel>();
        }

        [DataMember(IsRequired = true)]
        public List<FeaturedModel> Featured { get; set; }
    }


    public class GetBestSellersRequest : BaseRequest { }

    public class GetBestSellersResponse : BaseResponse
    {
        public GetBestSellersResponse()
        {
            this.BestSellers = new List<BestSellerModel>();
        }

        [DataMember(IsRequired = true)]
        public List<BestSellerModel> BestSellers { get; set; }
    }


    public class GetOnSaleRequest : BaseRequest { }

    public class GetOnSaleResponse : BaseResponse
    {
        public GetOnSaleResponse()
        {
            this.OnSale = new List<OnSaleModel>();
        }

        [DataMember(IsRequired = true)]
        public List<OnSaleModel> OnSale { get; set; }
    }


    public class GetRecentlyViewedRequest : BaseRequest { }

    public class GetRecentlyViewedResponse : BaseResponse
    {
        public GetRecentlyViewedResponse()
        {
            this.RecentlyViewed = new List<RecentlyViewedModel>();
        }

        [DataMember(IsRequired = true)]
        public List<RecentlyViewedModel> RecentlyViewed { get; set; }
    }

    public class SaveRecentlyViewedRequest : BaseRequest
    {
        public SaveRecentlyViewedRequest()
        {
            this.ProductId = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid ProductId { get; set; }
    }

    public class SaveRecentlyViewedResponse : BaseResponse { }
}