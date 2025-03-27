using DemoModels;
using DemoRepository.Entities;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static DemoModels.Enums;

namespace DemoServices
{
    public class WorkItemService(IConfiguration configuration, DemoSqlContext dbContext, IMemoryCache memoryCache, IServiceProvider serviceProvider)
        : DbContextService(configuration, dbContext, memoryCache, serviceProvider), IWorkItemService
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

            _dbContext.WorkItems.Add(entity);
            bool dbUpdated = _dbContext.SaveChanges() > 0;

            if (dbUpdated)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Create audit record
                    var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                    auditService.CreateWorkItem(entity, userId);
                }
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
            var entity = _dbContext.WorkItems.Find(workItemId);
            return GetModel(entity);
        }

        /// <summary>
        /// Get work items.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeInternal"></param>
        /// <returns>Collection of WorkItemModel objects.</returns>
        public List<WorkItemModel> GetWorkItems(bool activeOnly = true, bool excludeCompleted = true)
        {
            var entities = new List<WorkItemView>();
            // Check for active
            if (activeOnly)
            {
                entities = _dbContext.WorkItemViews.Where(x => x.IsActive).ToList();
            }
            else
            {
                entities = _dbContext.WorkItemViews.ToList();
            }

            // Check for completed
            if (excludeCompleted)
            {
                entities = entities.Where(x => x.TypeId != (int)WorkItemStatus.Completed).ToList();
            }

            var models = new List<WorkItemModel>();
            foreach (var entity in entities.OrderBy(x => x.Title))
            {
                var model = GetModel(entity);
                if (model != null)
                {
                    models.Add(model);
                }
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

            var entity = _dbContext.WorkItems.Find(model.WorkItemId);
            if (entity != null)
            {
                // Check for a name change
                if (entity.WorkItemTitle.ToLower().Trim() != model.Title.ToLower().Trim())
                {
                    // Check for unique title
                    var titleIsUnique = CheckForUniqueTitle(model.WorkItemId, model.Title);
                    if (!titleIsUnique)
                    {
                        throw new ApplicationException("Work Item Title already exists - must be unique.");
                    }
                }

                // Get entity before update
                var entityBefore = entity;

                // Update entity property values
                entity.WorkItemTypeId = (int)model.Type;
                entity.WorkItemStatusId = (int)model.Status;
                entity.WorkItemIsActive = model.IsActive;
                entity.WorkItemTitle = model.Title.Trim();
                entity.WorkItemSubTitle = model.SubTitle.Trim();
                entity.WorkItemSummary = model.Summary.Trim();
                entity.WorkItemBody = model.Body.Trim();

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.UpdateWorkItem(entityBefore, entity, userId);
                    }
                }
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

            var entity = _dbContext.WorkItems.Find(workItemId);
            if (entity != null)
            {
                // Get entity before update
                var entityBefore = entity;

                // Update entity property values
                entity.WorkItemIsDeleted = true;

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.DeleteWorkItem(entityBefore, entity, userId);
                    }
                }
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
            var entities = _dbContext.WorkItems.Where(x => x.WorkItemClientId != workItemId && x.WorkItemTitle.ToLower() == title.ToLower().Trim());
            return !entities.Any();
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
            var entities = (from workItemUserView in _dbContext.WorkItemUserViews
                            join user in _dbContext.UserViews on workItemUserView.UserId equals user.UserId
                            where workItemUserView.WorkItemId  == workItemId
                            select new { User = user });

            var models = new List<UserModel>();
            foreach (var entity in entities.OrderBy(x => x.User.FullName))
            {
                var model = UserService.GetModel(entity.User);
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

            // Check for existing work item user
            var entity = _dbContext.WorkItemUsers.FirstOrDefault(x => x.WorkItemUserClientId == workItemId && x.WorkItemUserUserId == userId);
            if (entity == null)
            {
                entity = new WorkItemUser
                {
                    WorkItemUserClientId = workItemId,
                    WorkItemUserUserId = userId,
                };

                _dbContext.WorkItemUsers.Add(entity);
            }
            else
            {
                entity.WorkItemUserIsDeleted = false;
            }

            dbUpdated = _dbContext.SaveChanges() > 0;

            if (dbUpdated)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Create audit record
                    var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                    auditService.CreateWorkItemUser(entity, userId_Source);
                }
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

            var entity = _dbContext.WorkItemUsers.FirstOrDefault(x => x.WorKItemUserClientId == workItemId && x.WorkItemUserUserId == userId);
            if (entity != null)
            {
                entity.WorkItemUserIsDeleted = true;
                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.DeleteWorkItemUser(entity, userId_Source);
                    }
                }
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
