using DemoModels;
using DemoServices.Interfaces;
using DemoUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    // Be careful here! The decoration below means anyone can get to anything in this class.
    [AllowAnonymous]
    [Route("test")]
    public class TestController : ControllerBase
    {
        // Dependencies
        internal readonly IConfiguration _configuration;
        internal readonly IServiceProvider _serviceProvider;
        internal readonly EmailUtility _emailUtility;

        // Configuration Values
        public readonly List<int> TestClientIds = new();
        public readonly List<int> TestClientUserIds = new();
        public readonly List<int> TestUserIds = new();
        public readonly List<int> TestWorkItemIds = new();
        public readonly string RegExPatternUrl;

        public TestController(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            // Dependencies
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

        #region Public Methods

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            UserModel? model;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                model = userService.GetUser(TestUserIds.First());
            }
            return Ok(model);
        }

        [HttpGet("client")]
        public IActionResult GetClient()
        {
            var clientId = TestClientIds.First();

            ClientModel? model;
            using (var scope = _serviceProvider.CreateScope())
            {
                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                model = clientService.GetClient(clientId);
            }

            return Ok(model);
        }

        [HttpGet("workitem")]
        public IActionResult GetWorkItem()
        {
            var workItemId = TestWorkItemIds.First();

            WorkItemModel? model;
            using (var scope = _serviceProvider.CreateScope())
            {
                var workItemService = scope.ServiceProvider.GetRequiredService<IWorkItemService>();
                model = workItemService.GetWorkItem(workItemId);
            }
            return Ok(model);
        }

        #endregion

        #region Private Methods

        private string GetConfigurationKeyValue(string key)
        {
            return ConfigurationUtility.GetConfigurationKeyValue(_configuration, key);
        }

        #endregion
    }
}
