using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Data;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CampbellSupply.DataLayer.DataServices
{
    public static class ProductService
    {
        public static GetProductResponse GetProduct(GetProductRequest request)
        {
            try
            {
                var response = new GetProductResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var product = context.Products.AsNoTracking().FirstOrDefault(p => p.Id == request.ProductId && !p.IsDeleted && p.IsActive);
                    if (product == null) return null;
                    response.Product = new ProductModel
                    {
                        Id = product.Id,
                        ManufacturerId = product.ManufacturerId ?? Guid.Empty,
                        ManufacturerName = product.Manufacturer == null ? string.Empty : product.Manufacturer.Name,
                        ManufacturerUPC = product.Manufacturer == null ? string.Empty : product.ManufacturerUPC,
                        DepartmentId = product.DepartmentId ?? Guid.Empty,
                        DepartmentName = product.Department == null ? string.Empty : product.Department.Name,
                        CategoryId = product.DepartmentId ?? Guid.Empty,
                        CategoryName = product.Category == null ? string.Empty : product.Category.Name,
                        SubCategoryId = product.SubCategoryId ?? Guid.Empty,
                        SubCategoryName = product.SubCategory == null ? string.Empty : product.SubCategory.Name,
                        Sku = product.ManufacturerUPC,
                        Name = product.Name,
                        DescriptionShort = product.DescriptionShort,
                        DescriptionLong = product.DescriptionLong,
                        PartNumber = product.PartNumber,
                        WebPartNumber = product.WebPartNumber,
                        Unkonwn = product.Unknown,
                        Color = product.Color == "-1" ? "" : product.Color,
                        Size = product.Size == "-1" ? "" : product.Size,
                        Brand = product.Brand,
                        ImageName = product.ImageURL,
                        ImageUrl = "https://s3-us-west-2.amazonaws.com/campbellsp/Product/" + product.ImageURL,
                        Status = product.Status,
                        Market = product.Market,
                        Group = product.Group,
                        Mirror = product.Mirror,
                        Weight = product.Weight,
                        PurchasePrice = product.OverridePrice > 0 ? product.OverridePrice : product.PurchasePrice,
                        OverridePrice = product.OverridePrice,
                        SalePrice = product.SalePrice,
                        ShippingPrice = product.ShippingPrice,
                        IsFeatured = product.IsFeatured,
                        IsOnSale = product.IsOnSale,
                        IsShippingValid = product.IsShippingValid,
                        IsTaxable = product.IsTaxable,
                        IsActive = product.IsActive,
                        IsDeleted = product.IsDeleted,
                        DateOn = product.DateOn ?? DateTime.MinValue,
                        DateOff = product.DateOff ?? DateTime.MinValue,
                        DateActiveChanged = product.DateActiveChanged,
                        DateDeletedChanged = product.DateDeletedChanged,
                        DateCreated = product.DateCreated,
                        RatedReviewed = product.RatedRevieweds.Where(r => r.IsAuthorized && r.IsActive).Select(r =>
                            new ProductRatedReviwedModel
                            {
                                Id = r.Id,
                                AccountId = r.AccountId,
                                ProductId = r.ProductId,
                                FirstName = r.Account.FirstName,
                                LastName = r.Account.LastName,
                                Review = r.Review,
                                Rating = r.Rating,
                                DateCreated = r.DateCreated
                            }).ToList(),
                        RelatedSizeAndColor = new List<Tuple<Guid, string>>()
                    };

                    var products = context.Products.Where(p => p.Name.Trim().Equals(product.Name.Trim(), StringComparison.InvariantCultureIgnoreCase) && !p.IsDeleted && p.IsActive).ToList();
                    foreach (var related in products)
                    {
                        var value = "";
                        if (!string.IsNullOrEmpty(related.Size))
                            if (!related.Size.Trim().Equals("-1"))
                                value = "Size: " + related.Size.Trim() + " ";
                        if (!string.IsNullOrEmpty(related.Color))
                            if (!related.Color.Trim().Equals("-1"))
                                value += "Color: " + related.Color.Trim();
                        if (string.IsNullOrEmpty(value))
                            value = related.PartNumber;
                        response.Product.RelatedSizeAndColor.Add(new Tuple<Guid, string>(related.Id, value));
                    }

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetProduct", Ex = ex });
                return new GetProductResponse { ErrorMessage = "Unable to get specified product." };
            }
        }

        public static GetProductsByPageResponse GetProductsByPage(GetProductsByPageRequest request)
        {
            try
            {
                var response = new GetProductsByPageResponse();
                if (request.Category.ToUpper().Trim().Equals("ALL"))
                {
                    using (var context = new CampbellSupplyEntities())
                    {
                        response.RecordCount = context.Products.AsNoTracking().Where(p => p.Department.Name.Trim().Equals(request.Department, StringComparison.InvariantCultureIgnoreCase)
                                                                                       && p.IsActive && !p.IsDeleted).GroupBy(p => p.Name).Count();

                        var productNames = context.Products.AsNoTracking().Where(p => p.Department.Name.Trim().Equals(request.Department, StringComparison.InvariantCultureIgnoreCase) && p.IsActive && !p.IsDeleted)
                                                                          .GroupBy(p => p.Name)
                                                                          .OrderBy(p => p.Key).Skip(12 * (request.PageIndex - 1)).Take(12)
                                                                          .Select(p => p.Key).ToList();


                        foreach (var name in productNames)
                        {
                            var product = context.Products.FirstOrDefault(p => p.Name == name && p.IsActive && !p.IsDeleted);
                            response.Products.Add(new ProductsByPageModel
                            {
                                ProductId = product.Id,
                                Name = product.Name,
                                Description = product.DescriptionShort,
                                Manufacturer = product.Manufacturer.Name,
                                Brand = product.Brand,
                                PartNumber = product.PartNumber,
                                Price = product.OverridePrice > 0 ? product.OverridePrice : product.PurchasePrice,
                                URL = product.ImageURL
                            });
                        }

                        response.IsSuccess = true;
                        return response;
                    }
                }
                else
                {
                    using (var context = new CampbellSupplyEntities())
                    {
                        response.RecordCount = context.Products.AsNoTracking().Where(p => p.Department.Name.Trim().Equals(request.Department, StringComparison.InvariantCultureIgnoreCase)
                                                                                       && p.Category.Name.Trim().Equals(request.Category, StringComparison.InvariantCultureIgnoreCase)
                                                                                       && p.IsActive && !p.IsDeleted).GroupBy(p => p.Name).Count();
                        var productNames = context.Products.AsNoTracking().Where(p => p.Department.Name.Trim().Equals(request.Department, StringComparison.InvariantCultureIgnoreCase)
                                                                                   && p.Category.Name.Trim().Equals(request.Category, StringComparison.InvariantCultureIgnoreCase)
                                                                                   && p.IsActive && !p.IsDeleted)
                                                                          .GroupBy(p => p.Name)
                                                                          .OrderBy(p => p.Key).Skip(12 * (request.PageIndex - 1)).Take(12)
                                                                          .Select(p => p.Key).ToList();

                        foreach (var name in productNames)
                        {
                            var product = context.Products.FirstOrDefault(p => p.Name == name && p.IsActive && !p.IsDeleted);
                            response.Products.Add(new ProductsByPageModel
                            {
                                ProductId = product.Id,
                                Name = product.Name,
                                Description = product.DescriptionShort,
                                Manufacturer = product.Manufacturer.Name,
                                Brand = product.Brand,
                                PartNumber = product.PartNumber,
                                Price = product.OverridePrice > 0 ? product.OverridePrice : product.PurchasePrice,
                                URL = product.ImageURL
                            });
                        }

                        response.IsSuccess = true;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetProductsByPage", Ex = ex });
                return new GetProductsByPageResponse { ErrorMessage = "Unable to get products by page." };
            }
        }

        public static GetAdminProductsResponse GetAdminProducts(GetAdminProductsRequest request)
        {
            try
            {
                var response = new GetAdminProductsResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var products = context.Products.AsNoTracking().Where(p => p.Department.Name.Trim().Equals(request.Department, StringComparison.InvariantCultureIgnoreCase)
                                                                           && (request.IncludeInactive ? (!p.IsActive || p.IsActive) : p.IsActive)
                                                                           && (request.IncludeDeleted ? (!p.IsDeleted || p.IsDeleted) : !p.IsDeleted))
                                                                  .Select(p => new ProductModel
                                                                  {
                                                                      Id = p.Id,
                                                                      ManufacturerId = p.ManufacturerId ?? Guid.Empty,
                                                                      ManufacturerName = p.Manufacturer == null ? string.Empty : p.Manufacturer.Name,
                                                                      ManufacturerUPC = p.Manufacturer == null ? string.Empty : p.ManufacturerUPC,
                                                                      DepartmentId = p.DepartmentId ?? Guid.Empty,
                                                                      DepartmentName = p.Department == null ? string.Empty : p.Department.Name,
                                                                      CategoryId = p.DepartmentId ?? Guid.Empty,
                                                                      CategoryName = p.Category == null ? string.Empty : p.Category.Name,
                                                                      SubCategoryId = p.SubCategoryId ?? Guid.Empty,
                                                                      SubCategoryName = p.SubCategory == null ? string.Empty : p.SubCategory.Name,
                                                                      Sku = p.ManufacturerUPC,
                                                                      Name = p.Name,
                                                                      DescriptionShort = p.DescriptionShort,
                                                                      DescriptionLong = p.DescriptionLong,
                                                                      PartNumber = p.PartNumber,
                                                                      WebPartNumber = p.WebPartNumber,
                                                                      Unkonwn = p.Unknown,
                                                                      Color = p.Color == "-1" ? "" : p.Color,
                                                                      Size = p.Size == "-1" ? "" : p.Size,
                                                                      Brand = p.Brand,
                                                                      ImageUrl = p.ImageURL,
                                                                      Status = p.Status,
                                                                      Market = p.Market,
                                                                      Group = p.Group,
                                                                      Mirror = p.Mirror,
                                                                      Weight = p.Weight,
                                                                      PurchasePrice = p.PurchasePrice,
                                                                      OverridePrice = p.OverridePrice,
                                                                      SalePrice = p.SalePrice,
                                                                      ShippingPrice = p.ShippingPrice,
                                                                      IsFeatured = p.IsFeatured,
                                                                      IsOnSale = p.IsOnSale,
                                                                      IsShippingValid = p.IsShippingValid,
                                                                      IsActive = p.IsActive,
                                                                      IsDeleted = p.IsDeleted,
                                                                      DateOn = p.DateOn,
                                                                      DateOff = p.DateOff,
                                                                      DateActiveChanged = p.DateActiveChanged,
                                                                      DateDeletedChanged = p.DateDeletedChanged,
                                                                      DateCreated = p.DateCreated
                                                                  });
                    if (products != null) response.Products = products.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetAdminProducts", Ex = ex });
                return new GetAdminProductsResponse { ErrorMessage = "Unable to get products." };
            }
        }

        public static SearchProductsResponse SerachProducts(SearchProductsRequest request)
        {
            try
            {
                var response = new SearchProductsResponse();
                if (string.IsNullOrEmpty(request.SearchValue)) return new SearchProductsResponse { ErrorMessage = "Search value cannot be empty." };
                /*
                using (var context = new CampbellSupplyEntities())
                {
                    response.RecordCount = context.Products.AsNoTracking().Where(p => p.Name.Contains(request.SearchValue) && p.IsActive && !p.IsDeleted).Count();
                    var products = context.Products.AsNoTracking().Where(p => p.Name.Contains(request.SearchValue) && p.IsActive && !p.IsDeleted)
                                                  .OrderBy(p => p.Name).Skip(12 * (request.PageIndex - 1)).Take(12)
                                                  .Select(p => new ProductsByPageModel
                                                  {
                                                      ProductId = p.Id,
                                                      Name = p.Name,
                                                      Description = p.DescriptionShort,
                                                      Manufacturer = p.Manufacturer.Name,
                                                      Brand = p.Brand,
                                                      Price = p.OverridePrice > 0 ? p.OverridePrice : p.PurchasePrice,
                                                      URL = p.ImageURL
                                                  });
                    response.Products = products.ToList();
                    response.IsSuccess = true;
                    return response;
                }
                */

                using (var context = new CampbellSupplyEntities())
                {
                    //response.RecordCount = context.Products.AsNoTracking().Where(p => p.Department.Name.Trim().Equals(request.Department, StringComparison.InvariantCultureIgnoreCase)
                    //                                                               && p.IsActive && !p.IsDeleted).Count();

                    response.RecordCount = context.Products.AsNoTracking().Where(p => (p.Name.Contains(request.SearchValue) && p.IsActive && !p.IsDeleted) || (p.Brand.Contains(request.SearchValue) && p.IsActive && !p.IsDeleted)).GroupBy(p => p.Name).Count();

                    var productNames = context.Products.AsNoTracking().Where(p => (p.Name.Contains(request.SearchValue) && p.IsActive && !p.IsDeleted) || (p.Brand.Contains(request.SearchValue) && p.IsActive && !p.IsDeleted))
                                                                      .GroupBy(p => p.Name)
                                                                      .OrderBy(p => p.Key).Skip(12 * (request.PageIndex - 1)).Take(12)
                                                                      .Select(p => p.Key).ToList();
                    foreach (var name in productNames)
                    {
                        var product = context.Products.FirstOrDefault(p => p.Name == name && p.IsActive && !p.IsDeleted);
                        response.Products.Add(new ProductsByPageModel
                        {
                            ProductId = product.Id,
                            Name = product.Name,
                            Description = product.DescriptionShort,
                            Manufacturer = product.Manufacturer.Name,
                            Brand = product.Brand,
                            Price = product.OverridePrice > 0 ? product.OverridePrice : product.PurchasePrice,
                            URL = product.ImageURL
                        });
                    }

                    response.IsSuccess = true;
                    return response;
                }


            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SerachProducts", Ex = ex });
                return new SearchProductsResponse { ErrorMessage = "Unable to search for products at this time." };
            }
        }

        public static SearchAdminProductsResponse SerachAdminProducts(SearchAdminProductsRequest request)
        {
            try
            {
                var response = new SearchAdminProductsResponse();
                if (string.IsNullOrEmpty(request.SearchValue)) return new SearchAdminProductsResponse { ErrorMessage = "Search value cannot be empty." };
                using (var context = new CampbellSupplyEntities())
                {
                    var products = context.Products.AsNoTracking().Where(p => p.Name.Contains(request.SearchValue)
                                                                           && (request.IncludeInactive ? (!p.IsActive || p.IsActive) : p.IsActive)
                                                                           && (request.IncludeDeleted ? (!p.IsDeleted || p.IsDeleted) : !p.IsDeleted))
                                                   .OrderBy(p => p.Name)
                                                   .Select(p => new ProductModel
                                                   {
                                                       Id = p.Id,
                                                       ManufacturerId = p.ManufacturerId ?? Guid.Empty,
                                                       ManufacturerName = p.Manufacturer == null ? string.Empty : p.Manufacturer.Name,
                                                       ManufacturerUPC = p.Manufacturer == null ? string.Empty : p.ManufacturerUPC,
                                                       DepartmentId = p.DepartmentId ?? Guid.Empty,
                                                       DepartmentName = p.Department == null ? string.Empty : p.Department.Name,
                                                       CategoryId = p.DepartmentId ?? Guid.Empty,
                                                       CategoryName = p.Category == null ? string.Empty : p.Category.Name,
                                                       SubCategoryId = p.SubCategoryId ?? Guid.Empty,
                                                       SubCategoryName = p.SubCategory == null ? string.Empty : p.SubCategory.Name,
                                                       Sku = p.ManufacturerUPC,
                                                       Name = p.Name,
                                                       DescriptionShort = p.DescriptionShort,
                                                       DescriptionLong = p.DescriptionLong,
                                                       PartNumber = p.PartNumber,
                                                       WebPartNumber = p.WebPartNumber,
                                                       Unkonwn = p.Unknown,
                                                       Color = p.Color == "-1" ? "" : p.Color,
                                                       Size = p.Size == "-1" ? "" : p.Size,
                                                       Brand = p.Brand,
                                                       ImageUrl = p.ImageURL,
                                                       Status = p.Status,
                                                       Market = p.Market,
                                                       Group = p.Group,
                                                       Mirror = p.Mirror,
                                                       Weight = p.Weight,
                                                       PurchasePrice = p.PurchasePrice,
                                                       OverridePrice = p.OverridePrice,
                                                       SalePrice = p.SalePrice,
                                                       ShippingPrice = p.ShippingPrice,
                                                       IsFeatured = p.IsFeatured,
                                                       IsOnSale = p.IsOnSale,
                                                       IsShippingValid = p.IsShippingValid,
                                                       IsActive = p.IsActive,
                                                       IsDeleted = p.IsDeleted,
                                                       DateOn = p.DateOn,
                                                       DateOff = p.DateOff,
                                                       DateActiveChanged = p.DateActiveChanged,
                                                       DateDeletedChanged = p.DateDeletedChanged,
                                                       DateCreated = p.DateCreated
                                                   });
                    if (products != null) response.Products = products.ToList();
                    response.IsSuccess = true;
                    return response;
                }

            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SerachAdminProducts", Ex = ex });
                return new SearchAdminProductsResponse { ErrorMessage = "Unable to search for products at this time." };
            }
        }

        public static SaveAdminProductResponse SaveAdminProduct(SaveAdminProductRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveAdminProductResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == request.Product.Id);
                    if (product == null) throw new ApplicationException("Could not find admin product " + request.Product.Id.ToString());
                    product.ManufacturerUPC = request.Product.ManufacturerUPC;
                    product.Name = request.Product.Name;
                    product.DescriptionShort = request.Product.DescriptionShort;
                    product.PartNumber = request.Product.PartNumber;
                    product.WebPartNumber = request.Product.WebPartNumber;
                    product.Sku = request.Product.ManufacturerUPC;
                    product.PurchasePrice = request.Product.PurchasePrice;
                    product.OverridePrice = request.Product.OverridePrice;
                    product.SalePrice = request.Product.SalePrice;
                    product.ShippingPrice = request.Product.ShippingPrice;
                    product.IsFeatured = request.Product.IsFeatured;
                    product.IsOnSale = request.Product.IsOnSale;
                    product.IsShippingValid = request.Product.IsShippingValid;
                    product.IsActive = request.Product.IsActive;
                    product.IsDeleted = request.Product.IsDeleted;
                    product.Unknown = request.Product.Unkonwn;
                    product.Color = request.Product.Color;
                    product.Size = request.Product.Size;
                    product.Brand = request.Product.Brand;
                    product.ImageURL = request.Product.ImageUrl;
                    product.Status = request.Product.Status;
                    product.Market = request.Product.Market;
                    product.Group = request.Product.Group;
                    product.Mirror = request.Product.Mirror;
                    product.Weight = request.Product.Weight;
                    product.DateOn = request.Product.DateOn;
                    product.DateOff = request.Product.DateOff;
                    product.DateEdited = now;
                    product.EditedBy = "AdminPage";
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SaveAdminProduct", Ex = ex });
                return new SaveAdminProductResponse { ErrorMessage = "Unable to save admin product." };
            }
        }


        public static GetTopRatedResponse GetTopRated(GetTopRatedRequest request)
        {
            try
            {
                var response = new GetTopRatedResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var topRated = context.RatedRevieweds.AsNoTracking().Where(r => r.IsActive && r.IsAuthorized)
                                                                        .GroupBy(r => new { r.Product })
                                                                        .Select(r => new TopRatedModel
                                                                        {
                                                                            ProductId = r.Key.Product.Id,
                                                                            Name = r.Key.Product.Name,
                                                                            Description = r.Key.Product.DescriptionShort,
                                                                            Manufacturer = r.Key.Product.Manufacturer.Name,
                                                                            Brand = r.Key.Product.Brand,
                                                                            Price = r.Key.Product.OverridePrice > 0 ? r.Key.Product.OverridePrice : r.Key.Product.PurchasePrice,
                                                                            URL = r.Key.Product.ImageURL,
                                                                            Rating = r.Average(t => t.Rating)
                                                                        }).ToList().OrderByDescending(r => r.Rating).Take(4);
                    response.TopRated = topRated.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetTopRated", Ex = ex });
                return new GetTopRatedResponse { ErrorMessage = "Unable to get top rated products." };
            }
        }

        public static GetRatedAndReviewedResponse GetRatedAndReviewed(GetRatedAndReviewedRequest request)
        {
            try
            {
                var response = new GetRatedAndReviewedResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var rr = context.RatedRevieweds.AsNoTracking().OrderByDescending(r => r.DateCreated)
                                                   .Select(r => new AdminRatedReviewedModel
                                                   {
                                                       Id = r.Id,
                                                       AccountId = r.AccountId,
                                                       FirstName = r.Account.FirstName,
                                                       LastName = r.Account.LastName,
                                                       ProductId = r.ProductId,
                                                       ProductUpc = r.Product.ManufacturerUPC,
                                                       ProductName = r.Product.Name,
                                                       Rating = r.Rating,
                                                       Review = r.Review,
                                                       IsActive = r.IsActive,
                                                       IsAuthorized = r.IsAuthorized,
                                                       DateActiveChanged = r.DateActiveChanged,
                                                       DateAuthorizedChanged = r.DateAuthorizedChanged,
                                                       DateCreated = r.DateCreated
                                                   });
                    if (rr != null) response.RatedAndReviewed = rr.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetRatedAndReviewed", Ex = ex });
                return new GetRatedAndReviewedResponse { ErrorMessage = "Unable to get ratings and reviews" };
            }
        }

        public static SaveRatedAndReviewedResponse SaveAdminRatedAndReviewed(SaveRatedAndReviewedRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveRatedAndReviewedResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var rr = context.RatedRevieweds.FirstOrDefault(r => r.Id == request.RatedAndReviewed.Id);
                    if (rr == null) throw new ApplicationException("Unable to find rating and review: " + request.RatedAndReviewed.Id.ToString());
                    if (rr.IsActive != request.RatedAndReviewed.IsActive)
                    {
                        rr.IsActive = request.RatedAndReviewed.IsActive;
                        rr.DateActiveChanged = now;
                    }
                    if (rr.IsAuthorized != request.RatedAndReviewed.IsAuthorized)
                    {
                        rr.IsAuthorized = request.RatedAndReviewed.IsAuthorized;
                        rr.DateAuthorizedChanged = now;
                    }
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetRatedAndReviewed", Ex = ex });
                return new SaveRatedAndReviewedResponse { ErrorMessage = "Unable to save rating and review" };
            }
        }

        public static SaveRatedAndReviewedResponse SaveRatedAndReviewed(SaveRatedAndReviewedRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveRatedAndReviewedResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var rr = context.RatedRevieweds.FirstOrDefault(r => r.ProductId == request.RatedAndReviewed.ProductId && r.AccountId == request.RatedAndReviewed.AccountId);
                    if (rr != null) return new SaveRatedAndReviewedResponse { ErrorMessage = "You have already rated and reviewed this product." };
                    rr = new RatedReviewed
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.RatedAndReviewed.ProductId,
                        AccountId = request.RatedAndReviewed.AccountId,
                        Rating = request.RatedAndReviewed.Rating,
                        Review = request.RatedAndReviewed.Review,
                        IsActive = false,
                        IsAuthorized = false,
                        DateActiveChanged = now,
                        DateAuthorizedChanged = now,
                        DateCreated = now
                    };
                    context.RatedRevieweds.Add(rr);
                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetRatedAndReviewed", Ex = ex });
                return new SaveRatedAndReviewedResponse { ErrorMessage = "Unable to save rating and review" };
            }
        }


        public static GetFeaturedResponse GetFeatured(GetFeaturedRequest request)
        {
            try
            {
                var response = new GetFeaturedResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var featured = context.Products.AsNoTracking().Where(p => p.IsFeatured && p.IsActive && !p.IsDeleted)
                                                    .Take(4)
                                                    .Select(p => new FeaturedModel
                                                    {
                                                        ProductId = p.Id,
                                                        Name = p.Name,
                                                        Description = p.DescriptionShort,
                                                        Manufacturer = p.Manufacturer.Name,
                                                        Brand = p.Brand,
                                                        Price = p.OverridePrice > 0 ? p.OverridePrice : p.PurchasePrice,
                                                        URL = p.ImageURL
                                                    });
                    response.Featured = featured.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetFeatured", Ex = ex });
                return new GetFeaturedResponse { ErrorMessage = "Unable to get featured products." };
            }
        }

        public static GetBestSellersResponse GetBestSellers(GetBestSellersRequest request)
        {
            try
            {
                var response = new GetBestSellersResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var best = context.OrderItems.AsNoTracking()
                                                 .GroupBy(b => new { b.ProductId, b.Quantity })
                                                 .OrderByDescending(b => b.Key.Quantity)
                                                 .Join(context.Products, b => b.Key.ProductId, p => p.Id, (b, p) => new { b, p })
                                                 .Take(4)
                                                 .Select(b => new BestSellerModel
                                                 {
                                                     ProductId = b.p.Id,
                                                     Name = b.p.Name,
                                                     Description = b.p.DescriptionShort,
                                                     Manufacturer = b.p.Manufacturer.Name,
                                                     Brand = b.p.Brand,
                                                     Price = b.p.OverridePrice > 0 ? b.p.OverridePrice : b.p.PurchasePrice,
                                                     URL = b.p.ImageURL
                                                 });
                    response.BestSellers = best.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetBestSellers", Ex = ex });
                return new GetBestSellersResponse { ErrorMessage = "Unable to get best sellers." };
            }
        }

        public static GetOnSaleResponse GetOnSale(GetOnSaleRequest request)
        {
            try
            {
                var response = new GetOnSaleResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var products = context.Products.AsNoTracking()
                                                   .Where(p => p.IsActive && p.IsOnSale)
                                                   .Take(4)
                                                   .Select(p => new OnSaleModel
                                                   {
                                                       ProductId = p.Id,
                                                       Name = p.Name,
                                                       Description = p.DescriptionShort,
                                                       Manufacturer = p.Manufacturer.Name,
                                                       Brand = p.Brand,
                                                       Price = p.OverridePrice > 0 ? p.OverridePrice : p.PurchasePrice,
                                                       URL = p.ImageURL
                                                   });
                    response.OnSale = products.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetOnSale", Ex = ex });
                return new GetOnSaleResponse { ErrorMessage = "Unable to get one sale items." };
            }
        }

        public static GetRecentlyViewedResponse GetRecentlyViewed(GetRecentlyViewedRequest request)
        {
            try
            {
                var response = new GetRecentlyViewedResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var recent = context.ProductViewHistories.AsNoTracking().OrderByDescending(p => p.NumberTimesViewed)
                                                             .Take(20)
                                                             .Select(p => new RecentlyViewedModel
                                                             {
                                                                 ProductId = p.ProductId,
                                                                 Name = p.Product.Name,
                                                                 Description = p.Product.DescriptionShort,
                                                                 Manufacturer = p.Product.Manufacturer.Name,
                                                                 Brand = p.Product.Brand,
                                                                 Price = p.Product.OverridePrice > 0 ? p.Product.OverridePrice : p.Product.PurchasePrice,
                                                                 URL = p.Product.ImageURL
                                                             });
                    response.RecentlyViewed = recent.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetRecentlyViewed", Ex = ex });
                return new GetRecentlyViewedResponse { ErrorMessage = "Unable to get recently viewed items." };
            }
        }

        public static SaveRecentlyViewedResponse SaveRecentlyViewed(SaveRecentlyViewedRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveRecentlyViewedResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var product = context.ProductViewHistories.FirstOrDefault(p => p.ProductId == request.ProductId);
                    if (product == null)
                    {
                        product = new ProductViewHistory { Id = Guid.NewGuid(), ProductId = request.ProductId, DateCreated = now };
                        context.ProductViewHistories.Add(product);
                    }
                    product.NumberTimesViewed += 1;
                    product.DateLastViewed = now;
                    context.SaveChanges();

                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "SaveRecentlyViewed", Ex = ex });
                return new SaveRecentlyViewedResponse { ErrorMessage = "Unable to save recently viewed item." };
            }
        }
    }
}