using CampbellSupply.Common.Enums;
using System;
using System.Runtime.Serialization;

namespace CampbellSupply.DataLayer.DataContracts.Models
{
    [DataContract]
    public class WeeklyAdModel : BaseModel
    {
        public WeeklyAdModel()
        {
            this.Title = string.Empty;
            this.FileName = string.Empty;
            this.File = null;
            this.FileStorageType = StorageType.Unknown;
            this.FileStoragePath = string.Empty;
            this.Image = null;
            this.ImageName = string.Empty;
            this.ImageStorageType = StorageType.Unknown;
            this.ImageStoragePath = string.Empty;
            this.EffectiveDate = DateTime.MinValue;
            this.EndDate = null;
        }

        [DataMember(IsRequired = true)]
        public string Title { get; set; }
        [DataMember(IsRequired = true)]
        public string FileName { get; set; }
        [DataMember(IsRequired = true)]
        public byte[] File { get; set; }
        [DataMember(IsRequired = true)]
        public StorageType FileStorageType { get; set; }
        [DataMember(IsRequired = true)]
        public string FileStoragePath { get; set; }
        [DataMember(IsRequired = true)]
        public byte[] Image { get; set; }
        [DataMember(IsRequired = true)]
        public string ImageName { get; set; }
        [DataMember(IsRequired = true)]
        public StorageType ImageStorageType { get; set; }
        [DataMember(IsRequired = true)]
        public string ImageStoragePath { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime EffectiveDate { get; set; }
        [DataMember(IsRequired = true)]
        public DateTime? EndDate { get; set; }

    }
}