using System.ComponentModel.DataAnnotations;
using static DemoModels.Enums;

namespace DemoModels
{
    public class ClientModel
    {
        public int ClientId { get; set; }
        public Guid ClientGuid { get; set; }
        public ClientType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? AddressLine1 { get; set; }
        [MaxLength(255)]
        public string? AddressLine2 { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(50)]
        public string? Region { get; set; }
        [MaxLength(10)]
        public string? PostalCode { get; set; }
        [MaxLength(50)]
        public string? Country { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(150)]
        public string? Url { get; set; }
        public DateTime CreatedDate { get; set; } = new DateTime(1900, 1, 1);
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = new DateTime(1900, 1, 1);
        public string? ModifiedBy { get; set; }
    }
}
