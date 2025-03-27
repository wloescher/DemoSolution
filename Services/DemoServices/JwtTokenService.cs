using DemoModels;
using DemoRepository.Entities;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoServices
{
    public class JwtTokenService(IConfiguration configuration, DemoSqlContext dbContext, IMemoryCache memoryCache, IServiceProvider serviceProvider)
        : DbContextService(configuration, dbContext, memoryCache, serviceProvider), IJwtTokenService
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
