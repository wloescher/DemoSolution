using DemoModels;
using DemoRepository.Entities;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static DemoModels.Enums;

namespace DemoServices
{
    public class WorkItemService(IDbContextFactory<DemoSqlContext> dbContextFactory, IMemoryCache memoryCache, IServiceProvider serviceProvider, IConfiguration configuration)
        : DbContextService(dbContextFactory, memoryCache, serviceProvider, configuration), IWorkItemService
    {
        #region Public Methods

        /// <summary>
        /// Add a new work item to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns>New WorkItemModel object.</returns>
        public WorkItemModel? CreateWorkItem(WorkItemModel model, int userId)
        {
            var entity = new WorkItem
            {
                WorkItemClientId = model.ClientId,
                WorkItemTypeId = (int)model.Type,
                WorkItemStatusId = (int)model.Status,
                WorkItemTitle = model.Title.Trim(),
                WorkItemSubTitle = model.SubTitle,
                WorkItemSummary = model.Summary,
                WorkItemBody = model.Body,
            };

            bool dbUpdated;
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                dbContext.WorkItems.Add(entity);
                dbUpdated = dbContext.SaveChanges() > 0;
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.CreateWorkItem(entity, userId);
            }

            return GetModel(entity);
        }

        /// <summary>
        /// Get work item.
        /// </summary>
        /// <param name="workItemId"></param>
        /// <returns>WorkItemModel object.</returns>
        public WorkItemModel? GetWorkItem(int workItemId)
        {
            WorkItem? entity = null;
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entity = dbContext.WorkItems.Find(workItemId);
            }
            return GetModel(entity);
        }

        /// <summary>
        /// Get work items.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeInternal"></param>
        /// <param name="resetCache"></param>
        /// <returns>Collection of WorkItemModel objects.</returns>
        public List<WorkItemModel> GetWorkItems(bool activeOnly = true, bool excludeCompleted = true, bool resetCache = false)
        {
            // Get model from cache
            var cacheKey = string.Format("DemoWorkItems{0}", !activeOnly ? "IncludingInactive" : string.Empty);
            var models = _memoryCache.Get(cacheKey) as List<WorkItemModel>;
            if (models == null || resetCache)
            {
                var entities = new List<WorkItemView>();

                // Check for active
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    entities = activeOnly
                        ? dbContext.WorkItemViews.Where(x => x.IsActive).ToList()
                        : dbContext.WorkItemViews.ToList();
                }

                // Check for completed
                if (excludeCompleted)
                {
                    entities = entities.Where(x => x.TypeId != (int)WorkItemStatus.Completed).ToList();
                }

                // Convert to models
                models = new List<WorkItemModel>();
                foreach (var entity in entities.OrderBy(x => x.Title))
                {
                    var model = GetModel(entity);
                    if (model != null)
                    {
                        models.Add(model);
                    }
                }

                // Update cache
                _memoryCache.Set(cacheKey, models, _cacheOptions);
            }

            return models;
        }

        /// <summary>
        /// Save a work item.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool UpdateWorkItem(WorkItemModel model, int userId)
        {
            var dbUpdated = false;
            WorkItem entityBefore = new();
            WorkItem entityAfter = new();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entityBefore = dbContext.WorkItems.Find(model.WorkItemId) ?? new();
                if (entityBefore.WorkItemId != 0)
                {
                    // Check for a name change
                    if (entityBefore.WorkItemTitle.ToLower().Trim() != model.Title.ToLower().Trim())
                    {
                        // Check for unique title
                        var titleIsUnique = CheckForUniqueTitle(model.WorkItemId, model.Title);
                        if (!titleIsUnique)
                        {
                            throw new ApplicationException("Work Item Title already exists - must be unique.");
                        }
                    }

                    // Update entity property values
                    entityAfter = entityBefore;
                    entityAfter.WorkItemTypeId = (int)model.Type;
                    entityAfter.WorkItemStatusId = (int)model.Status;
                    entityAfter.WorkItemIsActive = model.IsActive;
                    entityAfter.WorkItemTitle = model.Title.Trim();
                    entityAfter.WorkItemSubTitle = model.SubTitle.Trim();
                    entityAfter.WorkItemSummary = model.Summary.Trim();
                    entityAfter.WorkItemBody = model.Body.Trim();

                    dbUpdated = dbContext.SaveChanges() > 0;
                }
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.UpdateWorkItem(entityBefore, entityAfter, userId);
            }

            return dbUpdated;
        }

        /// <summary>
        /// Delete a work item from the database.
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="userId"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool DeleteWorkItem(int workItemId, int userId)
        {
            var dbUpdated = false;
            WorkItem entityBefore = new();
            WorkItem entityAfter = new();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                // Get entity
                entityBefore = dbContext.WorkItems.Find(workItemId) ?? new();
                if (entityBefore.WorkItemId != 0)
                {
                    // Update entity property values
                    entityAfter = entityBefore;
                    entityAfter.WorkItemIsDeleted = true;

                    dbUpdated = dbContext.SaveChanges() > 0;
                }
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.DeleteWorkItem(entityBefore, entityAfter, userId);
            }

            return dbUpdated;
        }

        /// <summary>
        /// Check if the title is unique.
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="title"></param>
        /// <returns><c>true</c> if unique, otherwise <c>false</c>.</returns>
        public bool CheckForUniqueTitle(int workItemId, string title)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return !dbContext.WorkItems.Any(x => x.WorkItemClientId != workItemId && x.WorkItemTitle.ToLower() == title.ToLower().Trim());
        }

        /// <summary>
        /// Get work item key/value pairs.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeComplete"></param>
        /// <returns>Collection of key/value pairs.</returns>
        public List<KeyValuePair<int, string>> GetWorkItemKeyValuePairs(bool activeOnly = true, bool excludeComplete = true)
        {
            var models = GetWorkItems(activeOnly, excludeComplete);

            var keyValuePairs = new List<KeyValuePair<int, string>>();
            foreach (var model in models)
            {
                keyValuePairs.Add(new KeyValuePair<int, string>(model.WorkItemId, model.Title));
            }

            return keyValuePairs;
        }

        /// <summary>
        /// Get work item users.
        /// </summary>
        /// <param name="workItemId"></param>
        /// <returns>Collection of UserModel objects.</returns>
        public List<UserModel> GetWorkItemUsers(int workItemId)
        {
            // Get entities
            List<UserView> entities;
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entities = dbContext.WorkItemUserViews
                    .Join(dbContext.UserViews,
                        workItemUserView => workItemUserView.UserId,
                        userView => userView.UserId,
                        (workItemUserViews, userView) => new { WorkItemUserViews = workItemUserViews, UserView = userView })
                    .Where(x => x.WorkItemUserViews.WorkItemId == workItemId)
                    .Select(x => x.UserView)
                    .ToList();
            }

            // Convert to models
            var models = new List<UserModel>();
            foreach (var entity in entities.OrderBy(x => x.LastName).ThenBy(x => x.FirstName))
            {
                var model = UserService.GetModel(entity);
                if (model != null)
                {
                    models.Add(model);
                }
            }
            return models;
        }

        /// <summary>
        /// Add a user to a work item.
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="userId"></param>
        /// <param name="userId_Source"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool CreateWorkItemUser(int workItemId, int userId, int userId_Source)
        {
            var dbUpdated = false;
            WorkItemUser? entity;

            // Check for existing work item user
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entity = dbContext.WorkItemUsers.FirstOrDefault(x => x.WorkItemUserWorkItemId == workItemId && x.WorkItemUserUserId == userId);
                if (entity == null)
                {
                    entity = new WorkItemUser
                    {
                        WorkItemUserWorkItemId = workItemId,
                        WorkItemUserUserId = userId,
                    };

                    dbContext.WorkItemUsers.Add(entity);
                }
                else
                {
                    entity.WorkItemUserIsDeleted = false;
                }

                dbUpdated = dbContext.SaveChanges() > 0;
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.CreateWorkItemUser(entity, userId_Source);
            }

            return true;
        }

        /// <summary>
        /// Delete a user from a work item.
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="userId"></param>
        /// <param name="userId_Source"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool DeleteWorkItemUser(int workItemId, int userId, int userId_Source)
        {
            var dbUpdated = false;
            WorkItemUser entity;

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entity = dbContext.WorkItemUsers.FirstOrDefault(x => x.WorkItemUserWorkItemId == workItemId && x.WorkItemUserUserId == userId) ?? new();
                if (entity.WorkItemUserId != 0)
                {
                    entity.WorkItemUserIsDeleted = true;
                    dbUpdated = dbContext.SaveChanges() > 0;
                }
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.DeleteWorkItemUser(entity, userId_Source);
            }

            return dbUpdated;
        }

        #endregion

        #region Private Methods

        private static WorkItemModel? GetModel(WorkItem? entity)
        {
            if (entity == null) return null;

            var model = new WorkItemModel
            {
                WorkItemId = entity.WorkItemId,
                WorkItemGuid = entity.WorkItemGuid,
                ClientId = entity.WorkItemClientId,
                Type = (WorkItemType)entity.WorkItemTypeId,
                Status = (WorkItemStatus)entity.WorkItemStatusId,
                IsActive = entity.WorkItemIsActive,
                IsDeleted = entity.WorkItemIsDeleted,
                Title = entity.WorkItemTitle,
                SubTitle = entity.WorkItemSubTitle,
                Summary = entity.WorkItemSummary,
                Body = entity.WorkItemBody,
            };

            return model;
        }

        internal static WorkItemModel? GetModel(WorkItemView? entity)
        {
            if (entity == null) return null;

            var model = new WorkItemModel
            {
                WorkItemId = entity.WorkItemId,
                WorkItemGuid = entity.Guid,
                ClientId = entity.ClientId,
                Type = (WorkItemType)entity.TypeId,
                Status = (WorkItemStatus)entity.StatusId,
                IsActive = entity.IsActive,
                Title = entity.Title,
                SubTitle = entity.SubTitle,
                Summary = entity.Summary,
                Body = entity.Body,
            };

            return model;
        }

        #endregion
    }
}
