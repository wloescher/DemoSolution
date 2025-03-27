﻿using DemoRepository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace DemoServices.BaseClasses
{
    public abstract class DbContextService : ServiceProviderService
    {
        internal readonly IDbContextFactory<DemoSqlContext> _dbContextFactory;
        internal readonly IMemoryCache _memoryCache;
        internal readonly MemoryCacheEntryOptions _cacheOptions;
        internal readonly string _dbConnectionString = string.Empty;

        public IMemoryCache MemoryCache => _memoryCache;

        public DbContextService(IDbContextFactory<DemoSqlContext> dbContextFactory, IMemoryCache memoryCache, IServiceProvider serviceProvider, IConfiguration configuration)
            : base(serviceProvider, configuration)
        {
            _dbContextFactory = dbContextFactory;
            _memoryCache = memoryCache;

            var cacheSeconds = Convert.ToInt32(GetConfigurationKeyValue("CacheSeconds"));
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(cacheSeconds),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromSeconds(cacheSeconds)
            };

            _dbConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }
    }
}
