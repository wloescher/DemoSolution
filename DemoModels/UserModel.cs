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
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string MiddleName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Region { get; set; } = string.Empty;
        [MaxLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

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
