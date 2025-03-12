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
        public string AddressLine1 { get; set; } = string.Empty;
        [MaxLength(255)]
        public string AddressLine2 { get; set; } = string.Empty;
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Region { get; set; } = string.Empty;
        [MaxLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;
        [MaxLength(150)]
        public string Url { get; set; } = string.Empty;
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
