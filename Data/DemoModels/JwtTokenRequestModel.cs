namespace DemoModels
{
    public class JwtTokenRequestModel
    {
        public required string EmailAddress { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public int ClientId { get; set; }
    }
}
