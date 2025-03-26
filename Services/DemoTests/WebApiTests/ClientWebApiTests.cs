using DemoServices.Interfaces;
using DemoUtilities;
using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace DemoTests.WebApiTests
{
    [TestClass()]
    public class ClientWebApiTests
    {
        // Configuration Values
        private readonly List<int> _testClientIds = new();
        private readonly List<int> _testUserIds = new();
        private readonly List<int> _testWorkItemIds = new();

        public ClientWebApiTests()
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
        public async Task GetClientList(IClientService clientService)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get model
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/client");

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task GetClient(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get model
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}", clientId));

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task GetWorkItems(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get model
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}/workitems", clientId));

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task GetUsers(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get model
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}/users", clientId));

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task CheckName(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get model
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}/checkname", clientId));

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task CreateClient(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var expected = clientService.GetClient(clientId);
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PostAsync("/client", content);

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task UpdateClient(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var expected = clientService.GetClient(clientId);
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/client/{0}", clientId), content);

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task DeleteClient(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/client/{0}", clientId));

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task AddUser(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/client/{0}/user/{1}", clientId, userId), null);

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }

        [TestMethodDependencyInjection]
        public async Task DeleteUser(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/client/{0}/user/{1}", clientId, userId));

            stopWatch.Stop();

            // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // TODO: Make authenticated request
            Assert.Inconclusive("Test not fully implemented - make authenticated request and test response.");
        }
    }
}