﻿using DemoRepository.Entities;
using DemoServices;
using DemoServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoTests.BaseClasses
{
    [TestClass]
    public abstract class ServiceTestBase
    {
        internal static ServiceProvider? _serviceProvider;

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
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x => x.UseCompatibilityLevel(100));
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
