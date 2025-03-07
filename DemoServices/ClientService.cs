﻿using DemoModels;
using DemoRepository.Entities;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static DemoModels.Enums;

namespace DemoServices
{
    public class ClientService(IConfiguration configuration, DemoSqlContext dbContext, IMemoryCache memoryCache, IServiceProvider serviceProvider)
        : DbContextService(configuration, dbContext, memoryCache, serviceProvider), IClientService
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
                ClientAddress = model.Address.Trim(),
                ClientCity = model.City.Trim(),
                ClientRegion = model.Region.Trim(),
                ClientPostalCode = model.PostalCode.Trim(),
                ClientCountry = model.Country.Trim(),
                ClientUrl = model.Url.Trim(),
            };

            _dbContext.Clients.Add(entity);
            bool dbUpdated = _dbContext.SaveChanges() > 0;

            if (dbUpdated)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Create audit record
                    var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                    auditService.CreateClient(entity, userId);
                }
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
            var entity = _dbContext.Clients.Find(clientId);
            return GetModel(entity);
        }

        /// <summary>
        /// Get clients.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeInternal"></param>
        /// <returns>Collection of ClientModel objects.</returns>
        public List<ClientModel> GetClients(bool activeOnly = true, bool excludeInternal = true)
        {
            var entities = new List<ClientView>();
            // Check for active
            if (activeOnly)
            {
                entities = _dbContext.ClientViews.Where(x => x.IsActive).ToList();
            }
            else
            {
                entities = _dbContext.ClientViews.ToList();
            }

            // Check for internal
            if (excludeInternal)
            {
                entities = entities.Where(x => x.TypeId != (int)ClientType.Internal).ToList();
            }

            var models = new List<ClientModel>();
            foreach (var entity in entities.OrderBy(x => x.Name))
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
        public bool UpdateClient(ClientModel model, int userId)
        {
            var dbUpdated = false;

            var entity = _dbContext.Clients.Find(model.ClientId);
            if (entity != null)
            {
                // Check for a name change
                if (entity.ClientName.ToLower().Trim() != model.Name.ToLower().Trim())
                {
                    // Check for unique client name
                    var clientNameIsUnique = CheckForUniqueClientName(model.ClientId, model.Name);
                    if (!clientNameIsUnique)
                    {
                        throw new ApplicationException("Client Name already exists - must be unique.");
                    }
                }

                // Get entity before update
                var entityBefore = entity;

                // Update entity property values
                entity.ClientIsActive = model.IsActive;
                entity.ClientName = model.Name.Trim();
                entity.ClientAddress = model.Address.Trim();
                entity.ClientCity = model.City.Trim();
                entity.ClientRegion = model.Region.Trim();
                entity.ClientPostalCode = model.PostalCode.Trim();
                entity.ClientCountry = model.Country.Trim();
                entity.ClientUrl = model.Url.Trim();

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.UpdateClient(entityBefore, entity, userId);
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
        public bool DeleteClient(int clientId, int userId)
        {
            var dbUpdated = false;

            var entity = _dbContext.Clients.Find(clientId);
            if (entity != null)
            {
                // Get entity before update
                var entityBefore = entity;

                // Update entity property values
                entity.ClientIsDeleted = true;

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.DeleteClient(entityBefore, entity, userId);
                    }
                }
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
            var entities = _dbContext.Clients.Where(x => x.ClientName.ToLower() == clientName.ToLower().Trim() && x.ClientId != clientId);
            return !entities.Any();
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
            var entities = (from clientUserView in _dbContext.ClientUserViews
                            join user in _dbContext.UserViews on clientUserView.UserId equals user.UserId
                            where clientUserView.ClientId == clientId
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
        /// Add a user to a client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <param name="userId_Source"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool CreateClientUser(int clientId, int userId, int userId_Source)
        {
            var dbUpdated = false;

            // Check for existing client user
            var entities = _dbContext.ClientUsers.Where(x => x.ClientUserClientId == clientId && x.ClientUserUserId == userId);
            if (entities.Count() == 0)
            {
                var entity = new ClientUser
                {
                    ClientUserClientId = clientId,
                    ClientUserUserId = userId,
                };

                _dbContext.ClientUsers.Add(entity);

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.CreateClientUser(entity, userId_Source);
                    }
                }
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

            var entity = _dbContext.ClientUsers.FirstOrDefault(x => x.ClientUserClientId == clientId && x.ClientUserUserId == userId);
            if (entity != null)
            {
                _dbContext.ClientUsers.Remove(entity);

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.DeleteClientUser(entity, userId_Source);
                    }
                }
            }

            return dbUpdated;
        }


        /// <summary>
        /// Get client work items.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Collection of WorkItemModel objects.</returns>
        public List<WorkItemModel> GetClientWorkItems(int clientId)
        {
            var entities = _dbContext.WorkItemViews.Where(x => x.ClientId == clientId);

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
                ClientGuid = entity.ClienttGuid,
                Type = (ClientType)entity.ClientTypeId,
                IsActive = entity.ClientIsActive,
                IsDeleted = entity.ClientIsDeleted,
                Name = entity.ClientName,
                Address = entity.ClientAddress,
                City = entity.ClientCity,
                Region = entity.ClientRegion,
                PostalCode = entity.ClientPostalCode,
                Country = entity.ClientCountry,
                Url = entity.ClientUrl,
            };

            return model;
        }

        private static ClientModel? GetModel(ClientView? entity)
        {
            if (entity == null) return null;

            var model = new ClientModel
            {
                ClientId = entity.ClientId,
                ClientGuid = entity.Guid,
                Type = (ClientType)entity.TypeId,
                IsActive = entity.IsActive,
                Name = entity.Name,
                Address = entity.Address,
                City = entity.City,
                Region = entity.Region,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
                Url = entity.Url,
            };

            return model;
        }

        #endregion
    }
}
