CREATE VIEW WorkItemAuditView AS
	
	SELECT WorkItemAuditId
		, ActionId = WorkItemAuditActionId
		, [Action] = AuditAction.DataDictionaryKey
		, WorkItemId = WorkItemAuditWorkItemId
		, UserId = WorkItemAuditUserId
		, WorkItemTitle
		, UserFullName = UserFirstName + ' ' + UserLastName
		, UserEmailAddress
		, [Date] = WorkItemAuditDate
		, BeforeJson = WorkItemAuditBeforeJson
		, AfterJson = WorkItemAuditAfterJson
		, AffectedColumns = WorkItemAuditAffectedColumns

	FROM WorkItemAudit
		LEFT JOIN DataDictionary AS AuditAction ON DataDictionaryGroupId = 1 -- AuditAction
			AND WorkItemAuditActionId = AuditAction.DataDictionaryValue
		LEFT JOIN WorkItem ON WorkItemAuditWorkItemId = WorkItemId
		LEFT JOIN [User] ON WorkItemAuditUserId = UserId;