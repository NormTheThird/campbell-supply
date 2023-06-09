using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.Models
{
    [DataContract]
    public class ManufacturerModel : MenuModel { }

    [DataContract]
    public class DepartmentModel : MenuOrderModel { }

    [DataContract]
    public class CategoryModel : MenuModel
    {
        public CategoryModel()
        {
            this.Department = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string Department { get; set; }
    }

    [DataContract]
    public class SubCategoryModel : MenuModel { }
}