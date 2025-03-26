using DemoModels;
using DemoServices.Interfaces;
using DemoTests.TestHelpers;
using DemoUtilities;
using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
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
        public async Task GetClientListUnauthenticated(IClientService clientService)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
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

        [TestMethodDependencyInjection]
        public async Task GetClientListAuthenticated(IClientService clientService)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
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
            var expected = clientService.GetClients();
            var actual = JsonConvert.DeserializeObject<List<ClientModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            ClientTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task GetClientUnauthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
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

        [TestMethodDependencyInjection]
        public async Task GetClientAuthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
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
            var expected = clientService.GetClient(clientId) ?? new();
            var actual = JsonConvert.DeserializeObject<ClientModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            ClientTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task GetWorkItemsUnauthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
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

        [TestMethodDependencyInjection]
        public async Task GetWorkItemsAuthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
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
            var expected = clientService.GetClientWorkItems(clientId);
            var actual = JsonConvert.DeserializeObject<List<WorkItemModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            WorkItemTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task GetUsersUnauthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
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


        [TestMethodDependencyInjection]
        public async Task GetUsersAuthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
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
            var expected = clientService.GetClientUsers(clientId);
            var actual = JsonConvert.DeserializeObject<List<UserModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            UserTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task CheckForUniqueClientNameUnauthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
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

        [TestMethodDependencyInjection]
        public async Task CheckForUniqueClientNameAuthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var uniqueClientname = Guid.NewGuid().ToString();
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();

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
            var expected = clientService.CheckForUniqueClientName(clientId, uniqueClientname);
            var actual = await response.Content.ReadAsStringAsync() == "true";
            Assert.AreEqual(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // ------------------------------------------------------------
            // Client name is NOT unique
            // ------------------------------------------------------------

            var testClientId = _testClientIds.Skip(1).First();
            var client = clientService.GetClient(testClientId) ?? new();
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
            expected = clientService.CheckForUniqueClientName(clientId, duplicateClientname);
            actual = await response.Content.ReadAsStringAsync() == "true";
            Assert.AreEqual(expected, actual);

            elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task CreateClientUnauthenticated(IClientService clientService)
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

        [TestMethodDependencyInjection]
        public async Task CreateClientAuthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var expected = clientService.GetClient(clientId) ?? new();
            expected.Name = Guid.NewGuid().ToString();
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
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
            ClientTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task UpdateClientUnauthenticated(IClientService clientService)
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

        [TestMethodDependencyInjection]
        public async Task UpdateClientAuthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var expected = clientService.GetClient(clientId) ?? new();
            StringContent content = new(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
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
            ClientTestHelper.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        [TestMethodDependencyInjection]
        public async Task DeleteClientUnauthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
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

        [TestMethodDependencyInjection]
        public async Task DeleteClientAuthenticated(IClientService clientService)
        {
            var clientId = _testClientIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<TestController>();
            using var httpClient = application.CreateClient();
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

        [TestMethodDependencyInjection]
        public async Task AddUserUnauthenticated(IClientService clientService)
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

        [TestMethodDependencyInjection]
        public async Task AddUserAuthenticated(IClientService clientService)
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

        [TestMethodDependencyInjection]
        public async Task DeleteUserUnauthenticated(IClientService clientService)
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

        [TestMethodDependencyInjection]
        public async Task DeleteUserAuthenticated(IClientService clientService)
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
    }
}