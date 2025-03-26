namespace DemoServices.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string emailAddress, string password);
    }
}
