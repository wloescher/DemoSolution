using DemoRepository.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace DemoServices.BaseClasses
{
    public abstract class DbContextService : ConfigurationService
    {
        internal readonly DemoSqlContext _dbContext;
        internal readonly IMemoryCache _memoryCache;
        internal readonly MemoryCacheEntryOptions _cacheOptions;
        internal readonly IServiceProvider _serviceProvider;

        internal readonly string _dbConnectionString = string.Empty;

        public DemoSqlContext DbContext => _dbContext;
        public IMemoryCache MemoryCache => _memoryCache;

        public DbContextService(IConfiguration configuration, DemoSqlContext dbContext, IMemoryCache memoryCache, IServiceProvider serviceProvider)
            : base(configuration)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;

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
