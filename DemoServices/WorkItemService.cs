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
        /// Add a new client to the database.
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
        /// Get client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>WorkItemModel object.</returns>
        public WorkItemModel? GetWorkItem(int clientId)
        {
            var entity = _dbContext.WorkItems.Find(clientId);
            return GetModel(entity);
        }

        /// <summary>
        /// Get clients.
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
        /// Save a client.
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
                    // Check for unique client name
                    var clientNameIsUnique = CheckForUniqueWorkItemTitle(model.WorkItemId, model.Title);
                    if (!clientNameIsUnique)
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
        /// Delete a client from the database.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool DeleteWorkItem(int clientId, int userId)
        {
            var dbUpdated = false;

            var entity = _dbContext.WorkItems.Find(clientId);
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
        /// Check if the client name is unique.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="title"></param>
        /// <returns><c>true</c> if unique, otherwise <c>false</c>.</returns>
        public bool CheckForUniqueWorkItemTitle(int clientId, string title)
        {
            var entities = _dbContext.WorkItems.Where(x => x.WorkItemClientId != clientId && x.WorkItemTitle.ToLower() == title.ToLower().Trim());
            return !entities.Any();
        }

        /// <summary>
        /// Get client key/value pairs.
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
