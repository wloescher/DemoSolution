using DemoModels;
using DemoServices.Interfaces;
using DemoTests.BaseClasses;
using DemoTests.TestHelpers;
using DemoUtilities;
using System.Diagnostics;
using static DemoModels.Enums;

namespace DemoTests.ServiceTests
{
    [TestClass()]
    public class ClientServiceTests : TestBase
    {
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
            var userId = _testUserIds.First();
            var ticks = DateTime.Now.Ticks;
            var model = new ClientModel
            {
                Type = ClientType.Internal,
                Name = string.Format("Name-{0}", ticks),
                AddressLine1 = string.Format("AddressLine1-{0}", ticks),
                AddressLine2 = string.Format("AddressLine2-{0}", ticks),
                City = string.Format("City-{0}", ticks),
                Region = string.Format("Region-{0}", ticks),
                PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6)), // Max length 10
                Country = string.Format("Country-{0}", ticks),
                PhoneNumber = string.Format("PhoneNumber-{0}", ticks.ToString().Substring(ticks.ToString().Length - 8)), // Max length 20
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
            int clientId = _testClientIds.First();
            var model = clientService.GetClient(clientId);
            Assert.IsNotNull(model);

            Assert.IsTrue(clientService.CheckForUniqueClientName(clientId, model.Name));
            Assert.IsTrue(clientService.CheckForUniqueClientName(clientId, new Guid().ToString()));
            Assert.IsTrue(clientService.CheckForUniqueClientName(clientId + 1, new Guid().ToString()));
            Assert.IsFalse(clientService.CheckForUniqueClientName(clientId + 1, model.Name));
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
        public void GetClientUsersTest(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var result = clientService.GetClientUsers(clientId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void AddThenRemoveUserTest(IClientService clientService)
        {
            var clientId = 0;
            var userId = 0;

            // Create user
            var result = clientService.CreateClientUser(clientId, userId, userId);
            Assert.IsTrue(result);

            // Delete user
            result = clientService.DeleteClientUser(clientId, userId, userId);
            Assert.IsTrue(result);
        }

        [TestMethodDependencyInjection]
        public void GetClientWorkItemsTest(IClientService clientService)
        {
            var clientId = _testClientIds.First();
            var result = clientService.GetClientWorkItems(clientId);
            Assert.AreNotEqual(0, result.Count);
        }

        #endregion

        #region Private Methods

        private void GetClientTest(IClientService clientService, int clientId)
        {
            // Get entity
            var entity = CreateDbContext().ClientViews.FirstOrDefault(x => x.ClientId == clientId);

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
            var entities = CreateDbContext().ClientAudits
                .Where(x => x.ClientAuditClientId == clientId && x.ClientAuditUserId == userId && x.ClientAuditActionId == (int)action);

            Assert.AreEqual(1, entities.Count());
            Console.WriteLine("Audit record created.");
        }

        private static int CreateClientTest(IClientService clientService, ClientModel model, int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = clientService.CreateClient(model, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsNotNull(result);
            model.ClientId = result.ClientId;
            model.ClientGuid = result.ClientGuid;
            CompareModels.Compare(model, result);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Display results
            Console.WriteLine(string.Format("ClientId: {0}", result.ClientId));
            Console.Write(string.Format("Create: {0}ms...", stopWatch.ElapsedMilliseconds));

            // TODO: Check for audit record
            // CheckForClientAuditRecord(result.ClientId, userId, AuditAction.Create);

            return result.ClientId;
        }

        private static void GetClientTest(IClientService clientService, ClientModel model, int newClientId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var getClientResult = clientService.GetClient(newClientId);
            stopWatch.Stop();

            // Check results
            CompareModels.Compare(model, getClientResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.WriteLine(string.Format("Get: {0}ms", stopWatch.ElapsedMilliseconds));
        }

        private static void UpdateClientTest(IClientService clientService, ClientModel model, int newClientId, int userId)
        {
            // Update properties
            var ticks = DateTime.Now.Ticks;
            model.Type = ClientType.Internal;
            model.Name = string.Format("Name-{0}", ticks);
            model.AddressLine1 = string.Format("AddressLine1-{0}", ticks);
            model.AddressLine2 = string.Format("AddressLine2-{0}", ticks);
            model.City = string.Format("City-{0}", ticks);
            model.Region = string.Format("Region-{0}", ticks);
            model.PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6)); // Max length 10
            model.Country = string.Format("Country-{0}", ticks);
            model.PhoneNumber = string.Format("PhoneNumber-{0}", ticks.ToString().Substring(ticks.ToString().Length - 8)); // Max length 20
            model.Url = string.Format("Url-{0}", ticks);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var updateClientResult = clientService.UpdateClient(model, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsTrue(updateClientResult);
            var getUpdatedClientResult = clientService.GetClient(newClientId);
            Assert.IsNotNull(getUpdatedClientResult);
            CompareModels.Compare(model, getUpdatedClientResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.Write(string.Format("Update: {0}ms...", stopWatch.ElapsedMilliseconds));

            // TODO: Check for audit record
            // CheckForClientAuditRecord(newClientId, userId, AuditAction.Update);
        }

        private static void DeleteClientTest(IClientService clientService, int testClientId, int userId)
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

            // TODO: Check for audit record
            // CheckForClientAuditRecord(testClientId, userId, AuditAction.Delete);
        }

        #endregion
    }
}