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
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

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
