using DemoModels;
using DemoServices.Interfaces;
using DemoTests.TestHelpers;
using DemoUtilities;
using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DemoTests.WebApiTests
{
    [TestClass()]
    public class TestWebApiTests
    {
        // Configuration Values
        private readonly List<int> _testClientIds = new();
        private readonly List<int> _testUserIds = new();
        private readonly List<int> _testWorkItemIds = new();

        public TestWebApiTests()
        {
            // Configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();

            // Configuration Values
            _testClientIds = (configuration.GetValue<string>("Demo:TestClientIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            _testUserIds = (configuration.GetValue<string>("Demo:TestUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
            _testWorkItemIds = (configuration.GetValue<string>("Demo:TestWorkItemIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
        }

        [TestMethodDependencyInjection]
        public async Task GetClient(IClientService clientService)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/test/client");

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            response.EnsureSuccessStatusCode();
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Check result
            var expected = clientService.GetClient(_testClientIds.First()) ?? new();
            var actual = JsonConvert.DeserializeObject<ClientModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            ClientTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}, {1}, {2}", expected.ClientId, expected.Type, elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task GetUser(IUserService userService)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/test/user");

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            response.EnsureSuccessStatusCode();
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Check result
            var expected = userService.GetUser(_testUserIds.First()) ?? new();
            var actual = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            UserTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}, {1}, {2}", expected.UserId, expected.Type, elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task GetWorkItem(IWorkItemService workItemService)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/test/workItem");

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            response.EnsureSuccessStatusCode();
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Check result
            var expected = workItemService.GetWorkItem(_testWorkItemIds.First()) ?? new();
            var actual = JsonConvert.DeserializeObject<WorkItemModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            WorkItemTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}, {1}, {2}", expected.WorkItemId, expected.Type, elapsedTime));
        }
    }
}