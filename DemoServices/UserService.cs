using DemoModels;
using DemoRepository.Entities;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using static DemoModels.Enums;

namespace DemoServices
{
    public class UserService(IConfiguration configuration, DemoSqlContext dbContext, IMemoryCache memoryCache, IServiceProvider serviceProvider)
        : DbContextService(configuration, dbContext, memoryCache, serviceProvider), IUserService
    {
        #region Public Methods

        /// <summary>
        /// Add a new user to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns>New UserModel object.</returns>
        public UserModel? CreateUser(UserModel model, int userId)
        {
            var entity = new User
            {
                UserTypeId = (int)model.Type,
                UserEmailAddress = model.EmailAddress.Trim(),
                UserFirstName = model.FirstName.Trim(),
                UserMiddleName = model.MiddleName.Trim(),
                UserLastName = model.LastName.Trim(),
                UserAddress = model.Address.Trim(),
                UserCity = model.City.Trim(),
                UserRegion = model.Region.Trim(),
                UserPostalCode = model.PostalCode.Trim(),
                UserCountry = model.Country.Trim(),
            };

            _dbContext.Users.Add(entity);
            bool dbUpdated = _dbContext.SaveChanges() > 0;

            if (dbUpdated)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Create audit record
                    var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                    auditService.CreateUser(entity, userId);
                }
            }

            return GetModel(entity);
        }

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>UserModel object.</returns>
        public UserModel? GetUser(int userId)
        {
            var entity = _dbContext.Users.Find(userId);
            return GetModel(entity);
        }

        /// <summary>
        /// Get users.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeInternal"></param>
        /// <returns>Collection of UserModel objects.</returns>
        public List<UserModel> GetUsers(bool activeOnly = true, bool excludeInternal = true)
        {
            var entities = new List<UserView>();
            // Check for active
            if (activeOnly)
            {
                entities = _dbContext.UserViews.Where(x => x.IsActive).ToList();
            }
            else
            {
                entities = _dbContext.UserViews.ToList();
            }

            // Check for internal
            if (excludeInternal)
            {
                entities = entities.Where(x => x.TypeId != (int)UserType.Client).ToList();
            }

            var models = new List<UserModel>();
            foreach (var entity in entities.OrderBy(x => x.LastName).ThenBy(x => x.FirstName))
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
        /// Save a user.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool UpdateUser(UserModel model, int userId)
        {
            var dbUpdated = false;

            var entity = _dbContext.Users.Find(model.UserId);
            if (entity != null)
            {
                // Check for a name change
                if (entity.UserEmailAddress != model.EmailAddress.ToLower().Trim())
                {
                    // Check for unique email address
                    var emailAddressIsUnique = CheckForUniqueUserEmailAddress(model.UserId, model.EmailAddress);
                    if (!emailAddressIsUnique)
                    {
                        throw new ApplicationException("Email Address already exists - must be unique.");
                    }
                }

                // Get entity before update
                var entityBefore = entity;

                // Update entity property values
                entity.UserIsActive = model.IsActive;
                entity.UserEmailAddress = model.EmailAddress.Trim();
                entity.UserFirstName = model.FirstName.Trim();
                entity.UserMiddleName = model.MiddleName.Trim();
                entity.UserLastName = model.LastName.Trim();
                entity.UserAddress = model.Address.Trim();
                entity.UserCity = model.City.Trim();
                entity.UserRegion = model.Region.Trim();
                entity.UserPostalCode = model.PostalCode.Trim();
                entity.UserCountry = model.Country.Trim();

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.UpdateUser(entityBefore, entity, userId);
                    }
                }
            }

            return dbUpdated;
        }

        /// <summary>
        /// Delete a user from the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userId_Source"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool DeleteUser(int userId, int userId_Source)
        {
            var dbUpdated = false;

            var entity = _dbContext.Users.Find(userId);
            if (entity != null)
            {
                // Get entity before update
                var entityBefore = entity;

                // Update entity property values
                entity.UserIsDeleted = true;

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.DeleteUser(entityBefore, entity, userId_Source);
                    }
                }
            }

            return dbUpdated;
        }

        /// <summary>
        /// Check if the user email address is unique.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emailAddress"></param>
        /// <returns><c>true</c> if unique, otherwise <c>false</c>.</returns>
        public bool CheckForUniqueUserEmailAddress(int userId, string emailAddress)
        {
            var entities = _dbContext.Users.Where(x => x.UserEmailAddress.ToLower() == emailAddress.ToLower().Trim() && x.UserId != userId);
            return !entities.Any();
        }

        /// <summary>
        /// Get user key/value pairs.
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <param name="excludeInternal"></param>
        /// <returns>Collection of key/value pairs.</returns>
        public List<KeyValuePair<int, string>> GetUserKeyValuePairs(bool activeOnly = true, bool excludeInternal = true)
        {
            var models = GetUsers(activeOnly, excludeInternal);

            var keyValuePairs = new List<KeyValuePair<int, string>>();
            foreach (var model in models)
            {
                keyValuePairs.Add(new KeyValuePair<int, string>(model.UserId, model.FullName));
            }

            return keyValuePairs;
        }

        /// <summary>
        /// Get the current user.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>Current UserId as Int</returns>
        public int GetCurrentUserId(HttpContext httpContext)
        {
            var nameIdentifierClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return nameIdentifierClaim == null ? 0 : Convert.ToInt32(nameIdentifierClaim.Value);
        }

        #endregion

        #region Private Methods

        private static UserModel? GetModel(User? entity)
        {
            if (entity == null) return null;

            var model = new UserModel
            {
                UserId = entity.UserId,
                Type = (UserType)entity.UserTypeId,
                IsActive = entity.UserIsActive,
                EmailAddress = entity.UserEmailAddress,
                FirstName = entity.UserFirstName,
                MiddleName = entity.UserMiddleName,
                LastName = entity.UserLastName,
                Address = entity.UserAddress,
                City = entity.UserCity,
                Region = entity.UserRegion,
                PostalCode = entity.UserPostalCode,
                Country = entity.UserCountry,
            };

            return model;
        }

        private static UserModel? GetModel(UserView? entity)
        {
            if (entity == null) return null;

            var model = new UserModel
            {
                UserId = entity.UserId,
                Type = (UserType)entity.TypeId,
                IsActive = entity.IsActive,
                EmailAddress = entity.EmailAddress,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Address = entity.Address,
                City = entity.City,
                Region = entity.Region,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
            };

            return model;
        }

        #endregion
    }
}
