using DemoModels;
using DemoTests.BaseClasses;
using DemoTests.TestHelpers;
using DemoUtilities;
using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
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
    public class UserWebApiTests : TestBase
    {
        // Dependencies
        private readonly UserController _controller;

        public UserWebApiTests()
        {
            // Create controller
            var mockLogger = new Mock<ILogger<UserController>>();
            Assert.IsNotNull(_configuration);
            Assert.IsNotNull(_serviceProvider);
            var webHostEnvironment = new Mock<IWebHostEnvironment>().Object;
            _controller = new UserController(mockLogger.Object, _configuration, _serviceProvider, webHostEnvironment);

            // Initialize HttpContext
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
        }

        #region Test Methods

        [TestMethod()]
        public async Task GetUserListTest()
        {
            await GetUserListUnauthenticatedTest();
            await GetUserListAuthenticatedTest();
        }

        [TestMethod()]
        public async Task GetUserTest()
        {
            await GetUserUnauthenticatedTest();
            await GetUserAuthenticatedTest();
        }

        [TestMethod()]
        public async Task GetClientsTest()
        {
            await GetClientsUnauthenticatedTest();
            await GetClientsAuthenticatedTest();
        }

        [TestMethod()]
        public async Task CheckForUniqueEmailAddressTest()
        {
            await CheckForUniqueEmailAddressUnauthenticatedTest();
            await CheckForUniqueEmailAddressAuthenticatedTest();
        }

        [TestMethod()]
        public async Task CrudTest()
        {
            var dateSlug = DateTime.Now.ToString("yyyyMMdd");
            var guid = Guid.NewGuid();

            // Create model for testing
            var model = new UserModel
            {
                Type = UserType.Admin,
                IsActive = true,
                EmailAddress = string.Format("UserCrudTest-EmailAddress-{0}-{1}@demo.com", dateSlug, guid),
                FirstName = string.Format("FirstName-{0}", dateSlug),
                MiddleName = string.Format("MiddleName-{0}", dateSlug),
                LastName = string.Format("LastName-{0}", dateSlug),
                AddressLine1 = string.Format("AddressLine1-{0}", dateSlug),
                AddressLine2 = string.Format("AddressLine2-{0}", dateSlug),
                City = string.Format("City-{0}", dateSlug),
                Region = string.Format("Region-{0}", dateSlug),
                PostalCode = string.Format("XX{0}", dateSlug), // Max length 10
                Country = string.Format("Country-{0}", dateSlug),
                PhoneNumber = string.Format("PhoneNumber-{0}", dateSlug), // Max length 20
            };

            // Create
            await CreateUserUnauthenticatedTest();
            var newModel = await CreateUserAuthenticatedTest(model);

            // Update
            newModel.EmailAddress = newModel.EmailAddress.Replace("@demo.com", "-UPDATED@demo.com");
            await UpdateUserUnauthenticatedTest();
            await UpdateUserAuthenticatedTest(newModel);

            // Delete
            await DeleteUserUnauthenticatedTest();
            await DeleteUserAuthenticatedTest(newModel.UserId);

            // Update EmailAddress to indicate deletion
            newModel.EmailAddress = newModel.EmailAddress.Replace("@demo.com", "-DELETED@demo.com");
            await UpdateUserAuthenticatedTest(newModel);
        }

        [TestMethod()]
        public async Task AddThenDeleteClientTest()
        {
            await AddClientUnauthenticatedTest();
            await AddClientAuthenticatedTest();

            await DeleteClientUnauthenticatedTest();
            await DeleteClientAuthenticatedTest();
        }

        #endregion

        #region Private Methods

        private static async Task GetUserListUnauthenticatedTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/user");

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

        private async Task GetUserListAuthenticatedTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync("/user/list");

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
            var getClientListResult = _controller.GetUserList() as OkObjectResult;
            Assert.IsNotNull(getClientListResult);
            var expected = (List<GenericListItemModel>?)getClientListResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<List<GenericListItemModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetUserUnauthenticatedTest()
        {
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/user/{0}", userId));

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

        private async Task GetUserAuthenticatedTest()
        {
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync(string.Format("/user/{0}", userId));

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
            var getClientResult = _controller.GetUser(userId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (UserModel?)getClientResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetClientsUnauthenticatedTest()
        {
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/user/{0}/clients", userId));

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

        private async Task GetClientsAuthenticatedTest()
        {
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync(string.Format("/user/{0}/clients", userId));

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
            var getClientsResult = _controller.GetClients(userId) as OkObjectResult;
            Assert.IsNotNull(getClientsResult);
            var expected = (List<ClientModel>?)getClientsResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<List<ClientModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task CheckForUniqueEmailAddressUnauthenticatedTest()
        {
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/user/{0}/checkemailaddress", userId));

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

        private async Task CheckForUniqueEmailAddressAuthenticatedTest()
        {
            var userId = _testUserIds.First();
            var uniqueEmailAddress = Guid.NewGuid().ToString();
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");

            // ------------------------------------------------------------
            // Email Address IS unique
            // ------------------------------------------------------------

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            var response = await httpClient.GetAsync(string.Format("/user/{0}/checkemailaddress?emailAddress={1}", userId, uniqueEmailAddress));

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
            var checkEmailAddressDuplicateResult = _controller.CheckForUniqueEmailAddress(userId, uniqueEmailAddress) as OkObjectResult;
            Assert.IsNotNull(checkEmailAddressDuplicateResult);
            var expected = checkEmailAddressDuplicateResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // ------------------------------------------------------------
            // Email Address is NOT unique
            // ------------------------------------------------------------

            var testUserId = _testUserIds.Skip(1).First();
            var getUserResult = _controller.GetUser(testUserId) as OkObjectResult;
            Assert.IsNotNull(getUserResult);
            var user = (UserModel?)getUserResult.Value;
            Assert.IsNotNull(user);
            var duplicateEmailAddress = user.EmailAddress;

            stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            response = await httpClient.GetAsync(string.Format("/user/{0}/checkemailaddress?emailAddress={1}", userId, duplicateEmailAddress));

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
            checkEmailAddressDuplicateResult = _controller.CheckForUniqueEmailAddress(userId, duplicateEmailAddress) as OkObjectResult;
            Assert.IsNotNull(checkEmailAddressDuplicateResult);
            expected = checkEmailAddressDuplicateResult.Value;
            Assert.IsNotNull(expected);
            actual = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(expected, actual);
            elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task CreateUserUnauthenticatedTest()
        {
            var userId = _testUserIds.First();
            var getClientResult = _controller.GetUser(userId) as OkObjectResult;
            Assert.IsNotNull(getClientResult);
            var expected = (UserModel?)getClientResult.Value;
            Assert.IsNotNull(expected);

            var content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PostAsync("/user", content);

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

        private static async Task<UserModel> CreateUserAuthenticatedTest(UserModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PostAsync("/user", content);

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
            var actual = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            var expected = model;
            model.UserId = actual.UserId;
            model.UserGuid = actual.UserGuid;
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            return actual;
        }

        private async Task UpdateUserUnauthenticatedTest()
        {
            var userId = _testUserIds.First();
            var getUserResult = _controller.GetUser(userId) as OkObjectResult;
            Assert.IsNotNull(getUserResult);
            var expected = (UserModel?)getUserResult.Value;
            Assert.IsNotNull(expected);

            var content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/user/{0}", userId), content);

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

        private static async Task UpdateUserAuthenticatedTest(UserModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PutAsync(string.Format("/user/{0}", model.UserId), content);

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
            var userId = _testUserIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/user/{0}", userId));

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

        private static async Task DeleteUserAuthenticatedTest(int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.DeleteAsync(string.Format("/user/{0}", userId));

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

        private static async Task AddClientUnauthenticatedTest()
        {
            var userId = 0;
            var clientId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/user/{0}/client/{1}", userId, clientId), null);

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

        private static async Task AddClientAuthenticatedTest()
        {
            var userId = 0;
            var clientId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PutAsync(string.Format("/user/{0}/client/{1}", userId, clientId), null);

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

        private static async Task DeleteClientUnauthenticatedTest()
        {
            var userId = 0;
            var clientId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/user/{0}/client/{1}", userId, clientId));

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

        private static async Task DeleteClientAuthenticatedTest()
        {
            var userId = 0;
            var clientId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<UserController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.DeleteAsync(string.Format("/user/{0}/client/{1}", userId, clientId));

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