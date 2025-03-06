using static DemoModels.Enums;

namespace DemoModels
{
    public class ClientModel
    {
        public int ClientId { get; set; }
        public Guid ClientGuid { get; set; }
        public ClientType Type { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
