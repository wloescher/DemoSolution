using DemoModels;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoServices
{
    public class JwtTokenService(IServiceProvider serviceProvider, IConfiguration configuration)
        : ServiceProviderService(serviceProvider, configuration), IJwtTokenService
    {
        public string GenerateToken(string emailAddress, string password)
        {
            UserModel? user;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                user = userService.GetUser(emailAddress, password);
            }

            if (user == null)
            {
                return string.Empty;
            }

            return JwtTokenUtility.GenerateToken(_configuration, user.UserId, user.EmailAddress, user.FirstName ?? string.Empty, user.LastName ?? string.Empty);
        }
    }
}
