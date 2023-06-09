using System;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.Models
{
    [DataContract]
    public class RegisterModel
    {
        public RegisterModel()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = string.Empty;
            this.PhoneNumber = string.Empty;
            this.Password = string.Empty;
        }

        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public string PhoneNumber { get; set; }
        [DataMember(IsRequired = true)]
        public string Password { get; set; }     
    }

    [DataContract]
    public class AccountModel : ActiveModel
    {
        public AccountModel()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Username = string.Empty;
            this.Email = string.Empty;
            this.IsRegistered = false;
            this.IsAdmin = false;
            this.DateRegisteredChanged = DateTime.MinValue;
            this.DateLastLogin = DateTime.MinValue;
            this.DateCreated = DateTime.MinValue;
        }

        public string FirstName { get; set; }
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public string Username { get; set; }
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsRegistered { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsAdmin { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateRegisteredChanged { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateLastLogin { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime DateCreated { get; set; }
    }
}