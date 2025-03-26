using DemoModels;
using DemoRepository.Entities;
using DemoServices.BaseClasses;
using DemoServices.Interfaces;
using DemoUtilities;
using DemoUtilities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Text;
using static DemoModels.Enums;

namespace DemoServices
{
    public class UserService(IConfiguration configuration, DemoSqlContext dbContext, IMemoryCache memoryCache, IServiceProvider serviceProvider)
        : DbContextService(configuration, dbContext, memoryCache, serviceProvider), IUserService
    {
        #region Public Methods

        /// <summary>
        /// Get a user by username and password.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns>null if not found or password is incorrect.</returns>
        public UserModel? GetUser(string emailAddress, string password)
        {
            var entity = _dbContext.Users.FirstOrDefault(x => x.UserEmailAddress == emailAddress && !x.UserIsDeleted && x.UserIsActive);

            // Check password
            if (entity != null && SecurityUtility.PasswordHashVerify(password, entity.UserPasswordHash))
            {
                return GetModel(entity);
            }

            return null;
        }

        /// <summary>
        /// Get the currently logged in user's UserId.
        /// </summary>
        /// <returns>UserId of logged in User</returns>
        public int GetCurrentUserId(HttpContext httpContext)
        {
            var nameIdentifierClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return nameIdentifierClaim == null ? 0 : Convert.ToInt32(nameIdentifierClaim.Value);
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="errorMessage"></param>
        /// <returns>New UserModel object.</returns>
        public UserModel? CreateUser(UserModel model, int userId, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Check for existing User record
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.UserEmailAddress == model.EmailAddress.Trim());
            if (userEntity != null)
            {
                errorMessage = "User already exists - please try a different Email Address.";
                return null;
            }

            // Set password
            var password = SecurityUtility.GeneratePassword();

            var entity = new User
            {
                UserTypeId = (int)model.Type,
                UserIsActive = true,
                UserEmailAddress = model.EmailAddress.Trim(),
                UserPassword = password,
                UserFirstName = model.FirstName.Trim(),
                UserMiddleName = model.MiddleName.Trim(),
                UserLastName = model.LastName.Trim(),
                UserAddressLine1 = model.AddressLine1.Trim(),
                UserAddressLine2 = model.AddressLine2.Trim(),
                UserCity = model.City.Trim(),
                UserRegion = model.Region.Trim(),
                UserPostalCode = model.PostalCode.Trim(),
                UserCountry = model.Country.Trim(),
                UserPhoneNumber = model.PhoneNumber.Trim(),
                UserPasswordHash = SecurityUtility.PasswordHash(password),
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

                // Send access info email
                var emailBody = GetChangePasswordEmailBody(model.EmailAddress, password);
                var returnMessage = string.Empty;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailUtility = scope.ServiceProvider.GetRequiredService<IEmailUtility>();
                    emailUtility.SendMail(model.EmailAddress, "Access Information", emailBody, out returnMessage, true);
                }
            }

            // Update cached data
            GetUsers(true, false, true);

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
        /// <param name="resetCache"></param>
        /// <returns>Collection of UserModel objects.</returns>
        public List<UserModel> GetUsers(bool activeOnly = true, bool excludeInternal = true, bool resetCache = false)
        {
            // Get model from cache
            var cacheKey = string.Format("DemoUsers{0}", !activeOnly ? "IncludingInactive" : string.Empty);
            var models = _memoryCache.Get(cacheKey) as List<UserModel>;
            if (models == null || resetCache)
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

                models = new List<UserModel>();
                foreach (var entity in entities.OrderBy(x => x.LastName).ThenBy(x => x.FirstName))
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

            return models
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList();
        }

        /// <summary>
        /// Save a user.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns><c>true</c> if successful, otherwise <c>fale</c>.</returns>
        public bool UpdateUser(UserModel model, int userId, out string errorMessage)
        {
            errorMessage = string.Empty;

            var dbUpdated = false;

            // Check for duplicate real name
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.UserEmailAddress == model.EmailAddress.Trim() && x.UserId != userId);
            if (userEntity != null)
            {
                errorMessage = "User already exists - please try a different Email Address.";
                return false;
            }

            var entity = _dbContext.Users.Find(model.UserId);
            if (entity != null)
            {
                // Check for an email address change
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
                entity.UserTypeId = (int)model.Type;
                entity.UserIsActive = model.IsActive;
                entity.UserEmailAddress = model.EmailAddress.Trim();
                entity.UserFirstName = model.FirstName.Trim();
                entity.UserMiddleName = model.MiddleName.Trim();
                entity.UserLastName = model.LastName.Trim();
                entity.UserAddressLine1 = model.AddressLine1.Trim();
                entity.UserAddressLine2 = model.AddressLine2.Trim();
                entity.UserCity = model.City.Trim();
                entity.UserRegion = model.Region.Trim();
                entity.UserPostalCode = model.PostalCode.Trim();
                entity.UserCountry = model.Country.Trim();
                entity.UserPhoneNumber = model.PhoneNumber.Trim();

                dbUpdated = _dbContext.SaveChanges() > 0;

                if (dbUpdated)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Create audit record
                        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                        auditService.UpdateUser(entityBefore, entity, userId);
                    }

                    // Update cached data
                    GetUsers(true, false, true);
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

        #endregion

        #region Private Methods

        private static UserModel? GetModel(User? entity)
        {
            if (entity == null) return null;

            var model = new UserModel
            {
                UserId = entity.UserId,
                UserGuid = entity.UserGuid,
                Type = (UserType)entity.UserTypeId,
                IsActive = entity.UserIsActive,
                IsDeleted = entity.UserIsDeleted,
                EmailAddress = entity.UserEmailAddress,
                FirstName = entity.UserFirstName,
                MiddleName = entity.UserMiddleName,
                LastName = entity.UserLastName,
                AddressLine1 = entity.UserAddressLine1,
                AddressLine2 = entity.UserAddressLine2,
                City = entity.UserCity,
                Region = entity.UserRegion,
                PostalCode = entity.UserPostalCode,
                Country = entity.UserCountry,
            };

            return model;
        }

        internal static UserModel? GetModel(UserView? entity)
        {
            if (entity == null) return null;

            var model = new UserModel
            {
                UserId = entity.UserId,
                UserGuid = entity.Guid,
                Type = (UserType)entity.TypeId,
                IsActive = entity.IsActive,
                EmailAddress = entity.EmailAddress,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                City = entity.City,
                Region = entity.Region,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
            };

            return model;
        }

        #endregion

        #region Private Methods

        private string GetChangePasswordEmailBody(string username, string password)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<p>Here is your access information for demo.com.</p>");
            sb.AppendFormat("<p>You can sign into the website by visiting <a href=\"https://www.demo.com\">demo.com</a> and entering your information.</p>");
            sb.AppendFormat("<div><strong>Username:</strong> {0}</div>", username);
            sb.AppendFormat("<div><strong>Password:</strong> {0}</div>", password);
            return sb.ToString();
        }

        #endregion
    }
}
