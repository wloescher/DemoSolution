﻿using DemoRepository.Entities;
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

        private bool CreateClientAudit(int clientId, int userId, AuditAction action, string beforeJson, string afterJson, List<string> affectedColumns)
        {
            // Create audit record
            _dbContext.ClientAudits.Add(new ClientAudit
            {
                ClientAuditClientId = clientId,
                ClientAuditUserId = userId,
                ClientAuditDate = DateTime.Now,
                ClientAuditActionId = (int)action,
                ClientAuditBeforeJson = beforeJson,
                ClientAuditAfterJson = afterJson,
                ClientAuditAffectedColumns = string.Join(",", affectedColumns),
            });

            return _dbContext.SaveChanges() > 0;
        }

        private bool CreateClientUserAudit(int clientUserId, int userId, AuditAction action, string beforeJson, string afterJson)
        {
            // Create audit record
            _dbContext.ClientUserAudits.Add(new ClientUserAudit
            {
                ClientUserAuditClientUserId = clientUserId,
                ClientUserAuditUserId = userId,
                ClientUserAuditDate = DateTime.Now,
                ClientUserAuditActionId = (int)action,
                ClientUserAuditBeforeJson = beforeJson,
                ClientUserAuditAfterJson = afterJson,
                ClientUserAuditAffectedColumns = string.Empty,
            });

            return _dbContext.SaveChanges() > 0;
        }

        #endregion
    }
}
