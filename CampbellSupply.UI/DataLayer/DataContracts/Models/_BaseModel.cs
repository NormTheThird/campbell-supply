using System;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.Models
{
    [DataContract]
    public class BaseModel
    {
        public BaseModel()
        {
            this.Id = Guid.Empty;
        }

        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class ActiveModel : BaseModel
    {
        public ActiveModel()
        {
            this.IsActive = false;
            this.DateActiveChanged = DateTime.MinValue;
        }

        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateActiveChanged { get; set; }
    }

    [DataContract]
    public class MenuModel : ActiveModel
    {
        public MenuModel()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.DateCreated = DateTime.MinValue;
        }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateCreated { get; set; }
    }

    [DataContract]
    public class MenuOrderModel : MenuModel
    {
        public MenuOrderModel()
        {
            this.Order = 0;
        }

        [DataMember(IsRequired = true)]
        public int Order { get; set; }
    }
}