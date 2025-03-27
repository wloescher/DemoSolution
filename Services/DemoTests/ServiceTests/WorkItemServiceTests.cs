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
    public class WorkItemServiceTests : TestBase
    {
        #region Test Methods

        [TestMethodDependencyInjection]
        public void GetWorkItemTest(IWorkItemService workItemService)
        {
            Console.WriteLine("WorkItemId, Type, Elapsed");
            foreach (var userId in _testWorkItemIds)
            {
                GetWorkItemTest(workItemService, userId);
            }
        }

        [TestMethodDependencyInjection]
        public void GetWorkItemsTest(IWorkItemService workItemService)
        {
            // Get models
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var models = workItemService.GetWorkItems();

            stopWatch.Stop();
            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0} models, {1} elapsed", models.Count, elapsedTime));

            // Check results
            Assert.AreNotEqual(0, models.Count);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 2);
#endif
            // Display results
            Console.WriteLine("WorkItemId, Type, Title");
            foreach (var model in models)
            {
                Console.WriteLine(string.Format("{0}, {1}, {2}", model.WorkItemId, model.Type, model.Title));
            }
        }

        [TestMethodDependencyInjection]
        public void CrudTest(IWorkItemService workItemService)
        {
            var userId = _testUserIds.First();
            var ticks = DateTime.Now.Ticks;
            var model = new WorkItemModel
            {
                ClientId = _testClientIds.First(),
                Type = WorkItemType.UserStory,
                Status = WorkItemStatus.New,
                Title = string.Format("Title-{0}", ticks),
                SubTitle = string.Format("SubTitle-{0}", ticks),
                Summary = string.Format("Summary-{0}", ticks),
                Body = string.Format("Body-{0}", ticks),
            };

            var testWorkItemId = CreateWorkItemTest(workItemService, model, userId);
            GetWorkItemTest(workItemService, model, testWorkItemId);
            UpdateWorkItemTest(workItemService, model, testWorkItemId, userId);
            DeleteWorkItemTest(workItemService, testWorkItemId, userId);
        }

        [TestMethodDependencyInjection]
        public void CheckForUniqueWorkItemNameTest(IWorkItemService workItemService)
        {
            int workItemId = _testWorkItemIds.First();
            var model = workItemService.GetWorkItem(workItemId);
            Assert.IsNotNull(model);

            Assert.IsTrue(workItemService.CheckForUniqueTitle(workItemId, model.Title));
            Assert.IsTrue(workItemService.CheckForUniqueTitle(workItemId, new Guid().ToString()));
            Assert.IsTrue(workItemService.CheckForUniqueTitle(workItemId + 1, new Guid().ToString()));
            Assert.IsFalse(workItemService.CheckForUniqueTitle(workItemId + 1, model.Title));
        }

        [TestMethodDependencyInjection]
        public void GetWorkItemKeyValuePairsTest(IWorkItemService workItemService)
        {
            // Get models
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var keyValuePairs = workItemService.GetWorkItemKeyValuePairs();

            stopWatch.Stop();
            var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
            Console.WriteLine(string.Format("{0} key/value pairs, {1} elapsed", keyValuePairs.Count, elapsedTime));

            // Check results
            Assert.AreNotEqual(0, keyValuePairs.Count);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 2);
#endif

            // Display results
            Console.WriteLine("WorkItemId, Name");
            foreach (var keyValuePair in keyValuePairs)
            {
                Console.WriteLine(string.Format("{0}, {1}", keyValuePair.Key, keyValuePair.Value));
            }
        }

        [TestMethodDependencyInjection]
        public void GetWorkItemUsersTest(IWorkItemService workItemService)
        {
            var workItemId = _testWorkItemIds.First();
            var result = workItemService.GetWorkItemUsers(workItemId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void AddThenRemoveUserTest(IWorkItemService workItemService)
        {
            var workItemId = 0;
            var userId = 0;

            // Create user
            var result = workItemService.CreateWorkItemUser(workItemId, userId, userId);
            Assert.IsTrue(result);

            // Delete user
            result = workItemService.DeleteWorkItemUser(workItemId, userId, userId);
            Assert.IsTrue(result);
        }

        #endregion

        #region Private Methods

        private void GetWorkItemTest(IWorkItemService workItemService, int workItemId)
        {
            // Get entity
            var entity = CreateDbContext().WorkItemViews.FirstOrDefault(x => x.WorkItemId == workItemId);

            if (entity == null)
            {
                Console.WriteLine(string.Format("WorkItemId {0} not found.", workItemId));
            }
            else
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                // Get model
                var model = workItemService.GetWorkItem(entity.WorkItemId);
                Assert.IsNotNull(model);

                stopWatch.Stop();

                // Check results
#if !DEBUG
                Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

                var elapsedTime = DateTimeUtility.GetElapsedTime(stopWatch.Elapsed);
                Console.WriteLine(string.Format("{0}, {1}, {2}", entity.WorkItemId, entity.Type, elapsedTime));
            }
        }

        private void CheckForWorkItemAuditRecord(int workItemId, int userId, AuditAction action)
        {
            var entities = CreateDbContext().WorkItemAudits.Where(x => x.WorkItemAuditWorkItemId == workItemId
                && x.WorkItemAuditUserId == userId
                && x.WorkItemAuditActionId == (int)action);

            Assert.AreEqual(1, entities.Count());
            Console.WriteLine("Audit record created.");
        }

        private int CreateWorkItemTest(IWorkItemService workItemService, WorkItemModel model, int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = workItemService.CreateWorkItem(model, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsNotNull(result);
            model.WorkItemId = result.WorkItemId;
            model.WorkItemGuid = result.WorkItemGuid;
            CompareModels.Compare(model, result);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            // Display results
            Console.WriteLine(string.Format("WorkItemId: {0}", result.WorkItemId));
            Console.Write(string.Format("Create: {0}ms...", stopWatch.ElapsedMilliseconds));

            // TODO: Check for audit record
            // CheckForWorkItemAuditRecord(result.WorkItemId, userId, AuditAction.Create);

            return result.WorkItemId;
        }

        private static void GetWorkItemTest(IWorkItemService workItemService, WorkItemModel model, int newWorkItemId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var getWorkItemResult = workItemService.GetWorkItem(newWorkItemId);
            stopWatch.Stop();

            // Check results
            CompareModels.Compare(model, getWorkItemResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.WriteLine(string.Format("Get: {0}ms", stopWatch.ElapsedMilliseconds));
        }

        private void UpdateWorkItemTest(IWorkItemService workItemService, WorkItemModel model, int newWorkItemId, int userId)
        {
            // Update properties
            var ticks = DateTime.Now.Ticks;
            model.Type = WorkItemType.Bug;
            model.Status = WorkItemStatus.Approved;
            model.Title = string.Format("Title-{0}", ticks);
            model.SubTitle = string.Format("SubTitle-{0}", ticks);
            model.Summary = string.Format("Summary-{0}", ticks);
            model.Body = string.Format("Body-{0}", ticks);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var updateWorkItemResult = workItemService.UpdateWorkItem(model, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsTrue(updateWorkItemResult);
            var getUpdatedWorkItemResult = workItemService.GetWorkItem(newWorkItemId);
            Assert.IsNotNull(getUpdatedWorkItemResult);
            CompareModels.Compare(model, getUpdatedWorkItemResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            Console.Write(string.Format("Update: {0}ms...", stopWatch.ElapsedMilliseconds));

            // TODO: Check for audit record
            // CheckForWorkItemAuditRecord(newWorkItemId, userId, AuditAction.Update);
        }

        private void DeleteWorkItemTest(IWorkItemService workItemService, int testWorkItemId, int userId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var deleteWorkItemResult = workItemService.DeleteWorkItem(testWorkItemId, userId);
            stopWatch.Stop();

            // Check results
            Assert.IsTrue(deleteWorkItemResult);
#if !DEBUG
            Assert.IsTrue(stopWatch.Elapsed.TotalSeconds <= 1);
#endif

            var getDeletedWorkItemResult = workItemService.GetWorkItem(testWorkItemId);
            Assert.IsNotNull(getDeletedWorkItemResult);
            Assert.IsTrue(getDeletedWorkItemResult.IsDeleted);

            Console.Write(string.Format("Delete: {0}ms...", stopWatch.ElapsedMilliseconds));

            // TODO: Check for audit record
            // CheckForWorkItemAuditRecord(testWorkItemId, userId, AuditAction.Delete);
        }

        #endregion
    }
}