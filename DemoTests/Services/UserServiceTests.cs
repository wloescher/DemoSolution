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

namespace DemoTests.Services
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
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x => x.UseCompatibilityLevel(100));
            _dbContext = new DemoSqlContext(optionsBuilder.Options);

            // Configuration Values
            _testUserIds = (configuration.GetValue<string>("Demo:TestUserIds") ?? string.Empty).Split(',').Select(int.Parse).ToList();
        }

        #region Test Methods

        [TestMethodDependencyInjection]
        public void GetUserTest(IUserService userService)
        {
            Console.WriteLine("UserId, UserType, Elapsed");
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
            var ticks = DateTime.Now.Ticks;
            var model = new UserModel
            {
                Type = UserType.Admin,
                EmailAddress = string.Format("{0}@demo.com", ticks),
                FirstName = string.Format("First-{0}", ticks),
                MiddleName = string.Format("Middle-{0}", ticks),
                LastName = string.Format("Last-{0}", ticks),
                Address = string.Format("Address-{0}", ticks),
                City = string.Format("City-{0}", ticks),
                Region = string.Format("Region-{0}", ticks),
                PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6)),
                Country = string.Format("Country-{0}", ticks),
            };
            var userIdSource = _dbContext.UserViews.First().UserId;

            // ------------------------------------------------------------
            // Create
            // ------------------------------------------------------------

            Console.Write("Create User...");

            var stopWatchCreate = new Stopwatch();
            stopWatchCreate.Start();
            var createUserResult = userService.CreateUser(model, userIdSource);
            stopWatchCreate.Stop();

            // Check results
            Assert.IsNotNull(createUserResult);
            var newUserId = createUserResult.UserId;
            model.UserId = newUserId;
            UserTestHelper.Compare(model, createUserResult);
#if !DEBUG
            Assert.IsTrue(stopWatchCreate.Elapsed.TotalSeconds <= 1);
#endif

            CheckForUserAuditRecord(newUserId, userIdSource, AuditAction.Create);

            // ------------------------------------------------------------
            // Get
            // ------------------------------------------------------------

            Console.WriteLine("Get User.");

            var stopWatchGet = new Stopwatch();
            stopWatchGet.Start();
            var getUserResult = userService.GetUser(newUserId);
            stopWatchGet.Stop();

            // Check results
            UserTestHelper.Compare(model, getUserResult);
#if !DEBUG
            Assert.IsTrue(stopWatchGet.Elapsed.TotalSeconds <= 1);
#endif

            // ------------------------------------------------------------
            // Update
            // ------------------------------------------------------------

            Console.Write("Update User...");

            // Update properties
            ticks = DateTime.Now.Ticks;
            model.Type = UserType.Client;
            model.EmailAddress = string.Format("{0}@demo.com", ticks);
            model.FirstName = string.Format("First-{0}", ticks);
            model.MiddleName = string.Format("Middle-{0}", ticks);
            model.LastName = string.Format("Last-{0}", ticks);
            model.Address = string.Format("Address-{0}", ticks);
            model.City = string.Format("City-{0}", ticks);
            model.Region = string.Format("Region-{0}", ticks);
            model.PostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6));
            model.Country = string.Format("Country-{0}", ticks);

            Assert.AreNotEqual(model.EmailAddress, createUserResult.EmailAddress);

            var stopWatchUpdate = new Stopwatch();
            stopWatchUpdate.Start();
            var updateUserResult = userService.UpdateUser(model, userIdSource);
            stopWatchGet.Stop();

            // Check results
            Assert.IsTrue(updateUserResult);
            var getUpdatedUserResult = userService.GetUser(newUserId);
            Assert.IsNotNull(getUpdatedUserResult);
            UserTestHelper.Compare(model, getUpdatedUserResult);
#if !DEBUG
            Assert.IsTrue(stopWatchUpdate.Elapsed.TotalSeconds <= 1);
#endif

            CheckForUserAuditRecord(newUserId, userIdSource, AuditAction.Update);

            // ------------------------------------------------------------
            // Delete
            // ------------------------------------------------------------

            Console.Write("Delete User...");

            var stopWatchDelete = new Stopwatch();
            stopWatchDelete.Start();
            var deleteUserResult = userService.DeleteUser(newUserId, userIdSource);
            stopWatchDelete.Stop();

            // Check results
            Assert.IsTrue(deleteUserResult);
#if !DEBUG
            Assert.IsTrue(stopWatchDelete.Elapsed.TotalSeconds <= 1);
#endif

            var getDeletedUserResult = userService.GetUser(newUserId);
            Assert.IsNotNull(getDeletedUserResult);
            Assert.IsTrue(getDeletedUserResult.IsDeleted);

            CheckForUserAuditRecord(newUserId, userIdSource, AuditAction.Delete);

            // ------------------------------------------------------------

            // Display results
            Console.WriteLine();
            Console.WriteLine(string.Format("UserId: {0}", newUserId));
            Console.WriteLine(string.Format("Type: {0}", createUserResult.Type));
            Console.WriteLine(string.Format("Create: {0}ms", stopWatchCreate.ElapsedMilliseconds));
            Console.WriteLine(string.Format("Get: {0}ms", stopWatchGet.ElapsedMilliseconds));
            Console.WriteLine(string.Format("Update: {0}ms", stopWatchUpdate.ElapsedMilliseconds));
            Console.WriteLine(string.Format("Delete: {0}ms", stopWatchDelete.ElapsedMilliseconds));
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
            Console.WriteLine("UserId, Type, FullName");
            foreach (var keyValuePair in keyValuePairs)
            {
                Console.WriteLine(string.Format("{0}, {1}", keyValuePair.Key, keyValuePair.Value));
            }
        }

        [TestMethodDependencyInjection]
        public void GetCurrentUserIdTest(IUserService userService)
        {
            Assert.Fail();
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
                .Where(x => x.UserAuditUserId == userId && x.UserAuditUserIdSource == userIdSource && x.UserAuditActionId == (int)action);
            Assert.IsNotNull(entities);
            Assert.AreEqual(1, entities.Count());
            Console.WriteLine("audit record created.");
        }

        #endregion
    }
}