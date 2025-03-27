using DemoRepository.Entities;
using DemoServices;
using DemoServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;

namespace DemoTests.BaseClasses
{
    [TestClass]
    public abstract class TestBase
    {
        // Dependencies
        internal static IConfiguration? _configuration;
        internal readonly DemoSqlContext _dbContext;
        internal static ServiceProvider? _serviceProvider;
        internal static Mock<HttpContext> _mockHttpContext = new();

        // Configuration Values
        internal readonly List<int> _testClientIds = new();
        internal readonly List<int> _testClientUserIds = new();
        internal readonly List<int> _testUserIds = new();
        internal readonly List<int> _testWorkItemIds = new();
        internal readonly List<int> _testWorkItemUserIds = new();

        protected TestBase()
        {
            // Configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            _configuration = configurationBuilder.Build();

            // DbContext
            var optionsBuilder = new DbContextOptionsBuilder<DemoSqlContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            _dbContext = new DemoSqlContext(optionsBuilder.Options);

            // Configuration Values
            _testClientIds = (_configuration.GetValue<string>("Demo:TestClientIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            _testClientUserIds = (_configuration.GetValue<string>("Demo:TestClientUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            _testUserIds = (_configuration.GetValue<string>("Demo:TestUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            _testWorkItemIds = (_configuration.GetValue<string>("Demo:TestWorkItemIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            _testWorkItemUserIds = (_configuration.GetValue<string>("Demo:TestWorkItemUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();

            // Mock HttpContext
            _mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity("Admin")));
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var serviceCollection = new ServiceCollection();

            // Add configuration
            serviceCollection.AddSingleton<IConfiguration>(sp =>
            {
                var configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddJsonFile("appsettings.json");
                return configurationBuilder.Build();
            });

            // Add db context
            serviceCollection.AddTransient(static sp =>
            {
                var configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddJsonFile("appsettings.json");
                var configuration = configurationBuilder.Build();
                var optionsBuilder = new DbContextOptionsBuilder<DemoSqlContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                return new DemoSqlContext(optionsBuilder.Options);
            });

            serviceCollection.AddMemoryCache();
            serviceCollection.AddHttpClient();

            // Add services
            serviceCollection.AddSingleton<IAuditService, AuditService>();
            serviceCollection.AddSingleton<IClientService, ClientService>();
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<IWorkItemService, WorkItemService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _serviceProvider?.Dispose();
        }
    }
}
