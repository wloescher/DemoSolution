using DemoUtilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        // Dependencies
        internal readonly ILogger _logger;
        internal readonly IConfiguration _configuration;
        internal readonly IServiceProvider _serviceProvider;
        internal readonly EmailUtility _emailUtility;

        // Configuration Values
        public readonly List<int> TestClientIds = new();
        public readonly List<int> TestClientUserIds = new();
        public readonly List<int> TestUserIds = new();
        public readonly List<int> TestWorkItemIds = new();
        public readonly string RegExPatternUrl;

        public bool UserIsAuthenticated => HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated;

        public string CurrentUrl => HttpContext != null ? HttpContext.Request.GetEncodedUrl() : string.Empty;

        public BaseController(ILogger logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            // Dependencies
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _emailUtility = new(configuration);

            // Configuration Values
            TestClientIds = (configuration.GetValue<string>("Demo:TestClientIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            TestClientUserIds = (configuration.GetValue<string>("Demo:TestClientUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            TestUserIds = (configuration.GetValue<string>("Demo:TestUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            TestWorkItemIds = (configuration.GetValue<string>("Demo:TestWorkItemIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            RegExPatternUrl = GetConfigurationKeyValue("RegExPatternUrl").Replace("&amp;", "&");
        }

        internal string GetConfigurationKeyValue(string key)
        {
            return ConfigurationUtility.GetConfigurationKeyValue(_configuration, key);
        }
    }
}
