using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.Models;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Data;
using System;
using System.Linq;

namespace CampbellSupply.Services
{
    public static class ProductService
    {
        public static GetProductsResponse GetProducts(GetProductsRequest request)
        {
            try
            {
                var response = new GetProductsResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var products = context.Products.AsNoTracking()
                                                   .Select(p => new ProductModel
                                                   {
                                                       ManufacturerUPC = p.Manufacturer == null ? string.Empty : p.ManufacturerUPC,
                                                       DescriptionShort = p.DescriptionShort,
                                                       PartNumber = p.PartNumber,
                                                       ManufacturerName = p.Manufacturer == null ? string.Empty : p.Manufacturer.Name,
                                                       Unkonwn = p.Unknown,
                                                       PurchasePrice = p.PurchasePrice,
                                                       ImageUrl = p.ImageURL,
                                                       Status = p.Status,
                                                       IsActive = p.IsActive,
                                                       IsDeleted = p.IsDeleted,
                                                       WebPartNumber = p.WebPartNumber,
                                                       Sku = p.Sku,
                                                       Name = p.Name,
                                                       DepartmentName = p.Department == null ? string.Empty : p.Department.Name,
                                                       CategoryName = p.Category == null ? string.Empty : p.Category.Name,
                                                       SubCategoryName = p.SubCategory == null ? string.Empty : p.SubCategory.Name,
                                                       DescriptionLong = p.DescriptionLong,
                                                       OverridePrice = p.OverridePrice,
                                                       ShippingPrice = p.ShippingPrice,
                                                       IsShippingValid = p.IsShippingValid,
                                                       Color = p.Color ?? "",
                                                       Size = p.Size ?? "",
                                                       Weight = p.Weight,
                                                       DateOn = p.DateOn,
                                                       DateOff = p.DateOff,
                                                       IsFeatured = p.IsFeatured,
                                                       Brand = p.Brand,
                                                       Market = p.Market,
                                                       Group = p.Group,
                                                       Mirror = p.Mirror,
                                                   });
                    if (products != null) response.Products = products.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetProducts", Ex = ex });
                return new GetProductsResponse { ErrorMessage = ex.Message };
            }
        }

        public static UpdateProductResponse UpdateProduct(UpdateProductRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new UpdateProductResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var newManufactureId = GetManufactureId(request.Product.ManufacturerName, now, context);
                    var newDepartmentId = GetDepartmentId(request.Product.DepartmentName, now, context);
                    var newCategoryId = GetCategoryId(request.Product.CategoryName, now, context);
                    var newSubCategoryId = GetSubCategoryId(request.Product.SubCategoryName, now, context);
                    var visibilityId = Convert.ToInt32(ConvertOnOff(CheckValue(request.Product.VisibilityId, "Off")));
                    var product = context.Products.FirstOrDefault(p => p.ManufacturerUPC.Equals(request.Product.ManufacturerUPC, StringComparison.CurrentCultureIgnoreCase));
                    if (product == null)
                    {
                        product = new Product
                        {
                            Id = Guid.NewGuid(),
                            OldId = 0,
                            ManufacturerUPC = CheckValue(request.Product.ManufacturerUPC, ""),
                            DateCreated = now,
                            ManufacturerId = newManufactureId,
                            DepartmentId = newDepartmentId,
                            CategoryId = newCategoryId,
                            SubCategoryId = newSubCategoryId,
                            Sku = CheckValue(request.Product.Sku, ""),
                            Name = CheckValue(request.Product.Name, ""),
                            DescriptionShort = CheckValue(request.Product.DescriptionShort, ""),
                            DescriptionLong = CheckValue(request.Product.DescriptionLong, ""),
                            PartNumber = CheckValue(request.Product.PartNumber, ""),
                            WebPartNumber = CheckValue(request.Product.WebPartNumber, ""),
                            Unknown = CheckValue(request.Product.Unknown, ""),
                            Color = CheckValue(request.Product.Color, "-1"),
                            Size = CheckValue(request.Product.Size, "-1"),
                            Brand = CheckValue(request.Product.Brand, ""),
                            ImageURL = CheckValue(request.Product.ImageUrl, ""),
                            Status = CheckValue(request.Product.Status, ""),
                            Market = CheckValue(request.Product.Market, ""),
                            Group = CheckValue(request.Product.Group, ""),
                            Mirror = CheckValue(request.Product.Mirror, ""),
                            Weight = Convert.ToDecimal(CheckValue(request.Product.Weight, "0")),
                            PurchasePrice = Convert.ToDecimal(CheckValue(request.Product.PurchasePrice, "0")),
                            OverridePrice = Convert.ToDecimal(CheckValue(request.Product.OverridePrice, "0")),
                            SalePrice = 0.0m,
                            ShippingPrice = Convert.ToDecimal(CheckValue(request.Product.ShippingPrice, "0")),
                            vsibilityID = visibilityId,
                            IsFeatured = Convert.ToBoolean(CheckValue(request.Product.IsFeatured, "false")),
                            IsOnSale = false,
                            IsShippingValid = Convert.ToBoolean(CheckValue(request.Product.IsShippingValid, "false")),
                            IsActive = visibilityId == 1 ? true : false,
                            IsDeleted = visibilityId == 2 ? true : false,
                            DateOn = CheckDate(CheckValue(request.Product.DateOn, "")),
                            DateOff = CheckDate(CheckValue(request.Product.DateOff, "")),
                            DateActiveChanged = now,
                            DateDeletedChanged = now,

                            EditedBy = "Upload",
                            DateEdited = now
                        };
                        context.Products.Add(product);
                    }
                    else
                    {
                        visibilityId = string.IsNullOrEmpty(request.Product.VisibilityId) ? product.vsibilityID : visibilityId;
                        var newIsActive = visibilityId == 1 ? true : false;
                        var newIsDeleted = visibilityId == 2 ? true : false;

                        product.ManufacturerId = CheckGuid(request.Product.ManufacturerName, newManufactureId, product.ManufacturerId);
                        product.DepartmentId = CheckGuid(request.Product.DepartmentName, newDepartmentId, product.DepartmentId);
                        product.CategoryId = CheckGuid(request.Product.CategoryName, newCategoryId, product.CategoryId);
                        product.SubCategoryId = CheckGuid(request.Product.SubCategoryName, newSubCategoryId, product.SubCategoryId);
                        product.Sku = GetValue<string>(request.Product.Sku, product.Sku);
                        product.Name = GetValue<string>(request.Product.Name, product.Name);
                        product.DescriptionShort = GetValue<string>(request.Product.DescriptionShort, product.DescriptionShort);
                        product.DescriptionLong = GetValue<string>(request.Product.DescriptionLong, product.DescriptionLong);
                        product.PartNumber = GetValue<string>(request.Product.PartNumber, product.PartNumber);
                        product.WebPartNumber = GetValue<string>(request.Product.WebPartNumber, product.WebPartNumber);
                        product.Unknown = GetValue<string>(request.Product.Unknown, product.Unknown);
                        product.Color = GetValue<string>(request.Product.Color, product.Color);
                        product.Size = GetValue<string>(request.Product.Size, product.Size);
                        product.Brand = GetValue<string>(request.Product.Brand, product.Brand);
                        product.ImageURL = GetValue<string>(request.Product.ImageUrl, product.ImageURL);
                        product.Status = GetValue<string>(request.Product.Status, product.Status);
                        product.Market = GetValue<string>(request.Product.Market, product.Market);
                        product.Group = GetValue<string>(request.Product.Group, product.Group);
                        product.Mirror = GetValue<string>(request.Product.Mirror, product.Mirror);
                        product.Weight = GetValue<decimal>(request.Product.Weight, product.Weight);
                        product.PurchasePrice = GetValue<decimal>(request.Product.PurchasePrice, product.PurchasePrice);
                        product.OverridePrice = GetValue<decimal>(request.Product.OverridePrice, product.OverridePrice);
                        //product.SalePrice = // Not in file
                        product.ShippingPrice = GetValue<decimal>(request.Product.ShippingPrice, product.ShippingPrice);
                        product.vsibilityID = visibilityId;
                        product.IsFeatured = GetValue<bool>(request.Product.IsFeatured, product.IsFeatured, "false");
                        //product.IsOnSale = // Not in file
                        product.IsShippingValid = GetValue<bool>(request.Product.IsShippingValid, product.IsShippingValid, "false");
                        product.DateOn = CheckDate(request.Product.DateOn, product.DateOn);
                        product.DateOff = CheckDate(request.Product.DateOff, product.DateOff);

                        product.EditedBy = "Upload";
                        product.DateEdited = now;

                        if (product.IsActive != newIsActive)
                        {
                            product.IsActive = newIsActive;
                            product.DateActiveChanged = now;
                        }

                        if (product.IsDeleted != newIsDeleted)
                        {
                            product.IsDeleted = newIsDeleted;
                            product.DateDeletedChanged = now;
                        }
                    }

                    context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "UpdateProduct", Ex = ex });
                return new UpdateProductResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Checks to see if a manufacure name exists and if not then adds the new one.
        /// </summary>
        /// <param name="name">The manufacturer name to get the id</param>
        /// <param name="now">The datetime of now</param>
        /// <param name="context">the database context being used</param>
        /// <returns></returns>
        public static Guid? GetManufactureId(string name, DateTime now, CampbellSupplyEntities context)
        {
            try
            {
                Guid? manufactureId = null;
                if (!string.IsNullOrEmpty(name) && name != "^" && name != " ")
                {
                    var manufacture = context.Manufacturers.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    if (manufacture == null)
                    {
                        manufacture = new Manufacturer
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            Description = name,
                            IsActive = true,
                            DateActiveChanged = now,
                            DateCreated = now
                        };
                        context.Manufacturers.Add(manufacture);
                    }
                    manufactureId = manufacture.Id;
                }
                return manufactureId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks to see if a department name exists and if not then adds the new one.
        /// </summary>
        /// <param name="name">The department name to get the id</param>
        /// <param name="now">The datetime of now</param>
        /// <param name="context">the database context being used</param>
        /// <returns></returns>
        public static Guid? GetDepartmentId(string name, DateTime now, CampbellSupplyEntities context)
        {
            try
            {
                Guid? departmentId = null;
                if (!string.IsNullOrEmpty(name) && name != "^" && name != " ")
                {
                    var department = context.Departments.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    if (department == null)
                    {
                        var orderNum = context.Departments.AsNoTracking().DefaultIfEmpty().Max(d => d.Order);
                        department = new Department
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            Description = name + " Department",
                            Order = orderNum + 1,
                            IsActive = true,
                            DateActiveChanged = now,
                            DateCreated = now
                        };
                        context.Departments.Add(department);
                    }
                    departmentId = department.Id;
                }
                return departmentId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks to see if a category name exists and if not then adds the new one.
        /// </summary>
        /// <param name="name">The category name to get the id</param>
        /// <param name="now">The datetime of now</param>
        /// <param name="context">the database context being used</param>
        /// <returns></returns>
        public static Guid? GetCategoryId(string name, DateTime now, CampbellSupplyEntities context)
        {
            try
            {
                Guid? categoryId = null;
                if (!string.IsNullOrEmpty(name) && name != "^" && name != " ")
                {
                    var category = context.Categories.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    if (category == null)
                    {
                        category = new Category
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            Description = name,
                            IsActive = true,
                            DateActiveChanged = now,
                            DateCreated = now
                        };
                        context.Categories.Add(category);
                    }
                    categoryId = category.Id;
                }
                return categoryId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks to see if a sub category name exists and if not then adds the new one.
        /// </summary>
        /// <param name="name">The sub category name to get the id</param>
        /// <param name="now">The datetime of now</param>
        /// <param name="context">the database context being used</param>
        /// <returns></returns>
        public static Guid? GetSubCategoryId(string name, DateTime now, CampbellSupplyEntities context)
        {
            try
            {
                Guid? subCategoryId = null;
                if (!string.IsNullOrEmpty(name) && name != "^" && name != " ")
                {
                    var subCategory = context.SubCategories.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    if (subCategory == null)
                    {
                        subCategory = new SubCategory
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            Description = name,
                            IsActive = true,
                            DateActiveChanged = now,
                            DateCreated = now
                        };
                        context.SubCategories.Add(subCategory);
                    }
                    subCategoryId = subCategory.Id;
                }
                return subCategoryId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the value of the cell being updated
        /// </summary>
        /// <param name="_value">The value to check for update</param>
        /// <param name="_default">The default value to use</param>
        /// <returns>The value to use for the updated cell</returns>
        private static string CheckValue(string _value, string _default, string _replace = "")
        {
            try
            {
                // Check the value of the cell
                string returnString = "";
                if (_value == " ")
                    returnString = " ";
                if (_value.Contains("'"))
                {
                    _value.Replace("'", "''");
                }
                else if (_value.Contains("’"))
                {
                    _value.Replace("’", "'");
                }

                if (_value != " ")
                    if (string.IsNullOrEmpty(_replace))
                        returnString = string.IsNullOrEmpty(Convert.ToString(_value)) ? _default : Convert.ToString(_value).Trim();
                    else
                        returnString = string.IsNullOrEmpty(Convert.ToString(_value)) ? _default : Convert.ToString(_value).Trim().Replace(_replace.Trim(), "");

                // Check if replace is single quote
                if (_replace.Trim().Equals("'"))
                {
                    char _char1 = (char)39;
                    char _char2 = (char)8217;
                    returnString = returnString.Replace(_char1.ToString(), "");
                    returnString = returnString.Replace(_char2.ToString(), "");
                }

                // return the new value
                return returnString;
            }
            catch (Exception)
            {
                return _default.Trim();
            }
        }

        /// <summary>
        /// Converts the string value to int value
        /// </summary>
        /// <param name="value">The value to convert to on off</param>
        /// <returns></returns>
        public static int ConvertOnOff(string value)
        {
            try
            {
                if (value.ToUpper().Trim().Equals("ON"))
                    return 1;
                else if (value.ToUpper().Trim().Equals("OFF") || value.ToUpper().Trim().Equals("^"))
                    return 2;
                else if (value.ToUpper().Trim().Equals("DELETED"))
                    return 3;
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        /// <summary>
        /// Checks if the datetime string can be converted to a datetime
        /// </summary>
        /// <param name="value">The string to convert to a datetime</param>
        /// <returns></returns>
        public static DateTime? CheckDate(string newValue, DateTime? oldValue = null)
        {
            try
            {
                if (oldValue == null)
                {
                    if (string.IsNullOrEmpty(newValue) || newValue == "^" || newValue == " ") return null;
                    else return Convert.ToDateTime(newValue);
                }
                else
                {
                    if (newValue == " " || newValue == "^") return null;
                    else if (newValue == "") return oldValue;
                    else return Convert.ToDateTime(newValue);
                }

            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Checks if the guid has been changed for the id.
        /// </summary>
        /// <returns></returns>
        public static Guid? CheckGuid(string newValue, Guid? newId, Guid? oldId = null)
        {
            try
            {
                if (newValue == " " || newValue == "^") return null;
                else if (newValue == "") return oldId;
                else return newId;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Gets the value for the gereric type T
        /// </summary>
        /// <typeparam name="T">The type to get the value for</typeparam>
        /// <param name="cellValue"></param>
        /// <param name="replaceValue"></param>
        /// <returns></returns>
        private static T GetValue<T>(string newValue, T oldValue, string replaceValue = "")
        {
            // For every item
            // "" means keep same
            // " ", "^" means clear it out
            // none of the about then use new value

            if (newValue == " " || newValue == "^")
                if (typeof(T) == typeof(string))
                    return (T)Convert.ChangeType(string.Empty, typeof(T));
                else
                    return (T)Convert.ChangeType((T)GetDefault(typeof(T)), typeof(T));
            else if (newValue == "")
                return oldValue;
            else
                return (T)Convert.ChangeType(CheckValue(newValue, "", replaceValue), typeof(T));

            //if (typeof(T) == typeof(string))
            //{
            //    if (newValue == " " || newValue == "^")
            //        return (T)Convert.ChangeType(string.Empty, typeof(T));
            //    else if (newValue == "")
            //        return oldValue;
            //    else
            //        return (T)Convert.ChangeType(CheckValue(newValue, "", replaceValue), typeof(T));
            //}

            //if (typeof(T) == typeof(decimal))
            //{
            //    if (newValue == " " || newValue == "^")
            //        return (T)Convert.ChangeType(0.0m, typeof(T));
            //    else if (newValue == "")
            //        return oldValue;
            //    else
            //        return (T)Convert.ChangeType(CheckValue(Convert.ToString(newValue ?? ""), "0"), typeof(T));
            //}

            //if (typeof(T) == typeof(bool))
            //{
            //    if (newValue == " " || newValue == "^")
            //        return (T)Convert.ChangeType(false, typeof(T));
            //    else if (newValue == "")
            //        return oldValue;
            //    else
            //        return (T)Convert.ChangeType(CheckValue(Convert.ToString(newValue ?? ""), "false"), typeof(T));
            //}

            //return (T)GetDefault(typeof(T));
        }

        /// <summary>
        /// Gets the defualt value for the given type
        /// </summary>
        /// <param name="type">The type to get the defualt value for</param>
        /// <returns></returns>
        public static object GetDefault(Type type)
        {
            if (type.IsValueType) return Activator.CreateInstance(type);
            else return null;
        }
    }
}