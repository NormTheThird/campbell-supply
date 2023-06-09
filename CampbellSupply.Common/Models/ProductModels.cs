using System;
using System.Runtime.Serialization;

namespace CampbellSupply.Common.Models
{
    [DataContract]
    public class ProductModel : BaseModel
    {
        public ProductModel()
        {
            this.ManufacturerUPC = string.Empty;
            this.DescriptionShort = string.Empty;
            this.PartNumber = string.Empty;
            this.ManufacturerName = string.Empty;
            this.Unkonwn = string.Empty;
            this.PurchasePrice = 0.0m;
            this.ImageUrl = string.Empty;
            this.Status = string.Empty;
            this.IsActive = false;
            this.IsDeleted = false;
            this.WebPartNumber = string.Empty;
            this.Sku = string.Empty;
            this.Name = string.Empty;
            this.DepartmentName = string.Empty;
            this.CategoryName = string.Empty;
            this.SubCategoryName = string.Empty;
            this.DescriptionLong = string.Empty;
            this.OverridePrice = 0.0m;
            this.ShippingPrice = 0.0m;
            this.IsShippingValid = false;
            this.Color = string.Empty;
            this.Size = string.Empty;
            this.Weight = 0.0m;
            this.DateOn = DateTime.MinValue;
            this.DateOff = DateTime.MinValue;
            this.IsFeatured = false;
            this.Brand = string.Empty;
            this.Market = string.Empty;
            this.Group = string.Empty;
            this.Mirror = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string ManufacturerUPC { get; set; }
        [DataMember(IsRequired = true)]
        public string DescriptionShort { get; set; }
        [DataMember(IsRequired = true)]
        public string PartNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string ManufacturerName { get; set; }
        [DataMember(IsRequired = true)]
        public string Unkonwn { get; set; }
        [DataMember(IsRequired = true)]
        public decimal PurchasePrice { get; set; }
        [DataMember(IsRequired = true)]
        public string ImageUrl { get; set; }
        [DataMember(IsRequired = true)]
        public string Status { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsDeleted { get; set; }
        [DataMember(IsRequired = true)]
        public string WebPartNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string Sku { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string DepartmentName { get; set; }
        [DataMember(IsRequired = true)]
        public string CategoryName { get; set; }
        [DataMember(IsRequired = true)]
        public string SubCategoryName { get; set; }
        [DataMember(IsRequired = true)]
        public string DescriptionLong { get; set; }
        [DataMember(IsRequired = true)]
        public decimal OverridePrice { get; set; }
        [DataMember(IsRequired = true)]
        public decimal ShippingPrice { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsShippingValid { get; set; }
        [DataMember(IsRequired = true)]
        public string Color { get; set; }
        [DataMember(IsRequired = true)]
        public string Size { get; set; }
        [DataMember(IsRequired = true)]
        public decimal Weight { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DateOn { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? DateOff { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsFeatured { get; set; }
        [DataMember(IsRequired = true)]
        public string Brand { get; set; }
        [DataMember(IsRequired = true)]
        public string Market { get; set; }
        [DataMember(IsRequired = true)]
        public string Group { get; set; }
        [DataMember(IsRequired = true)]
        public string Mirror { get; set; }
    }

    [DataContract]
    public class UploadModel : BaseModel
    {
        public UploadModel()
        {
            this.ManufacturerUPC = string.Empty;
            this.DescriptionShort = string.Empty;
            this.PartNumber = string.Empty;
            this.ManufacturerName = string.Empty;
            this.Unknown = string.Empty;
            this.PurchasePrice = string.Empty;
            this.ImageUrl = string.Empty;
            this.Status = string.Empty;
            this.IsActive = string.Empty;
            this.IsDeleted = string.Empty;
            this.WebPartNumber = string.Empty;
            this.Sku = string.Empty;
            this.Name = string.Empty;
            this.DepartmentName = string.Empty;
            this.CategoryName = string.Empty;
            this.SubCategoryName = string.Empty;
            this.DescriptionLong = string.Empty;
            this.OverridePrice = string.Empty;
            this.ShippingPrice = string.Empty;
            this.IsShippingValid = string.Empty;
            this.Color = string.Empty;
            this.Size = string.Empty;
            this.Weight = string.Empty;
            this.DateOn = string.Empty;
            this.DateOff = string.Empty;
            this.IsFeatured = string.Empty;
            this.Brand = string.Empty;
            this.Market = string.Empty;
            this.Group = string.Empty;
            this.Mirror = string.Empty;
            this.VisibilityId = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string ManufacturerUPC { get; set; }
        [DataMember(IsRequired = true)]
        public string DescriptionShort { get; set; }
        [DataMember(IsRequired = true)]
        public string PartNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string ManufacturerName { get; set; }
        [DataMember(IsRequired = true)]
        public string Unknown { get; set; }
        [DataMember(IsRequired = true)]
        public string PurchasePrice { get; set; }
        [DataMember(IsRequired = true)]
        public string ImageUrl { get; set; }
        [DataMember(IsRequired = true)]
        public string Status { get; set; }
        [DataMember(IsRequired = true)]
        public string IsActive { get; set; }
        [DataMember(IsRequired = true)]
        public string IsDeleted { get; set; }
        [DataMember(IsRequired = true)]
        public string WebPartNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string Sku { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string DepartmentName { get; set; }
        [DataMember(IsRequired = true)]
        public string CategoryName { get; set; }
        [DataMember(IsRequired = true)]
        public string SubCategoryName { get; set; }
        [DataMember(IsRequired = true)]
        public string DescriptionLong { get; set; }
        [DataMember(IsRequired = true)]
        public string OverridePrice { get; set; }
        [DataMember(IsRequired = true)]
        public string ShippingPrice { get; set; }
        [DataMember(IsRequired = true)]
        public string IsShippingValid { get; set; }
        [DataMember(IsRequired = true)]
        public string Color { get; set; }
        [DataMember(IsRequired = true)]
        public string Size { get; set; }
        [DataMember(IsRequired = true)]
        public string Weight { get; set; }
        [DataMember(IsRequired = true)]
        public string DateOn { get; set; }
        [DataMember(IsRequired = true)]
        public string DateOff { get; set; }
        [DataMember(IsRequired = true)]
        public string IsFeatured { get; set; }
        [DataMember(IsRequired = true)]
        public string Brand { get; set; }
        [DataMember(IsRequired = true)]
        public string Market { get; set; }
        [DataMember(IsRequired = true)]
        public string Group { get; set; }
        [DataMember(IsRequired = true)]
        public string Mirror { get; set; }
        [DataMember(IsRequired = true)]
        public string VisibilityId { get; set; }
    }
}