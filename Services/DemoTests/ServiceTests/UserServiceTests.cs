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
    public class UserServiceTests : ServiceTestBase
    {
        // Dependencies
        private readonly DemoSqlContext _dbContext;

        // Configuration Values
        private readonly List<int> _testUserIds = new();

        public UserServiceTests()
        {
            // Configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();

            // Dependencies
            var optionsBuilder = new DbContextOptionsBuilder<DemoSqlContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            _dbContext = new DemoSqlContext(optionsBuilder.Options);

            // Configuration Values
            _testUserIds = (configuration.GetValue<string>("Demo:TestUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
        }

        #region Test Methods

        [TestMethodDependencyInjection]
        public void GetUserTest(IUserService userService)
        {
            Console.WriteLine("UserId, Type, Elapsed");
            foreach (var userId in _testUserIds)
            {
                GetUserTest(userService, userId);
            }
        }

        [TestMethodDependencyInjection]
        public void GetUsersTest(IUserService userService)
        {
            // Get models
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var models = userService.GetUsers();

            stopWatch.Stop();
            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0} models, {1} elapsed", models.Count, elapsedTime));

            // Check results
            Assert.AreNotEqual(0, models.Count);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 2);
#endif
            // Display results
            Console.WriteLine("UserId, Type, FullName");
            foreach (var model in models)
            {
                Console.WriteLine(string.Format("{0}, {1}, {2}", model.UserId, model.Type, model.FullName));
            }
        }

        [TestMethodDependencyInjection]
        public void CrudTest(IUserService userService)
        {
            var userId = _testUserIds.First();
            var ticks = DateTime.Now.Ticks;
            var model = new UserModel
            {
                Type = UserType.Admin,
                EmailAddress = string.Format("{0}@demo.com", ticks),
                FirstName = string.Format("First-{0}", ticks),
                MiddleName = string.Format("Middle-{0}", ticks),
                LastName = string.Format("Last-{0}", ticks),
                AddressLine1 = string.Format("AddressLine1-{0}", ticks),
                AddressLine2 = string.Format("AddressLine2-{0}", ticks),
                City = string.Format("City-{0}", ticks),
                Region = string.Format("Region-{0}", ticks),
                PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6)),
                Country = string.Format("Country-{0}", ticks),
            };

            var testUserId = CreateUserTest(userService, model, userId);
            GetUserTest(userService, model, testUserId);
            UpdateUserTest(userService, model, testUserId, userId);
            DeleteUserTest(userService, testUserId, userId);
        }

        [TestMethodDependencyInjection]
        public void CheckForUniqueUserEmailAddressTest(IUserService userService)
        {
            var entity = _dbContext.UserViews.First();
            Assert.IsTrue(userService.CheckForUniqueUserEmailAddress(entity.UserId, entity.EmailAddress));
            Assert.IsTrue(userService.CheckForUniqueUserEmailAddress(entity.UserId, new Guid().ToString()));
            Assert.IsTrue(userService.CheckForUniqueUserEmailAddress(entity.UserId + 1, new Guid().ToString()));
            Assert.IsFalse(userService.CheckForUniqueUserEmailAddress(entity.UserId + 1, entity.EmailAddress));
        }

        [TestMethodDependencyInjection]
        public void GetUserKeyValuePairsTest(IUserService userService)
        {
            // Get models
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var keyValuePairs = userService.GetUserKeyValuePairs();

            stopWatch.Stop();
            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0} key/value pairs, {1} elapsed", keyValuePairs.Count, elapsedTime));

            // Check results
            Assert.AreNotEqual(0, keyValuePairs.Count);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 2);
#endif

            // Display results
            Console.WriteLine("UserId, FullName");
            foreach (var keyValuePair in keyValuePairs)
            {
                Console.WriteLine(string.Format("{0}, {1}", keyValuePair.Key, keyValuePair.Value));
            }
        }

        #endregion

        #region Private Methods

        private void GetUserTest(IUserService userService, int userId)
        {
            // Get entity
            var entity = _dbContext.UserViews.FirstOrDefault(x => x.UserId == userId);
            if (entity == null)
            {
                Console.WriteLine(string.Format("UserId {0} not found.", userId));
            }
            else
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                // Get model
                var model = userService.GetUser(entity.UserId);
                Assert.IsNotNull(model);

                stopWatch.Stop();

                // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

                var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
                Console.WriteLine(string.Format("{0}, {1}, {2}", entity.UserId, entity.Type, elapsedTime));
            }
        }

        private void CheckForUserAuditRecord(int userId, int userIdSource, AuditAction action)
        {
            var entities = _dbContext.UserAudits
                .Where(x => x.UserAuditUserId == userId
                    && x.UserAuditUserIdSource == userIdSource
                    && x.UserAuditActionId == (int)action);
            Assert.IsNotNull(entities);
            Assert.AreEqual(1, entities.Count());
            Console.WriteLine("audit record created.");
        }

        private int CreateUserTest(IUserService userService, UserModel model, int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = userService.CreateUser(model, userId, out string _);
            stopWatch.Stop();

            // Check results
            Assert.IsNotNull(result);
            model.UserId = result.UserId;
            model.UserGuid = result.UserGuid;
            UserTestHelper.Compare(model, result);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Display results
            Console.WriteLine(string.Format("UserId: {0}", result.UserId));
            Console.Write(string.Format("Create: {0}ms...", stopWatch.ElapsedMilliseconds));

            CheckForUserAuditRecord(result.UserId, userId, AuditAction.Create);

            return result.UserId;
        }

        private static void GetUserTest(IUserService userService, UserModel model, int newUserId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var getUserResult = userService.GetUser(newUserId);
            stopWatch.Stop();

            // Check results
            UserTestHelper.Compare(model, getUserResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.WriteLine(string.Format("Get: {0}ms", stopWatch.ElapsedMilliseconds));
        }

        private void UpdateUserTest(IUserService userService, UserModel model, int newUserId, int userId)
        {
            // Update properties
            var ticks = DateTime.Now.Ticks;
            model.Type = UserType.Client;
            model.EmailAddress = string.Format("{0}@demo.com", ticks);
            model.FirstName = string.Format("First-{0}", ticks);
            model.MiddleName = string.Format("Middle-{0}", ticks);
            model.LastName = string.Format("Last-{0}", ticks);
            model.AddressLine1 = string.Format("AddressLine1-{0}", ticks);
            model.AddressLine2 = string.Format("AddressLine2-{0}", ticks);
            model.City = string.Format("City-{0}", ticks);
            model.Region = string.Format("Region-{0}", ticks);
            model.PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6));
            model.Country = string.Format("Country-{0}", ticks);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var updateUserResult = userService.UpdateUser(model, userId, out string _);
            stopWatch.Stop();

            // Check results
            Assert.IsTrue(updateUserResult);
            var getUpdatedUserResult = userService.GetUser(newUserId);
            Assert.IsNotNull(getUpdatedUserResult);
            UserTestHelper.Compare(model, getUpdatedUserResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.Write(string.Format("Update: {0}ms...", stopWatch.ElapsedMilliseconds));

            CheckForUserAuditRecord(newUserId, userId, AuditAction.Update);
        }

        private void DeleteUserTest(IUserService userService, int testUserId, int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var deleteUserResult = userService.DeleteUser(testUserId, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsTrue(deleteUserResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            var getDeletedUserResult = userService.GetUser(testUserId);
            Assert.IsNotNull(getDeletedUserResult);
            Assert.IsTrue(getDeletedUserResult.IsDeleted);

            Console.Write(string.Format("Delete: {0}ms...", stopWatch.ElapsedMilliseconds));

            CheckForUserAuditRecord(testUserId, userId, AuditAction.Delete);
        }

        #endregion
    }
}