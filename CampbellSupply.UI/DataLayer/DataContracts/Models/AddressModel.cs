using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.Models
{
    [DataContract]
    public class AddressModel : BaseModel
    {
        public AddressModel()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.PhoneNumber = string.Empty;
            this.Address1 = string.Empty;
            this.Address2 = string.Empty;
            this.City = string.Empty;
            this.State = string.Empty;
            this.ZipCode = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public string PhoneNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string Address1 { get; set; }
        [DataMember(IsRequired = true)]
        public string Address2 { get; set; }
        [DataMember(IsRequired = true)]
        public string City { get; set; }
        [DataMember(IsRequired = true)]
        public string State { get; set; }
        [DataMember(IsRequired = true)]
        public string ZipCode { get; set; }
    }
}