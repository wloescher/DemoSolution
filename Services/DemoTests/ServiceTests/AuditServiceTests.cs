using DemoRepository.Entities;
using DemoServices.Interfaces;
using DemoTests.BaseClasses;
using static DemoModels.Enums;

namespace DemoTests.ServiceTests
{
    [TestClass()]
    public class AuditServiceTests : TestBase
    {
        #region Test Methods

        [TestMethodDependencyInjection]
        public void GetClientAuditsTest(IAuditService auditService)
        {
            var clientId = _testClientIds.First();
            var result = auditService.GetClientAudits(clientId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void ClientCrudTest(IAuditService auditService)
        {
            var entity = new Client();
            var userId = _testUserIds.First();

            CreateClientTest(auditService, entity, userId);
            UpdateClientTest(auditService, entity, userId);
            DeleteClientTest(auditService, entity, userId);
        }

        [TestMethodDependencyInjection]
        public void GetClientUserAuditsTest(IAuditService auditService)
        {
            var clientUserId = _testClientUserIds.First();
            var result = auditService.GetClientUserAudits(clientUserId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void ClientUserCrudTest(IAuditService auditService)
        {
            var entity = new ClientUser { ClientUserId = 1, ClientUserClientId = 1, ClientUserUserId = 2 };
            Assert.IsTrue(auditService.CreateClientUser(entity, entity.ClientUserUserId));
            Assert.IsTrue(auditService.DeleteClientUser(entity, entity.ClientUserUserId));
        }

        [TestMethodDependencyInjection]
        public void GetUserAuditsTest(IAuditService auditService)
        {
            var userId = _testUserIds.First();
            var result = auditService.GetUserAudits(userId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void UserCrudTest(IAuditService auditService)
        {
            var userId = _testUserIds.First();
            var entity = new User();

            CreateUserTest(auditService, entity, userId);
            UpdateUserTest(auditService, entity, userId);
            DeleteUserTest(auditService, entity, userId);
        }

        [TestMethodDependencyInjection]
        public void GetWorkItemAuditsTest(IAuditService auditService)
        {
            var workItemId = _testWorkItemIds.First();
            var result = auditService.GetWorkItemAudits(workItemId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void WorkItemCrudTest(IAuditService auditService)
        {
            var userId = _testUserIds.First();
            var entity = new WorkItem();

            CreateWorkItemTest(auditService, entity, userId);
            UpdateWorkItemTest(auditService, entity, userId);
            DeleteWorkItemTest(auditService, entity, userId);
        }

        [TestMethodDependencyInjection]
        public void GetWorkItemUserAuditsTest(IAuditService auditService)
        {
            var workItemUserId = _testWorkItemUserIds.First();
            var result = auditService.GetWorkItemUserAudits(workItemUserId);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethodDependencyInjection]
        public void WorkitemUserCrudTest(IAuditService auditService)
        {
            var entity = new WorkItemUser { WorkItemUserId = 1, WorkItemUserWorkItemId = 1, WorkItemUserUserId = 2 };
            Assert.IsTrue(auditService.CreateWorkItemUser(entity, entity.WorkItemUserUserId));
            Assert.IsTrue(auditService.DeleteWorkItemUser(entity, entity.WorkItemUserUserId));
        }

        #endregion

        #region Private Methods

        private static void CreateClientTest(IAuditService auditService, Client entity, int userId)
        {
            var ticks = DateTime.Now.Ticks;
            entity.ClientTypeId = (int)ClientType.Lead;
            entity.ClientName = string.Format("Name-{0}", ticks);
            entity.ClientAddressLine1 = string.Format("AddressLine1-{0}", ticks);
            entity.ClientAddressLine2 = string.Format("AddressLine2-{0}", ticks);
            entity.ClientCity = string.Format("City-{0}", ticks);
            entity.ClientRegion = string.Format("Region-{0}", ticks);
            entity.ClientPostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6));
            entity.ClientCountry = string.Format("Country-{0}", ticks);
            entity.ClientPhoneNumber = string.Format("PhoneNumber-{0}", ticks);
            entity.ClientUrl = string.Format("Url-{0}", ticks);

            Assert.IsTrue(auditService.CreateClient(entity, userId));
        }

        private static void UpdateClientTest(IAuditService auditService, Client entity, int userId)
        {
            var entityBefore = entity;
            var entityAfter = entity;

            var ticks = DateTime.Now.Ticks;
            entityAfter.ClientTypeId = (int)ClientType.Lead;
            entityAfter.ClientName = string.Format("Name-{0}", ticks);
            entityAfter.ClientAddressLine1 = string.Format("AddressLine1-{0}", ticks);
            entityAfter.ClientAddressLine2 = string.Format("AddressLine2-{0}", ticks);
            entityAfter.ClientCity = string.Format("City-{0}", ticks);
            entityAfter.ClientRegion = string.Format("Region-{0}", ticks);
            entityAfter.ClientPostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6));
            entityAfter.ClientCountry = string.Format("Country-{0}", ticks);
            entityAfter.ClientPhoneNumber = string.Format("PhoneNumber-{0}", ticks);
            entityAfter.ClientUrl = string.Format("Url-{0}", ticks);

            Assert.IsTrue(auditService.UpdateClient(entityBefore, entityAfter, userId));
        }

        private static void DeleteClientTest(IAuditService auditService, Client entity, int userId)
        {
            var entityBefore = entity;
            var entityAfter = entity;
            entityAfter.ClientIsDeleted = true;

            Assert.IsTrue(auditService.DeleteClient(entityBefore, entityAfter, userId));
        }

        private static void CreateUserTest(IAuditService auditService, User entity, int userId)
        {
            var ticks = DateTime.Now.Ticks;
            entity.UserTypeId = (int)UserType.Accounting;
            entity.UserFirstName = string.Format("First-{0}", ticks);
            entity.UserMiddleName = string.Format("Middle-{0}", ticks);
            entity.UserLastName = string.Format("Last-{0}", ticks);
            entity.UserAddressLine1 = string.Format("AddressLine1-{0}", ticks);
            entity.UserAddressLine2 = string.Format("AddressLine2-{0}", ticks);
            entity.UserCity = string.Format("City-{0}", ticks);
            entity.UserRegion = string.Format("Region-{0}", ticks);
            entity.UserPostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6));
            entity.UserCountry = string.Format("Country-{0}", ticks);

            Assert.IsTrue(auditService.CreateUser(entity, userId));
        }

        private static void UpdateUserTest(IAuditService auditService, User entity, int userId)
        {
            var entityBefore = entity;
            var entityAfter = entity;

            var ticks = DateTime.Now.Ticks;
            entityAfter.UserTypeId = (int)UserType.Sales;
            entityAfter.UserFirstName = string.Format("First-{0}", ticks);
            entityAfter.UserMiddleName = string.Format("Middle-{0}", ticks);
            entityAfter.UserLastName = string.Format("Last-{0}", ticks);
            entityAfter.UserAddressLine1 = string.Format("AddressLine1-{0}", ticks);
            entityAfter.UserAddressLine2 = string.Format("AddressLine2-{0}", ticks);
            entityAfter.UserCity = string.Format("City-{0}", ticks);
            entityAfter.UserRegion = string.Format("Region-{0}", ticks);
            entityAfter.UserPostalCode = string.Format("Zip-{0}", ticks.ToString().Substring(ticks.ToString().Length - 6));
            entityAfter.UserCountry = string.Format("Country-{0}", ticks);

            Assert.IsTrue(auditService.UpdateUser(entityBefore, entityAfter, userId));
        }

        private static void DeleteUserTest(IAuditService auditService, User entity, int userId)
        {
            var entityBefore = entity;
            var entityAfter = entity;
            entityAfter.UserIsDeleted = true;

            Assert.IsTrue(auditService.DeleteUser(entityBefore, entityAfter, userId));
        }

        private static void CreateWorkItemTest(IAuditService auditService, WorkItem entity, int userId)
        {
            var ticks = DateTime.Now.Ticks;
            entity.WorkItemTypeId = (int)WorkItemType.UserStory;
            entity.WorkItemStatusId = (int)WorkItemStatus.New;
            entity.WorkItemTitle = string.Format("Title-{0}", ticks);
            entity.WorkItemSubTitle = string.Format("SubTitle-{0}", ticks);
            entity.WorkItemSummary = string.Format("Summary-{0}", ticks);
            entity.WorkItemBody = string.Format("Body-{0}", ticks);

            Assert.IsTrue(auditService.CreateWorkItem(entity, userId));
        }

        private static void UpdateWorkItemTest(IAuditService auditService, WorkItem entity, int userId)
        {
            var entityBefore = entity;
            var entityAfter = entity;

            var ticks = DateTime.Now.Ticks;
            entityAfter.WorkItemTypeId = (int)WorkItemType.UserStory;
            entityAfter.WorkItemStatusId = (int)WorkItemStatus.New;
            entityAfter.WorkItemTitle = string.Format("Title-{0}", ticks);
            entityAfter.WorkItemSubTitle = string.Format("SubTitle-{0}", ticks);
            entityAfter.WorkItemSummary = string.Format("Summary-{0}", ticks);
            entityAfter.WorkItemBody = string.Format("Body-{0}", ticks);

            Assert.IsTrue(auditService.UpdateWorkItem(entityBefore, entityAfter, userId));
        }

        private static void DeleteWorkItemTest(IAuditService auditService, WorkItem entity, int userId)
        {
            var entityBefore = entity;
            var entityAfter = entity;
            entityAfter.WorkItemIsDeleted = true;

            Assert.IsTrue(auditService.DeleteWorkItem(entityBefore, entityAfter, userId));
        }

        #endregion
    }
}