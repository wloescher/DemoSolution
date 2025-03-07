using DemoModels;
using DemoRepository.Entities;
using DemoServices.Interfaces;
using DemoTests.BaseClasses;
using DemoTests.TestHelpers;
using DemoUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static DemoModels.Enums;

namespace DemoTests.ServiceTests
{
    [TestClass()]
    public class ClientServiceTests : ServiceTestBase
    {
        // Dependencies
        private readonly DemoSqlContext _dbContext;

        // Configuration Values
        private readonly List<int> _testClientIds = new();

        public ClientServiceTests()
        {
            // Configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();

            // Dependencies
            var optionsBuilder = new DbContextOptionsBuilder<DemoSqlContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x => x.UseCompatibilityLevel(100));
            _dbContext = new DemoSqlContext(optionsBuilder.Options);

            // Configuration Values
            _testClientIds = (configuration.GetValue<string>("Demo:TestClientIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
        }

        #region Test Methods

        [TestMethodDependencyInjection]
        public void GetClientTest(IClientService clientService)
        {
            Console.WriteLine("ClientId, Type, Elapsed");
            foreach (var userId in _testClientIds)
            {
                GetClientTest(clientService, userId);
            }
        }

        [TestMethodDependencyInjection]
        public void GetClientsTest(IClientService clientService)
        {
            // Get models
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var models = clientService.GetClients();

            stopWatch.Stop();
            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0} models, {1} elapsed", models.Count, elapsedTime));

            // Check results
            Assert.AreNotEqual(0, models.Count);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 2);
#endif
            // Display results
            Console.WriteLine("ClientId, Type, Name");
            foreach (var model in models)
            {
                Console.WriteLine(string.Format("{0}, {1}, {2}", model.ClientId, model.Type, model.Name));
            }
        }

        [TestMethodDependencyInjection]
        public void CrudTest(IClientService clientService)
        {
            var userId = _dbContext.UserViews.First().UserId;
            var ticks = DateTime.Now.Ticks;
            var model = new ClientModel
            {
                Type = ClientType.Internal,
                Name = string.Format("Name-{0}", ticks),
                Address = string.Format("Address-{0}", ticks),
                City = string.Format("City-{0}", ticks),
                Region = string.Format("Region-{0}", ticks),
                PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6)),
                Country = string.Format("Country-{0}", ticks),
                Url = string.Format("Url-{0}", ticks),
            };

            var testClientId = CreateClientTest(clientService, model, userId);
            GetClientTest(clientService, model, testClientId);
            UpdateClientTest(clientService, model, testClientId, userId);
            DeleteClientTest(clientService, testClientId, userId);
        }

        [TestMethodDependencyInjection]
        public void CheckForUniqueClientNameTest(IClientService clientService)
        {
            var entity = _dbContext.ClientViews.First();
            Assert.IsTrue(clientService.CheckForUniqueClientName(entity.ClientId, entity.Name));
            Assert.IsTrue(clientService.CheckForUniqueClientName(entity.ClientId, new Guid().ToString()));
            Assert.IsTrue(clientService.CheckForUniqueClientName(entity.ClientId + 1, new Guid().ToString()));
            Assert.IsFalse(clientService.CheckForUniqueClientName(entity.ClientId + 1, entity.Name));
        }

        [TestMethodDependencyInjection]
        public void GetClientKeyValuePairsTest(IClientService clientService)
        {
            // Get models
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var keyValuePairs = clientService.GetClientKeyValuePairs();

            stopWatch.Stop();
            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0} key/value pairs, {1} elapsed", keyValuePairs.Count, elapsedTime));

            // Check results
            Assert.AreNotEqual(0, keyValuePairs.Count);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 2);
#endif

            // Display results
            Console.WriteLine("ClientId, Name");
            foreach (var keyValuePair in keyValuePairs)
            {
                Console.WriteLine(string.Format("{0}, {1}", keyValuePair.Key, keyValuePair.Value));
            }
        }

        [TestMethodDependencyInjection]
        public void CreateClientUserTest(IClientService clientService)
        {
            var clientId = _dbContext.ClientViews.First().ClientId;
            var userId = _dbContext.UserViews.First().UserId;
            var result = clientService.CreateClientUser(clientId, userId, userId);
            Assert.IsTrue(result);
        }

        [TestMethodDependencyInjection]
        public void DeleteClientUserTest(IClientService clientService)
        {
            var clientId = _dbContext.ClientViews.First().ClientId;
            var userId = _dbContext.UserViews.First().UserId;
            var result = clientService.DeleteClientUser(clientId, userId, userId);
            Assert.IsTrue(result);
        }

        [TestMethodDependencyInjection]
        public void GetClientUsersTest(IClientService clientService)
        {
            var clientId = _dbContext.ClientViews.First().ClientId;
            var result = clientService.GetClientUsers(clientId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void GetClientWorkItemsTest(IClientService clientService)
        {
            var clientId = _dbContext.ClientViews.First().ClientId;
            var result = clientService.GetClientWorkItems(clientId);
            Assert.AreNotEqual(0, result.Count);
        }

        #endregion

        #region Private Methods

        private void GetClientTest(IClientService clientService, int clientId)
        {
            // Get entity
            var entity = _dbContext.ClientViews.FirstOrDefault(x => x.ClientId == clientId);
            if (entity == null)
            {
                Console.WriteLine(string.Format("ClientId {0} not found.", clientId));
            }
            else
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                // Get model
                var model = clientService.GetClient(entity.ClientId);
                Assert.IsNotNull(model);

                stopWatch.Stop();

                // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

                var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
                Console.WriteLine(string.Format("{0}, {1}, {2}", entity.ClientId, entity.Type, elapsedTime));
            }
        }

        private void CheckForClientAuditRecord(int clientId, int userId, AuditAction action)
        {
            var entities = _dbContext.ClientAudits
                .Where(x => x.ClientAuditClientId == clientId && x.ClientAuditUserId == userId && x.ClientAuditActionId == (int)action);
            Assert.IsNotNull(entities);
            Assert.AreEqual(1, entities.Count());
            Console.WriteLine("audit record created.");
        }

        private int CreateClientTest(IClientService clientService, ClientModel model, int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = clientService.CreateClient(model, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsNotNull(result);
            model.ClientId = result.ClientId;
            model.ClientGuid = result.ClientGuid;
            ClientTestHelper.Compare(model, result);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Display results
            Console.WriteLine(string.Format("ClientId: {0}", result.ClientId));
            Console.Write(string.Format("Create: {0}ms...", stopWatch.ElapsedMilliseconds));

            CheckForClientAuditRecord(result.ClientId, userId, AuditAction.Create);

            return result.ClientId;
        }

        private static void GetClientTest(IClientService clientService, ClientModel model, int newClientId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var getClientResult = clientService.GetClient(newClientId);
            stopWatch.Stop();

            // Check results
            ClientTestHelper.Compare(model, getClientResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.WriteLine(string.Format("Get: {0}ms", stopWatch.ElapsedMilliseconds));
        }

        private void UpdateClientTest(IClientService clientService, ClientModel model, int newClientId, int userId)
        {
            // Update properties
            var ticks = DateTime.Now.Ticks;
            model.Type = ClientType.Internal;
            model.Name = string.Format("Name-{0}", ticks);
            model.Address = string.Format("Address-{0}", ticks);
            model.City = string.Format("City-{0}", ticks);
            model.Region = string.Format("Region-{0}", ticks);
            model.PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6));
            model.Country = string.Format("Country-{0}", ticks);
            model.Url = string.Format("Url-{0}", ticks);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var updateClientResult = clientService.UpdateClient(model, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsTrue(updateClientResult);
            var getUpdatedClientResult = clientService.GetClient(newClientId);
            Assert.IsNotNull(getUpdatedClientResult);
            ClientTestHelper.Compare(model, getUpdatedClientResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.Write(string.Format("Update: {0}ms...", stopWatch.ElapsedMilliseconds));

            CheckForClientAuditRecord(newClientId, userId, AuditAction.Update);
        }

        private void DeleteClientTest(IClientService clientService, int testClientId, int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var deleteClientResult = clientService.DeleteClient(testClientId, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsTrue(deleteClientResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            var getDeletedClientResult = clientService.GetClient(testClientId);
            Assert.IsNotNull(getDeletedClientResult);
            Assert.IsTrue(getDeletedClientResult.IsDeleted);

            Console.Write(string.Format("Delete: {0}ms...", stopWatch.ElapsedMilliseconds));

            CheckForClientAuditRecord(testClientId, userId, AuditAction.Delete);
        }

        #endregion
    }
}