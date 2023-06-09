using CampbellSupply.Common.Enums;
using CampbellSupply.Common.Helpers;
using CampbellSupply.Common.RequestAndResponses;
using CampbellSupply.Data;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.Services;
using System;
using System.Data.Entity;
using System.Linq;

namespace CampbellSupply.DataLayer.DataServices
{
    public static class MenuService
    {
        public static GetManufacturersResponse GetManufacturers(GetManufacturersRequest request)
        {
            try
            {
                var response = new GetManufacturersResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var manufactures = context.Manufacturers.Where(m => m.IsActive).AsNoTracking()
                                                            .Select(m => new ManufacturerModel
                                                            {
                                                                Id = m.Id,
                                                                Name = m.Name,
                                                                IsActive = m.IsActive,
                                                                DateActiveChanged = m.DateActiveChanged,
                                                                DateCreated = m.DateCreated
                                                            });
                    response.Manufacturers = manufactures.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetManufacturers", Ex = ex });
                return new GetManufacturersResponse { ErrorMessage = "Unable to get manufacturers." };
            }
        }

        public static SaveManufacturerResponse SaveManufacturer(SaveManufacturerRequest request)
        {
            try
            {
                var now = DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new SaveManufacturerResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    if (request.Manufacturer.Id == Guid.Empty) throw new ApplicationException("Manufacturer id cannot be an empty guid.");
                    var manufacturer = context.Manufacturers.FirstOrDefault(m => m.Id == request.Manufacturer.Id);
                    if (manufacturer == null)
                    {
                        manufacturer = new Manufacturer { Id = Guid.NewGuid(), IsActive = true, DateActiveChanged = now, DateCreated = now };
                        context.Manufacturers.Add(manufacturer);
                    }
                    else
                    {
                        if (manufacturer.IsActive != request.Manufacturer.IsActive)
                        {
                            manufacturer.IsActive = request.Manufacturer.IsActive;
                            manufacturer.DateActiveChanged = now;
                        }
                    }
                    manufacturer.Name = request.Manufacturer.Name;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetManufacturers", Ex = ex });
                return new SaveManufacturerResponse { ErrorMessage = "Unable to save manufacturer." };
            }
        }


        public static GetCategoriesResponse GetCategories(GetCategoriesRequest request)
        {
            try
            {
                var response = new GetCategoriesResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var categories = context.Products.Where(p => p.IsActive && p.Department.Name.Trim().Equals(request.Department, StringComparison.InvariantCultureIgnoreCase))
                                                     .GroupBy(p => new { p.Category })
                                                     .AsNoTracking()
                                                     .Select(p => new CategoryModel
                                                     {
                                                         Id = p.Key.Category.Id,
                                                         Name = p.Key.Category.Name,
                                                         Department = request.Department,
                                                         Description = p.Key.Category.Description,
                                                         IsActive = p.Key.Category.IsActive,
                                                         DateActiveChanged = p.Key.Category.DateActiveChanged,
                                                         DateCreated = p.Key.Category.DateCreated
                                                     });
                    response.Categories = categories.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetCategories", Ex = ex });
                return new GetCategoriesResponse { ErrorMessage = "Unable to get categories." };
            }
        }

        public static GetDepartmentsResponse GetDepartments(GetDepartmentsRequest request)
        {
            try
            {
                var response = new GetDepartmentsResponse();
                using (var context = new CampbellSupplyEntities())
                {
                    var departments = context.Departments.Where(d => d.IsActive)
                                                        .AsNoTracking()
                                                        .OrderBy(d => d.Name)
                                                        .Select(d => new DepartmentModel
                                                        {
                                                            Id = d.Id,
                                                            Name = d.Name,
                                                            Description = d.Description,
                                                            IsActive = d.IsActive,
                                                            DateActiveChanged = d.DateActiveChanged,
                                                            DateCreated = d.DateCreated,
                                                        });
                    response.Departments = departments.ToList();
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { Class = "GetDepartments", Ex = ex });
                return new GetDepartmentsResponse { ErrorMessage = "Unable to get departments." };
            }
        }
    }
}