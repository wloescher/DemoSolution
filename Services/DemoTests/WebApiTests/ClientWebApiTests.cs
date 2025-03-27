using DemoModels;
using DemoTests.BaseClasses;
using DemoTests.TestHelpers;
using DemoUtilities;
using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;
using static DemoModels.Enums;

namespace DemoTests.WebApiTests
{
    [TestClass()]
    public class ClientWebApiTests : TestBase
    {
        // Dependencies
        private readonly ClientController _controller;

        public ClientWebApiTests()
        {
            // Create controller
            var mockLogger = new Mock<ILogger<ClientController>>();
            Assert.IsNotNull(_configuration);
            Assert.IsNotNull(_serviceProvider);
            _controller = new ClientController(mockLogger.Object, _configuration, _serviceProvider);

            // Initialize HttpContext
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
        }

        #region Test Methods

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
        public async Task CrudTest()
        {
            var dateSlug = DateTime.Now.ToString("yyyyMMdd");
            var guid = Guid.NewGuid();

            // Create model for testing
            var model = new ClientModel
            {
                Type = ClientType.External,
                Name = string.Format("ClientCrudTest-Name-{0}-{1}", dateSlug, guid),
                AddressLine1 = string.Format("AddressLine1-{0}", dateSlug),
                AddressLine2 = string.Format("AddressLine2-{0}", dateSlug),
                City = string.Format("City-{0}", dateSlug),
                Region = string.Format("Region-{0}", dateSlug),
                PostalCode = string.Format("XX{0}", dateSlug), // Max length 10
                Country = string.Format("Country-{0}", dateSlug),
                PhoneNumber = string.Format("PhoneNumber-{0}", dateSlug), // Max length 20
                Url = string.Format("Url-{0}", dateSlug),
            };

            // Create
            await CreateClientUnauthenticatedTest();
            var newModel = await CreateClientAuthenticatedTest(model);

            // Update
            newModel.Name = string.Format("{0}-UPDATED", newModel.Name);
            await UpdateClientUnauthenticatedTest();
            await UpdateClientAuthenticatedTest(newModel);

            // Delete
            await DeleteClientUnauthenticatedTest();
            await DeleteClientAuthenticatedTest(newModel.ClientId);

            // Update Client Name to indicate deletion
            newModel.Name = string.Format("{0}-DELETED", newModel.Name);
            await UpdateClientAuthenticatedTest(newModel);
        }

        [TestMethod()]
        public async Task AddThenRemoveUserTest()
        {
            await AddUserUnauthenticatedTest();
            await AddUserAuthenticatedTest();

            await RemoveUserUnauthenticatedTest();
            await RemoveUserAuthenticatedTest();
        }

        #endregion

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

            var content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

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

        private static async Task<ClientModel> CreateClientAuthenticatedTest(ClientModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

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
            var expected = model;
            model.ClientId = actual.ClientId;
            model.ClientGuid = actual.ClientGuid;
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            return actual;
        }

        private async Task UpdateClientUnauthenticatedTest()
        {
            var clientId = _testClientIds.First();
            var getClientResult = _controller.GetClient(clientId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (ClientModel?)getClientResult.Value;
            Assert.IsNotNull(expected);

            var content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

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

        private static async Task UpdateClientAuthenticatedTest(ClientModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<ClientController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PutAsync(string.Format("/client/{0}", model.ClientId), content);

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

        private static async Task DeleteClientAuthenticatedTest(int clientId)
        {
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

        private static async Task AddUserUnauthenticatedTest()
        {
            var clientId = 0;
            var userId = 0;

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

        private static async Task AddUserAuthenticatedTest()
        {
            var clientId = 0;
            var userId = 0;

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

        private static async Task RemoveUserUnauthenticatedTest()
        {
            var clientId = 0;
            var userId = 0;

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

        private static async Task RemoveUserAuthenticatedTest()
        {
            var clientId = 0;
            var userId = 0;

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