using DemoModels;
using DemoServices.Interfaces;
using DemoTests.BaseClasses;
using DemoTests.TestHelpers;
using DemoUtilities;
using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DemoTests.WebApiTests
{
    [TestClass()]
    public class ClientWebApiTests : TestBase
    {
        // Dependencies
        private readonly ClientController _controller;

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

            // Create controller
            Assert.IsNotNull(_serviceProvider);
            var mockLogger = new Mock<ILogger<ClientController>>();
            var clientService = _serviceProvider.GetService<IClientService>();
            var webHostEnvironment = new Mock<IWebHostEnvironment>().Object;
            Assert.IsNotNull(clientService);
            _controller = new ClientController(mockLogger.Object, configuration, _serviceProvider);

            // Authenticate request
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity("Admin")));
            _controller.ControllerContext.HttpContext = mockHttpContext.Object;
        }

        [TestMethod()]
        public async Task GetClientListTest()
        {
            await GetClientListUnauthenticatedTest();
            await GetClientListAuthenticatedTest();
        }

        [TestMethod()]
        public async Task GetClientTest()
        {
            await GetClientUnauthenticatedTest();
            await GetClientAuthenticatedTest();
        }

        [TestMethod()]
        public async Task GetWorkItemsTest()
        {
            await GetWorkItemsUnauthenticatedTest();
            await GetWorkItemsAuthenticatedTest();
        }

        [TestMethod()]
        public async Task GetUsersTest()
        {
            await GetUsersUnauthenticatedTest();
            await GetUsersAuthenticatedTest();
        }

        [TestMethod()]
        public async Task CheckForUniqueClientNameTest()
        {
            await CheckForUniqueClientNameUnauthenticatedTest();
            await CheckForUniqueClientNameAuthenticatedTest();
        }

        [TestMethod()]
        public async Task CreateClientTest()
        {
            await CreateClientUnauthenticatedTest();
            await CreateClientAuthenticatedTest();
        }

        [TestMethod()]
        public async Task UpdateClientTest()
        {
            await UpdateClientUnauthenticatedTest();
            await UpdateClientAuthenticatedTest();
        }

        [TestMethod()]
        public async Task DeleteClientTest()
        {
            await DeleteClientUnauthenticatedTest();
            await DeleteClientAuthenticatedTest();
        }

        [TestMethod()]
        public async Task AddUserTest()
        {
            await AddUserUnauthenticatedTest();
            await AddUserAuthenticatedTest();
        }

        [TestMethod()]
        public async Task DeleteUserTest()
        {
            await DeleteUserUnauthenticatedTest();
            await DeleteUserAuthenticatedTest();
        }

        #region Private Methods

        private static async Task GetClientListUnauthenticatedTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/client");

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetClientListAuthenticatedTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync("/client");

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
            var getClientListResult = _controller.GetClientList() as OkObjectResult;
            Assert.IsNotNull(getClientListResult);
            var expected = (List<GenericListItemModel>?)getClientListResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<List<GenericListItemModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetClientUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}", clientId));

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetClientAuthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync(string.Format("/client/{0}", clientId));

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
            var getClientResult = _controller.GetClient(clientId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (ClientModel?)getClientResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<ClientModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetWorkItemsUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}/workitems", clientId));

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetWorkItemsAuthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync(string.Format("/client/{0}/workitems", clientId));

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
            var getWorkitemsResult = _controller.GetWorkItems(clientId) as OkObjectResult;
            Assert.IsNotNull(getWorkitemsResult);
            var expected = (List<WorkItemModel>?)getWorkitemsResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<List<WorkItemModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetUsersUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}/users", clientId));

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetUsersAuthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync(string.Format("/client/{0}/users", clientId));

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
            var getUsersResult = _controller.GetUsers(clientId) as OkObjectResult;
            Assert.IsNotNull(getUsersResult);
            var expected = (List<UserModel>?)getUsersResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<List<UserModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }


        private async Task CheckForUniqueClientNameUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/client/{0}/checkname", clientId));

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task CheckForUniqueClientNameAuthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var uniqueClientname = Guid.NewGuid().ToString();
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");

            // ------------------------------------------------------------
            // Client name IS unique
            // ------------------------------------------------------------

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            var response = await httpClient.GetAsync(string.Format("/client/{0}/checkname?name={1}", clientId, uniqueClientname));

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
            var checkForUniqueNameResult = _controller.CheckForUniqueName(clientId, uniqueClientname) as OkObjectResult;
            Assert.IsNotNull(checkForUniqueNameResult);
            var expected = checkForUniqueNameResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // ------------------------------------------------------------
            // Client name is NOT unique
            // ------------------------------------------------------------

            var testClientId = _testClientIds.Skip(1).First();
            var getClientResult = _controller.GetClient(testClientId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var client = (ClientModel?)getClientResult.Value;
            Assert.IsNotNull(client);
            var duplicateClientname = client.Name;

            stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            response = await httpClient.GetAsync(string.Format("/client/{0}/checkname?name={1}", clientId, duplicateClientname));

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
            checkForUniqueNameResult = _controller.CheckForUniqueName(clientId, duplicateClientname) as OkObjectResult;
            Assert.IsNotNull(checkForUniqueNameResult);
            expected = checkForUniqueNameResult.Value;
            Assert.IsNotNull(expected);
            actual = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(expected, actual);
            elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task CreateClientUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var getClientResult = _controller.GetClient(clientId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (ClientModel?)getClientResult.Value;
            Assert.IsNotNull(expected);
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PostAsync("/client", content);

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task CreateClientAuthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var getClientResult = _controller.GetClient(clientId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (ClientModel?)getClientResult.Value;
            Assert.IsNotNull(expected);
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PostAsync("/client", content);

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
            var actual = JsonConvert.DeserializeObject<ClientModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            expected.ClientId = actual.ClientId;
            expected.ClientGuid = actual.ClientGuid;
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task UpdateClientUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var getClientResult = _controller.GetClient(clientId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (ClientModel?)getClientResult.Value;
            Assert.IsNotNull(expected);
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/client/{0}", clientId), content);

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task UpdateClientAuthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var getClientResult = _controller.GetClient(clientId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (ClientModel?)getClientResult.Value;
            Assert.IsNotNull(expected);
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PutAsync(string.Format("/client/{0}", clientId), content);

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
            var actual = JsonConvert.DeserializeObject<ClientModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }


        private async Task DeleteClientUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/client/{0}", clientId));

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task DeleteClientAuthenticatedTest()
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.DeleteAsync(string.Format("/client/{0}", clientId));

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
            var actual = await response.Content.ReadAsStringAsync() == "true";
            Assert.IsTrue(actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task AddUserUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/client/{0}/user/{1}", clientId, userId), null);

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task AddUserAuthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PutAsync(string.Format("/client/{0}/user/{1}", clientId, userId), null);

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
            var actual = await response.Content.ReadAsStringAsync() == "true";
            Assert.IsTrue(actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }


        private async Task DeleteUserUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/client/{0}/user/{1}", clientId, userId));

            stopWatch.Stop();

            // Check elapsed time
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Check response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task DeleteUserAuthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.DeleteAsync(string.Format("/client/{0}/user/{1}", clientId, userId));

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
            var actual = await response.Content.ReadAsStringAsync() == "true";
            Assert.IsTrue(actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        #endregion
    }
}