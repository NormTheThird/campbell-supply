using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.Models
{
    [DataContract]
    public class ProductModel : BaseModel
    {
        public ProductModel()
        {
            this.ManufacturerId = Guid.Empty;
            this.ManufacturerName = string.Empty;
            this.ManufacturerUPC = string.Empty;
            this.DepartmentId = Guid.Empty;
            this.DepartmentName = string.Empty;
            this.CategoryId = Guid.Empty;
            this.CategoryName = string.Empty;
            this.SubCategoryId = Guid.Empty;
            this.SubCategoryName = string.Empty;
            this.Sku = string.Empty;
            this.Name = string.Empty;
            this.DescriptionShort = string.Empty;
            this.DescriptionLong = string.Empty;
            this.PartNumber = string.Empty;
            this.WebPartNumber = string.Empty;
            this.Unkonwn = string.Empty;
            this.Color = string.Empty;
            this.Size = string.Empty;
            this.Brand = string.Empty;
            this.ImageUrl = string.Empty;
            this.Status = string.Empty;
            this.Market = string.Empty;
            this.Group = string.Empty;
            this.Mirror = string.Empty;
            this.Weight = 0.0m;
            this.PurchasePrice = 0.0m;
            this.OverridePrice = 0.0m;
            this.SalePrice = 0.0m;
            this.ShippingPrice = 0.0m;
            this.IsFeatured = false;
            this.IsOnSale = false;
            this.IsShippingValid = false;
            this.IsTaxable = false;
            this.IsActive = false;
            this.IsDeleted = false;
            this.DateOn = null;
            this.DateOff = null;
            this.DateActiveChanged = DateTime.MinValue;
            this.DateDeletedChanged = DateTime.MinValue;
            this.DateCreated = DateTime.MinValue;

            this.RatedReviewed = new List<ProductRatedReviwedModel>();
            this.RelatedSizeAndColor = new List<Tuple<Guid, string>>();
        }

        [DataMember(IsRequired = true)]
        public Guid ManufacturerId { get; set; }
        [DataMember(IsRequired = true)]
        public string ManufacturerName { get; set; }
        [DataMember(IsRequired = true)]
        public string ManufacturerUPC { get; set; }
        [DataMember(IsRequired = true)]
        public Guid DepartmentId { get; set; }
        [DataMember(IsRequired = true)]
        public string DepartmentName { get; set; }
        [DataMember(IsRequired = true)]
        public Guid CategoryId { get; set; }
        [DataMember(IsRequired = true)]
        public string CategoryName { get; set; }
        [DataMember(IsRequired = true)]
        public Guid SubCategoryId { get; set; }
        [DataMember(IsRequired = true)]
        public string SubCategoryName { get; set; }
        [DataMember(IsRequired = true)]
        public string Sku { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string DescriptionShort { get; set; }
        [DataMember(IsRequired = true)]
        public string DescriptionLong { get; set; }
        [DataMember(IsRequired = true)]
        public string PartNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string WebPartNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string Unkonwn { get; set; }
        [DataMember(IsRequired = true)]
        public string Color { get; set; }
        [DataMember(IsRequired = true)]
        public string Size { get; set; }
        [DataMember(IsRequired = true)]
        public string Brand { get; set; }
        [DataMember(IsRequired = true)]
        public string ImageUrl { get; set; }
        [DataMember(IsRequired = true)]
        public string ImageName { get; set; }
        [DataMember(IsRequired = true)]
        public string Status { get; set; }
        [DataMember(IsRequired = true)]
        public string Market { get; set; }
        [DataMember(IsRequired = true)]
        public string Group { get; set; }
        [DataMember(IsRequired = true)]
        public string Mirror { get; set; }
        [DataMember(IsRequired = true)]
        public decimal Weight { get; set; }
        [DataMember(IsRequired = true)]
        public decimal PurchasePrice { get; set; }
        [DataMember(IsRequired = true)]
        public decimal OverridePrice { get; set; }
        [DataMember(IsRequired = true)]
        public decimal SalePrice { get; set; }
        [DataMember(IsRequired = true)]
        public decimal ShippingPrice { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsFeatured { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsOnSale { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsTaxable { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsShippingValid { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsDeleted { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DateOn { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DateOff { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateActiveChanged { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateDeletedChanged { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateCreated { get; set; }

        [DataMember(IsRequired = true)]
        public List<ProductRatedReviwedModel> RatedReviewed { get; set; }
        [DataMember(IsRequired = true)]
        public List<Tuple<Guid, string>> RelatedSizeAndColor { get; set; }
    }

    [DataContract]
    public class ProductRatedReviwedModel : BaseModel
    {
        public ProductRatedReviwedModel()
        {
            this.ProductId = Guid.Empty;
            this.AccountId = Guid.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Review = string.Empty;
            this.Rating = 0.0m;
            this.DateCreated = DateTime.MinValue;
        }

        [DataMember(IsRequired = true)]
        public Guid ProductId { get; set; }
        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public string Review { get; set; }
        [DataMember(IsRequired = true)]
        public decimal Rating { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateCreated { get; set; }
    }

    [DataContract]
    public class ProductQuickModel
    {
        public ProductQuickModel()
        {
            this.ProductId = Guid.Empty;
            this.Name = string.Empty;
            this.Sku = string.Empty;
            this.Description = string.Empty;
            this.Manufacturer = string.Empty;
            this.ManufacturerUPC = string.Empty;
            this.Brand = string.Empty;
            this.PartNumber = string.Empty;
            this.Color = string.Empty;
            this.Size = string.Empty;
            this.Price = 0.0m;
            this.URL = string.Empty;
            this.IsTaxable = false;
        }

        [DataMember(IsRequired = true)]
        public Guid ProductId { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string Sku { get; set; }
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
        [DataMember(IsRequired = true)]
        public string Manufacturer { get; set; }
        [DataMember(IsRequired = true)]
        public string ManufacturerUPC { get; set; }
        [DataMember(IsRequired = true)]
        public string Brand { get; set; }
        [DataMember(IsRequired = true)]
        public string PartNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string Size { get; set; }
        [DataMember(IsRequired = true)]
        public string Color { get; set; }
        [DataMember(IsRequired = true)]
        public decimal Price { get; set; }
        [DataMember(IsRequired = true)]
        public string URL { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsTaxable { get; set; }
    }

    [DataContract]
    public class TopRatedModel : ProductQuickModel
    {
        public TopRatedModel()
        {
            this.Rating = 0.0m;
        }

        [DataMember(IsRequired = true)]
        public decimal Rating { get; set; }
    }

    [DataContract]
    public class FeaturedModel : ProductQuickModel { }

    [DataContract]
    public class BestSellerModel : ProductQuickModel { }

    [DataContract]
    public class OnSaleModel : ProductQuickModel { }

    [DataContract]
    public class RecentlyViewedModel : ProductQuickModel { }

    [DataContract]
    public class ProductsByPageModel : ProductQuickModel { }

    [DataContract]
    public class AdminRatedReviewedModel : ProductRatedReviwedModel
    {
        public AdminRatedReviewedModel()
        {
            this.ProductUpc = string.Empty;
            this.ProductName = string.Empty;
            this.IsActive = false;
            this.IsAuthorized = false;
            this.DateActiveChanged = DateTime.MinValue;
            this.DateAuthorizedChanged = DateTime.MinValue;
        }

        [DataMember(IsRequired = true)]
        public string ProductUpc { get; set; }
        [DataMember(IsRequired = true)]
        public string ProductName { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsAuthorized { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateActiveChanged { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateAuthorizedChanged { get; set; }
    }
}