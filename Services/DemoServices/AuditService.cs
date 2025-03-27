using DemoModels;
using DemoRepository.Entities;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using static DemoModels.Enums;

namespace DemoServices
{
    public class AuditService(IConfiguration configuration, DemoSqlContext dbContext, IMemoryCache memoryCache, IServiceProvider serviceProvider)
        : DbContextService(configuration, dbContext, memoryCache, serviceProvider), IAuditService
    {
        #region Public Methods

        public List<AuditModel> GetClientAudits(int workitemId)
        {
            var models = new List<AuditModel>();

            var entities = _dbContext.ClientAudits.Where(a => a.ClientAuditClientId == workitemId);
            foreach (var entity in entities)
            {
                models.Add(GetModel(entity));
            }

            return models;
        }

        public bool CreateClient(Client entity, int userId)
        {
            var entityBefore = new Client();
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entity);
            var affectedColumns = GetAffectedColumns(entityBefore, entity);
            return CreateClientAudit(entity.ClientId, userId, AuditAction.Create, beforeJson, afterJson, affectedColumns);
        }

        public bool UpdateClient(Client entityBefore, Client entityAfter, int userId)
        {
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            var affectedColumns = GetAffectedColumns(entityBefore, entityAfter);
            return CreateClientAudit(entityAfter.ClientId, userId, AuditAction.Update, beforeJson, afterJson, affectedColumns);
        }

        public bool DeleteClient(Client entityBefore, Client entityAfter, int userId)
        {
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            var affectedColumns = GetAffectedColumns(entityBefore, entityAfter);
            return CreateClientAudit(entityAfter.ClientId, userId, AuditAction.Delete, beforeJson, afterJson, affectedColumns);
        }

        public List<AuditModel> GetClientUserAudits(int workitemUserId)
        {
            var models = new List<AuditModel>();

            var entities = _dbContext.ClientUserAudits.Where(a => a.ClientUserAuditClientUserId == workitemUserId);
            foreach (var entity in entities)
            {
                models.Add(GetModel(entity));
            }

            return models;
        }

        public bool CreateClientUser(ClientUser entity, int userId)
        {
            var entityBefore = new ClientUser();
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entity);
            return CreateClientUserAudit(entity.ClientUserId, userId, AuditAction.Create, beforeJson, afterJson);
        }

        public bool DeleteClientUser(ClientUser entity, int userId)
        {
            var entityAfter = new ClientUser();
            var beforeJson = JsonConvert.SerializeObject(entity);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            return CreateClientUserAudit(entity.ClientUserId, userId, AuditAction.Delete, beforeJson, afterJson);
        }

        public List<AuditModel> GetUserAudits(int userId)
        {
            var models = new List<AuditModel>();

            var entities = _dbContext.UserAudits.Where(a => a.UserAuditUserId == userId);
            foreach (var entity in entities)
            {
                models.Add(GetModel(entity));
            }

            return models;
        }

        public bool CreateUser(User entity, int userId)
        {
            var entityBefore = new User();
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entity);
            var affectedColumns = GetAffectedColumns(entityBefore, entity);
            return CreateUserAudit(entity.UserId, userId, AuditAction.Create, beforeJson, afterJson, affectedColumns);
        }

        public bool UpdateUser(User entityBefore, User entityAfter, int userId)
        {
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            var affectedColumns = GetAffectedColumns(entityBefore, entityAfter);
            return CreateUserAudit(entityAfter.UserId, userId, AuditAction.Update, beforeJson, afterJson, affectedColumns);
        }

        public bool DeleteUser(User entityBefore, User entityAfter, int userId)
        {
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            var affectedColumns = GetAffectedColumns(entityBefore, entityAfter);
            return CreateUserAudit(entityAfter.UserId, userId, AuditAction.Delete, beforeJson, afterJson, affectedColumns);
        }

        public List<AuditModel> GetWorkItemAudits(int workItemId)
        {
            var models = new List<AuditModel>();

            var entities = _dbContext.WorkItemAudits.Where(a => a.WorkItemAuditWorkItemId == workItemId);
            foreach (var entity in entities)
            {
                models.Add(GetModel(entity));
            }

            return models;
        }

        public bool CreateWorkItem(WorkItem entity, int userId)
        {
            var entityBefore = new WorkItem();
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entity);
            var affectedColumns = GetAffectedColumns(entityBefore, entity);
            return CreateWorkItemAudit(entity.WorkItemId, userId, AuditAction.Create, beforeJson, afterJson, affectedColumns);
        }

        public bool UpdateWorkItem(WorkItem entityBefore, WorkItem entityAfter, int userId)
        {
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            var affectedColumns = GetAffectedColumns(entityBefore, entityAfter);
            return CreateWorkItemAudit(entityAfter.WorkItemId, userId, AuditAction.Update, beforeJson, afterJson, affectedColumns);
        }

        public bool DeleteWorkItem(WorkItem entityBefore, WorkItem entityAfter, int userId)
        {
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            var affectedColumns = GetAffectedColumns(entityBefore, entityAfter);
            return CreateWorkItemAudit(entityAfter.WorkItemId, userId, AuditAction.Delete, beforeJson, afterJson, affectedColumns);
        }

        public List<AuditModel> GetWorkItemUserAudits(int workItemUserId)
        {
            var models = new List<AuditModel>();

            var entities = _dbContext.WorkItemUserAudits.Where(a => a.WorkItemUserAuditWorkItemUserId == workItemUserId);
            foreach (var entity in entities)
            {
                models.Add(GetModel(entity));
            }

            return models;
        }

        public bool CreateWorkItemUser(WorkItemUser entity, int userId)
        {
            var entityBefore = new WorkItemUser();
            var beforeJson = JsonConvert.SerializeObject(entityBefore);
            var afterJson = JsonConvert.SerializeObject(entity);
            return CreateWorkItemUserAudit(entity.WorkItemUserId, userId, AuditAction.Create, beforeJson, afterJson);
        }

        public bool DeleteWorkItemUser(WorkItemUser entity, int userId)
        {
            var entityAfter = new WorkItemUser();
            var beforeJson = JsonConvert.SerializeObject(entity);
            var afterJson = JsonConvert.SerializeObject(entityAfter);
            return CreateWorkItemUserAudit(entity.WorkItemUserId, userId, AuditAction.Delete, beforeJson, afterJson);
        }

        #endregion

        #region Private Methods

        private static List<string> GetAffectedColumns(object before, object after)
        {
            // Get properties to compare
            var propertiesToCompare = before.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Loop through properties and create list of values that have changed
            var changedProperties = new List<string>();
            foreach (var propertyInfo in propertiesToCompare.Where(a => a.Name != "ExtendedFields"))
            {
                // Check to see if property value has changed, if so add to list
                if (ValueChangedBoolean(propertyInfo.GetValue(before, null), propertyInfo.GetValue(after, null)))
                {
                    changedProperties.Add(propertyInfo.Name);
                }
            }

            return changedProperties;
        }

        private static bool ValueChangedBoolean(object? beforePropertyValue, object? afterPropertyValue)
        {
            // Compare property values
            var propertyValueHasChanged = false;
            var selfValueComparer = beforePropertyValue as IComparable;
            if (beforePropertyValue == null && afterPropertyValue != null ||
                beforePropertyValue != null && afterPropertyValue == null)
            {
                propertyValueHasChanged = true; // one of the values is null
            }
            else if (selfValueComparer != null && selfValueComparer.CompareTo(afterPropertyValue) != 0)
            {
                propertyValueHasChanged = true; // the comparison using IComparable failed
            }
            else if (!Equals(beforePropertyValue, afterPropertyValue))
            {
                propertyValueHasChanged = true; // the comparison using Equals failed
            }

            // Check to see if property value has changed, if so add to list
            return propertyValueHasChanged;
        }

        private bool CreateClientAudit(int workitemId, int userId, AuditAction action, string beforeJson, string afterJson, List<string> affectedColumns)
        {
            // Create audit record
            _dbContext.ClientAudits.Add(new ClientAudit
            {
                ClientAuditClientId = workitemId,
                ClientAuditUserId = userId,
                ClientAuditDate = DateTime.Now,
                ClientAuditActionId = (int)action,
                ClientAuditBeforeJson = beforeJson,
                ClientAuditAfterJson = afterJson,
                ClientAuditAffectedColumns = string.Join(",", affectedColumns),
            });

            return _dbContext.SaveChanges() > 0;
        }

        private bool CreateClientUserAudit(int workitemUserId, int userId, AuditAction action, string beforeJson, string afterJson)
        {
            // Create audit record
            _dbContext.ClientUserAudits.Add(new ClientUserAudit
            {
                ClientUserAuditClientUserId = workitemUserId,
                ClientUserAuditUserId = userId,
                ClientUserAuditDate = DateTime.Now,
                ClientUserAuditActionId = (int)action,
                ClientUserAuditBeforeJson = beforeJson,
                ClientUserAuditAfterJson = afterJson,
                ClientUserAuditAffectedColumns = string.Empty,
            });

            return _dbContext.SaveChanges() > 0;
        }

        private bool CreateUserAudit(int userId, int userId_Source, AuditAction action, string beforeJson, string afterJson, List<string> affectedColumns)
        {
            // Create audit record
            _dbContext.UserAudits.Add(new UserAudit
            {
                UserAuditUserId = userId,
                UserAuditUserIdSource = userId_Source,
                UserAuditDate = DateTime.Now,
                UserAuditActionId = (int)action,
                UserAuditBeforeJson = beforeJson,
                UserAuditAfterJson = afterJson,
                UserAuditAffectedColumns = string.Join(",", affectedColumns),
            });

            return _dbContext.SaveChanges() > 0;
        }

        private bool CreateWorkItemAudit(int workItemId, int userId, AuditAction action, string beforeJson, string afterJson, List<string> affectedColumns)
        {
            // Create audit record
            _dbContext.WorkItemAudits.Add(new WorkItemAudit
            {
                WorkItemAuditWorkItemId = workItemId,
                WorkItemAuditUserId = userId,
                WorkItemAuditDate = DateTime.Now,
                WorkItemAuditActionId = (int)action,
                WorkItemAuditBeforeJson = beforeJson,
                WorkItemAuditAfterJson = afterJson,
                WorkItemAuditAffectedColumns = string.Join(",", affectedColumns),
            });

            return _dbContext.SaveChanges() > 0;
        }

        private bool CreateWorkItemUserAudit(int workItemUserId, int userId, AuditAction action, string beforeJson, string afterJson)
        {
            // Create audit record
            _dbContext.WorkItemUserAudits.Add(new WorkItemUserAudit
            {
                WorkItemUserAuditWorkItemUserId = workItemUserId,
                WorkItemUserAuditUserId = userId,
                WorkItemUserAuditDate = DateTime.Now,
                WorkItemUserAuditActionId = (int)action,
                WorkItemUserAuditBeforeJson = beforeJson,
                WorkItemUserAuditAfterJson = afterJson,
                WorkItemUserAuditAffectedColumns = string.Empty,
            });

            return _dbContext.SaveChanges() > 0;
        }

        private static AuditModel GetModel(ClientAudit entity)
        {
            return new AuditModel
            {
                AuditId = entity.ClientAuditId,
                Date = entity.ClientAuditDate,
                Action = (AuditAction)entity.ClientAuditActionId,
                BeforeJson = entity.ClientAuditBeforeJson,
                AfterJson = entity.ClientAuditAfterJson,
                AffectedColumns = entity.ClientAuditAffectedColumns.Split(',').ToList(),
            };
        }

        private static AuditModel GetModel(ClientUserAudit entity)
        {
            return new AuditModel
            {
                AuditId = entity.ClientUserAuditId,
                Date = entity.ClientUserAuditDate,
                Action = (AuditAction)entity.ClientUserAuditActionId,
                BeforeJson = entity.ClientUserAuditBeforeJson,
                AfterJson = entity.ClientUserAuditAfterJson,
                AffectedColumns = entity.ClientUserAuditAffectedColumns.Split(',').ToList(),
            };
        }

        private static AuditModel GetModel(UserAudit entity)
        {
            return new AuditModel
            {
                AuditId = entity.UserAuditId,
                Date = entity.UserAuditDate,
                Action = (AuditAction)entity.UserAuditActionId,
                BeforeJson = entity.UserAuditBeforeJson,
                AfterJson = entity.UserAuditAfterJson,
                AffectedColumns = entity.UserAuditAffectedColumns.Split(',').ToList(),
            };
        }

        private static AuditModel GetModel(WorkItemAudit entity)
        {
            return new AuditModel
            {
                AuditId = entity.WorkItemAuditId,
                Date = entity.WorkItemAuditDate,
                Action = (AuditAction)entity.WorkItemAuditActionId,
                BeforeJson = entity.WorkItemAuditBeforeJson,
                AfterJson = entity.WorkItemAuditAfterJson,
                AffectedColumns = entity.WorkItemAuditAffectedColumns.Split(',').ToList(),
            };
        }

        private static AuditModel GetModel(WorkItemUserAudit entity)
        {
            return new AuditModel
            {
                AuditId = entity.WorkItemUserAuditId,
                Date = entity.WorkItemUserAuditDate,
                Action = (AuditAction)entity.WorkItemUserAuditActionId,
                BeforeJson = entity.WorkItemUserAuditBeforeJson,
                AfterJson = entity.WorkItemUserAuditAfterJson,
                AffectedColumns = entity.WorkItemUserAuditAffectedColumns.Split(',').ToList(),
            };
        }

        #endregion
    }
}
