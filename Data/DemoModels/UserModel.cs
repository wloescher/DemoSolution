using System.ComponentModel.DataAnnotations;
using static DemoModels.Enums;

namespace DemoModels
{
    public class UserModel
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public UserType Type { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? MiddleName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
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

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(MiddleName))
                {
                    return $"{FirstName} {LastName}";
                }
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }
    }
}
