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
    public class WorkItemWebApiTests : TestBase
    {
        // Dependencies
        private readonly WorkItemController _controller;

        public WorkItemWebApiTests()
        {
            // Create controller
            var mockLogger = new Mock<ILogger<WorkItemController>>();
            Assert.IsNotNull(_configuration);
            Assert.IsNotNull(_serviceProvider);
            _controller = new WorkItemController(mockLogger.Object, _configuration, _serviceProvider);

            // Initialize HttpContext
            _controller.ControllerContext.HttpContext = _mockHttpContext.Object;
        }

        #region Test Methods

        [TestMethod()]
        public async Task GetWorkItemListTest()
        {
            await GetWorkItemListUnauthenticatedTest();
            await GetWorkItemListAuthenticatedTest();
        }

        [TestMethod()]
        public async Task GetWorkItemTest()
        {
            await GetWorkItemUnauthenticatedTest();
            await GetWorkItemAuthenticatedTest();
        }

        [TestMethod()]
        public async Task GetUsersTest()
        {
            await GetUsersUnauthenticatedTest();
            await GetUsersAuthenticatedTest();
        }

        [TestMethod()]
        public async Task CheckForUniqueTitle()
        {
            await CheckForUniqueTitleUnauthenticatedTest();
            await CheckForUniqueTitleAuthenticatedTest();
        }

        [TestMethod()]
        public async Task CrudTest()
        {
            var dateSlug = DateTime.Now.ToString("yyyyMMdd");
            var guid = Guid.NewGuid();

            // Create model for testing
            var model = new WorkItemModel
            {
                ClientId = _testClientIds.First(),
                Type = WorkItemType.Bug,
                Status = WorkItemStatus.New,
                Title = string.Format("WorkItemCrudTest-Title-{0}-{1}", dateSlug, guid),
                SubTitle = string.Format("SubTitle-{0}", dateSlug),
                Summary = string.Format("Summary-{0}", dateSlug),
                Body = string.Format("Body-{0}", dateSlug),
            };

            // Create
            await CreateWorkItemUnauthenticatedTest();
            var newModel = await CreateWorkItemAuthenticatedTest(model);

            // Update
            newModel.Title = string.Format("{0}-UPDATED", newModel.Title);
            await UpdateWorkItemUnauthenticatedTest();
            await UpdateWorkItemAuthenticatedTest(newModel);

            // Delete
            await DeleteWorkItemUnauthenticatedTest();
            await DeleteWorkItemAuthenticatedTest(newModel.WorkItemId);

            // Update Title to indicate deletion
            newModel.Title = string.Format("{0}-DELETED", newModel.Title);
            await UpdateWorkItemAuthenticatedTest(newModel);
        }

        [TestMethod()]
        public async Task AddThenDeleteUserTest()
        {
            await AddUserUnauthenticatedTest();
            await AddUserAuthenticatedTest();

            await DeleteUserUnauthenticatedTest();
            await DeleteUserAuthenticatedTest();
        }

        #endregion

        #region Private Methods

        private static async Task GetWorkItemListUnauthenticatedTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync("/workitem");

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

        private async Task GetWorkItemListAuthenticatedTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync("/workitem");

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
            var getWorkItemListResult = _controller.GetWorkItemList() as OkObjectResult;
            Assert.IsNotNull(getWorkItemListResult);
            var expected = (List<GenericListItemModel>?)getWorkItemListResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<List<GenericListItemModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetWorkItemUnauthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/workitem/{0}", workItemId));

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

        private async Task GetWorkItemAuthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync(string.Format("/workitem/{0}", workItemId));

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
            var getWorkItemResult = _controller.GetWorkItem(workItemId) as OkObjectResult;
            Assert.IsNotNull(getWorkItemResult);
            var expected = (WorkItemModel?)getWorkItemResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<WorkItemModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task GetUsersUnauthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/workitem/{0}/users", workItemId));

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
            var workItemId = _testWorkItemIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.GetAsync(string.Format("/workitem/{0}/users", workItemId));

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
            var getWorkitemsResult = _controller.GetUsers(workItemId) as OkObjectResult;
            Assert.IsNotNull(getWorkitemsResult);
            var expected = (List<UserModel>?)getWorkitemsResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<List<UserModel>>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task CheckForUniqueTitleUnauthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.GetAsync(string.Format("/workitem/{0}/checktitle", workItemId));

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

        private async Task CheckForUniqueTitleAuthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();
            var uniqueTitle = Guid.NewGuid().ToString();
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");

            // ------------------------------------------------------------
            // Title name IS unique
            // ------------------------------------------------------------

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            var response = await httpClient.GetAsync(string.Format("/workitem/{0}/checktitle?title={1}", workItemId, uniqueTitle));

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
            var checkForUniqueTitleResult = _controller.CheckForUniqueTitle(workItemId, uniqueTitle) as OkObjectResult;
            Assert.IsNotNull(checkForUniqueTitleResult);
            var expected = checkForUniqueTitleResult.Value;
            Assert.IsNotNull(expected);
            var actual = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            // ------------------------------------------------------------
            // Title is NOT unique
            // ------------------------------------------------------------

            var testWorkItemId = _testWorkItemIds.Skip(1).First();
            var getWorkItemResult = _controller.GetWorkItem(testWorkItemId) as OkObjectResult;
            Assert.IsNotNull(getWorkItemResult);
            var workItem = (WorkItemModel?)getWorkItemResult.Value;
            Assert.IsNotNull(workItem);
            var duplicateTitle = workItem.Title;

            stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            response = await httpClient.GetAsync(string.Format("/workitem/{0}/checktitle?title={1}", workItemId, duplicateTitle));

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
            checkForUniqueTitleResult = _controller.CheckForUniqueTitle(workItemId, duplicateTitle) as OkObjectResult;
            Assert.IsNotNull(checkForUniqueTitleResult);
            expected = checkForUniqueTitleResult.Value;
            Assert.IsNotNull(expected);
            actual = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(expected, actual);
            elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));
        }

        private async Task CreateWorkItemUnauthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();
            var getWorkItemResult = _controller.GetWorkItem(workItemId) as OkObjectResult;
            Assert.IsNotNull(getWorkItemResult);
            var expected = (WorkItemModel?)getWorkItemResult.Value;
            Assert.IsNotNull(expected);

            var content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PostAsync("/workitem", content);

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

        private static async Task<WorkItemModel> CreateWorkItemAuthenticatedTest(WorkItemModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PostAsync("/workitem", content);

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
            var actual = JsonConvert.DeserializeObject<WorkItemModel>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(actual);
            var expected = model;
            model.WorkItemId = actual.WorkItemId;
            model.WorkItemGuid = actual.WorkItemGuid;
            CompareModels.Compare(expected, actual);

            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0}", elapsedTime));

            return actual;
        }

        private async Task UpdateWorkItemUnauthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();
            var getWorkItemResult = _controller.GetWorkItem(workItemId) as OkObjectResult;
            Assert.IsNotNull(getWorkItemResult);
            var expected = (WorkItemModel?)getWorkItemResult.Value;
            Assert.IsNotNull(expected);

            var content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/workitem/{0}", workItemId), content);

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

        private static async Task UpdateWorkItemAuthenticatedTest(WorkItemModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PutAsync(string.Format("/workitem/{0}", model.WorkItemId), content);

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

        private async Task DeleteWorkItemUnauthenticatedTest()
        {
            var workItemId = _testWorkItemIds.First();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/workitem/{0}", workItemId));

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

        private static async Task DeleteWorkItemAuthenticatedTest(int workItemId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.DeleteAsync(string.Format("/workitem/{0}", workItemId));

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
            var workItemId = 0;
            var userId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.PutAsync(string.Format("/workitem/{0}/user/{1}", workItemId, userId), null);

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
            var workItemId = 0;
            var userId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.PutAsync(string.Format("/workitem/{0}/user/{1}", workItemId, userId), null);

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

        private static async Task DeleteUserUnauthenticatedTest()
        {
            var workItemId = 0;
            var userId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            var response = await httpClient.DeleteAsync(string.Format("/workitem/{0}/user/{1}", workItemId, userId));

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

        private static async Task DeleteUserAuthenticatedTest()
        {
            var workItemId = 0;
            var userId = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make Web API call
            await using var application = new WebApplicationFactory<WorkItemController>();
            using var httpClient = application.CreateClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "Bearer demo");
            var response = await httpClient.DeleteAsync(string.Format("/workitem/{0}/user/{1}", workItemId, userId));

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