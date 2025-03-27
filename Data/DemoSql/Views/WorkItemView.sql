CREATE VIEW WorkItemView AS
	
	SELECT WorkItemId
		, [Guid] = WorkItemGuid
		, ClientId = WorkItemClientId
		, ClientName
		, TypeId = WorkItemTypeId
		, [Type] = WorkItemType.DataDictionaryKey
		, StatusId = WorkItemStatusId
		, [Status] = WorkItemStatus.DataDictionaryKey
		, IsActive = WorkItemIsActive
		, Title = WorkItemTitle
		, SubTitle = WorkItemSubTitle
		, Summary = WorkItemSummary
		, Body = WorkItemBody
		, CreatedDate = IsNull(AuditCreated.WorkItemAuditDate, '1/1/1900')
		, CreatedBy = IsNull(AuditCreated.UserFullName, 'System')
		, ModifiedDate = IsNull(AuditModified.WorkItemAuditDate, IsNull(AuditCreated.WorkItemAuditDate, '1/1/1900'))
		, ModifiedBy = IsNull(AuditModified.UserFullName, IsNull(AuditCreated.UserFullName, 'System'))

	FROM WorkItem
		LEFT JOIN Client ON WorkItemClientId = ClientId
		LEFT JOIN DataDictionary AS WorkItemType ON WorkItemType.DataDictionaryGroupId = 4 -- WorkItemType
			AND WorkItemTypeId = WorkItemType.DataDictionaryValue
		LEFT JOIN DataDictionary AS WorkItemStatus ON WorkItemStatus.DataDictionaryGroupId = 5 -- WorkItemStatus
			AND WorkItemTypeId = WorkItemStatus.DataDictionaryValue
		OUTER APPLY 
			(
				SELECT TOP 1 WorkItemAuditDate
					, UserFullName = UserFirstName + ' ' + UserLastName
				FROM WorkItemAudit
					LEFT JOIN [User] ON WorkItemAuditUserId = UserId
				WHERE WorkItemAuditWorkItemId = WorkItemId
					AND WorkItemAuditActionId = 1 -- Created
				ORDER BY WorkItemAuditDate
			) AS AuditCreated
		OUTER APPLY 
			(
				SELECT TOP 1 WorkItemAuditDate
					, UserFullName = UserFirstName + ' ' + UserLastName
				FROM WorkItemAudit
					LEFT JOIN [User] ON WorkItemAuditUserId = UserId
				WHERE WorkItemAuditWorkItemId = WorkItemId
					AND WorkItemAuditActionId = 2 -- Update
				ORDER BY WorkItemAuditDate DESC
			) AS AuditModified

	WHERE WorkItemIsDeleted = 0;