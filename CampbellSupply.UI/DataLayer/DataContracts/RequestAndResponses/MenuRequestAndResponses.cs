using CampbellSupply.DataLayer.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.RequestAndResponses
{
    public class GetManufacturersRequest : BaseRequest { }

    public class GetManufacturersResponse : BaseResponse
    {
        public GetManufacturersResponse()
        {
            this.Manufacturers = new List<ManufacturerModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ManufacturerModel> Manufacturers { get; set; }

    }

    public class SaveManufacturerRequest : BaseRequest
    {
        public SaveManufacturerRequest()
        {
            this.Manufacturer = new ManufacturerModel();
        }

        [DataMember(IsRequired = true)]
        public ManufacturerModel Manufacturer { get; set; }

    }

    public class SaveManufacturerResponse : BaseResponse { }


    public class GetCategoriesRequest : BaseRequest
    {
        public GetCategoriesRequest()
        {
            this.Department = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Department { get; set; }

    }

    public class GetCategoriesResponse : BaseResponse
    {
        public GetCategoriesResponse()
        {
            this.Categories = new List<CategoryModel>();
        }

        [DataMember(IsRequired = true)]
        public List<CategoryModel> Categories { get; set; }
    }


    public class GetDepartmentsRequest : BaseRequest { }

    public class GetDepartmentsResponse : BaseResponse
    {
        public GetDepartmentsResponse()
        {
            this.Departments = new List<DepartmentModel>();
        }

        [DataMember(IsRequired = true)]
        public List<DepartmentModel> Departments { get; set; }
    }
}