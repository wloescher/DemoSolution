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
    public class ClientService(IDbContextFactory<DemoSqlContext> dbContextFactory, IMemoryCache memoryCache, IServiceProvider serviceProvider, IConfiguration configuration)
        : DbContextService(dbContextFactory, memoryCache, serviceProvider, configuration), IClientService
    {
        #region Public Methods

        /// <summary>
        /// Add a new client to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns>New ClientModel object.</returns>
        public ClientModel? CreateClient(ClientModel model, int userId)
        {
            var entity = new Client
            {
                ClientTypeId = (int)model.Type,
                ClientName = model.Name.Trim(),
                ClientAddressLine1 = model.AddressLine1?.Trim(),
                ClientAddressLine2 = model.AddressLine2?.Trim(),
                ClientCity = model.City?.Trim(),
                ClientRegion = model.Region?.Trim(),
                ClientPostalCode = model.PostalCode?.Trim(),
                ClientCountry = model.Country?.Trim(),
                ClientPhoneNumber = model.PhoneNumber?.Trim(),
                ClientUrl = model.Url?.Trim(),
            };

            bool dbUpdated;
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                dbContext.Clients.Add(entity);
                dbUpdated = dbContext.SaveChanges() > 0;
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.CreateClient(entity, userId);
            }

            return GetModel(entity);
        }

        /// <summary>
        /// Get client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>ClientModel object.</returns>
        public ClientModel? GetClient(int clientId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var entity = dbContext.Clients.Find(clientId);
            return GetModel(entity);
        }

        /// <summary>
        /// Get clients.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeInternal"></param>
        /// <param name="resetCache"></param>
        /// <returns>Collection of ClientModel objects.</returns>
        public List<ClientModel> GetClients(bool activeOnly = true, bool excludeInternal = true, bool resetCache = false)
        {
            // Get model from cache
            var cacheKey = string.Format("DemoClients{0}", !activeOnly ? "IncludingInactive" : string.Empty);
            var models = _memoryCache.Get(cacheKey) as List<ClientModel>;
            if (models == null || resetCache)
            {
                List<ClientView> entities;

                // Check for active
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    entities = activeOnly
                        ? dbContext.ClientViews.Where(x => x.IsActive).ToList()
                        : dbContext.ClientViews.ToList();
                }

                // Check for internal
                if (excludeInternal)
                {
                    entities = entities.Where(x => x.TypeId != (int)ClientType.Internal).ToList();
                }

                // Convert to models
                models = new List<ClientModel>();
                foreach (var entity in entities.OrderBy(x => x.Name))
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
        /// Save a client.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool UpdateClient(ClientModel model, int userId)
        {
            var dbUpdated = false;
            Client entityBefore = new();
            Client entityAfter = new();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                // Get entity
                entityBefore = dbContext.Clients.Find(model.ClientId) ?? new();
                if (entityBefore.ClientId != 0)
                {
                    // Check for a name change
                    if (entityBefore.ClientName.ToLower().Trim() != model.Name.ToLower().Trim())
                    {
                        // Check for unique client name
                        var clientNameIsUnique = CheckForUniqueClientName(model.ClientId, model.Name);
                        if (!clientNameIsUnique)
                        {
                            throw new ApplicationException("Client Name already exists - must be unique.");
                        }
                    }

                    // Update entity property values
                    entityAfter = entityBefore;
                    entityAfter.ClientIsActive = model.IsActive;
                    entityAfter.ClientName = model.Name.Trim();
                    entityAfter.ClientAddressLine1 = model.AddressLine1?.Trim();
                    entityAfter.ClientAddressLine2 = model.AddressLine2?.Trim();
                    entityAfter.ClientCity = model.City?.Trim();
                    entityAfter.ClientRegion = model.Region?.Trim();
                    entityAfter.ClientPostalCode = model.PostalCode?.Trim();
                    entityAfter.ClientCountry = model.Country?.Trim();
                    entityAfter.ClientPhoneNumber = model.PhoneNumber?.Trim();
                    entityAfter.ClientUrl = model.Url?.Trim();

                    dbUpdated = dbContext.SaveChanges() > 0;
                }
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.UpdateClient(entityBefore, entityAfter, userId);
            }

            return dbUpdated;
        }

        /// <summary>
        /// Delete a client from the database.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool DeleteClient(int clientId, int userId)
        {
            var dbUpdated = false;
            Client entityBefore = new();
            Client entityAfter = new();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                // Get entity
                entityBefore = dbContext.Clients.Find(clientId) ?? new();
                if (entityBefore.ClientId != 0)
                {
                    // Update entity property values
                    entityAfter = entityBefore;
                    entityAfter.ClientIsDeleted = true;

                    dbUpdated = dbContext.SaveChanges() > 0;

                }
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.DeleteClient(entityBefore, entityAfter, userId);
            }

            return dbUpdated;
        }

        /// <summary>
        /// Check if the client name is unique.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientName"></param>
        /// <returns><c>true</c> if unique, otherwise <c>false</c>.</returns>
        public bool CheckForUniqueClientName(int clientId, string clientName)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return !dbContext.Clients.Any(x => x.ClientId != clientId && x.ClientName.ToLower() == clientName.ToLower().Trim());
        }

        /// <summary>
        /// Get client key/value pairs.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeInternal"></param>
        /// <returns>Collection of key/value pairs.</returns>
        public List<KeyValuePair<int, string>> GetClientKeyValuePairs(bool activeOnly = true, bool excludeInternal = true)
        {
            var models = GetClients(activeOnly, excludeInternal);

            var keyValuePairs = new List<KeyValuePair<int, string>>();
            foreach (var model in models)
            {
                keyValuePairs.Add(new KeyValuePair<int, string>(model.ClientId, model.Name));
            }

            return keyValuePairs;
        }

        /// <summary>
        /// Get client users.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Collection of UserModel objects.</returns>
        public List<UserModel> GetClientUsers(int clientId)
        {
            // Get entities
            List<UserView> entities;
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entities = dbContext.ClientUserViews
                    .Join(dbContext.UserViews,
                        clientUserView => clientUserView.UserId,
                        userView => userView.UserId,
                        (clientUserView, userView) => new { ClientUserView = clientUserView, UserView = userView })
                    .Where(x => x.ClientUserView.ClientId == clientId)
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
        /// Add a user to a client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <param name="userId_Source"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool CreateClientUser(int clientId, int userId, int userId_Source)
        {
            var dbUpdated = false;
            ClientUser entity = new();

            // Check for existing client user
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entity = dbContext.ClientUsers.FirstOrDefault(x => x.ClientUserClientId == clientId && x.ClientUserUserId == userId) ?? new();
                if (entity.ClientUserId == 0)
                {
                    entity = new ClientUser
                    {
                        ClientUserClientId = clientId,
                        ClientUserUserId = userId,
                    };

                    dbContext.ClientUsers.Add(entity);
                }
                else
                {
                    entity.ClientUserIsDeleted = false;
                }

                dbUpdated = dbContext.SaveChanges() > 0;
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.CreateClientUser(entity, userId_Source);
            }

            return true;
        }

        /// <summary>
        /// Delete a user from a client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <param name="userId_Source"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool DeleteClientUser(int clientId, int userId, int userId_Source)
        {
            var dbUpdated = false;
            ClientUser entity = new();

            // Check for existing client user
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entity = dbContext.ClientUsers.FirstOrDefault(x => x.ClientUserClientId == clientId && x.ClientUserUserId == userId) ?? new();
                if (entity.ClientUserId != 0)
                {
                    entity.ClientUserIsDeleted = true;
                    dbUpdated = dbContext.SaveChanges() > 0;
                }
            }

            if (dbUpdated)
            {
                // Create audit record
                using var scope = _serviceProvider.CreateScope();
                var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                auditService.DeleteClientUser(entity, userId_Source);
            }

            return dbUpdated;
        }

        /// <summary>
        /// Get client work items.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="includeActiveOnly"></param>
        /// <returns>Collection of WorkItemModel objects.</returns>
        public List<WorkItemModel> GetClientWorkItems(int clientId, bool includeActiveOnly = true)
        {
            // Get entities
            List<WorkItemView> entities;
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                entities = includeActiveOnly
                    ? dbContext.WorkItemViews.Where(x => x.ClientId == clientId && x.IsActive).ToList()
                    : dbContext.WorkItemViews.Where(x => x.ClientId == clientId).ToList();
            }

            // Convert to models
            var models = new List<WorkItemModel>();
            foreach (var entity in entities.OrderBy(x => x.Title))
            {
                var model = WorkItemService.GetModel(entity);
                if (model != null)
                {
                    models.Add(model);
                }
            }
            return models;
        }

        #endregion

        #region Private Methods

        private static ClientModel? GetModel(Client? entity)
        {
            if (entity == null) return null;

            var model = new ClientModel
            {
                ClientId = entity.ClientId,
                ClientGuid = entity.ClientGuid,
                Type = (ClientType)entity.ClientTypeId,
                IsActive = entity.ClientIsActive,
                IsDeleted = entity.ClientIsDeleted,
                Name = entity.ClientName,
                AddressLine1 = entity.ClientAddressLine1,
                AddressLine2 = entity.ClientAddressLine2,
                City = entity.ClientCity,
                Region = entity.ClientRegion,
                PostalCode = entity.ClientPostalCode,
                Country = entity.ClientCountry,
                PhoneNumber = entity.ClientPhoneNumber,
                Url = entity.ClientUrl,
            };

            return model;
        }

        internal static ClientModel? GetModel(ClientView? entity)
        {
            if (entity == null) return null;

            var model = new ClientModel
            {
                ClientId = entity.ClientId,
                ClientGuid = entity.Guid,
                Type = (ClientType)entity.TypeId,
                IsActive = entity.IsActive,
                Name = entity.Name,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                City = entity.City,
                Region = entity.Region,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
                PhoneNumber = entity.PhoneNumber,
                Url = entity.Url,
            };

            return model;
        }

        #endregion
    }
}
