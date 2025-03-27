CREATE VIEW WorkItemUserView AS
	
	SELECT WorkItemUserId
		, WorkItemId = WorkItemUserWorkItemId
		, UserId = WorkItemUserUserId
		, WorkItemTitle
		, UserFullName = UserFirstName + ' ' + UserLastName
		, UserEmailAddress
		, CreatedDate = IsNull(AuditCreated.WorkItemUserAuditDate, '1/1/1900')
		, CreatedBy = IsNull(AuditCreated.UserFullName, 'System')
		, ModifiedDate = IsNull(AuditModified.WorkItemUserAuditDate, IsNull(AuditCreated.WorkItemUserAuditDate, '1/1/1900'))
		, ModifiedBy = IsNull(AuditModified.UserFullName, IsNull(AuditCreated.UserFullName, 'System'))

	FROM WorkItemUser
		LEFT JOIN WorkItem ON WorkItemUserWorkItemId = WorkItemId
			AND WorkItemIsActive = 1
			AND WorkItemIsDeleted = 0
		LEFT JOIN [User] ON WorkItemUserUserId = UserId
			AND UserIsActive = 1
			AND UserIsDeleted = 0
		OUTER APPLY 
			(
				SELECT TOP 1 WorkItemUserAuditDate
					, UserFullName = UserFirstName + ' ' + UserLastName
				FROM WorkItemUserAudit
					LEFT JOIN [User] ON WorkItemUserAuditUserId = UserId
				WHERE WorkItemUserAuditWorkItemUserId = WorkItemUserId
					AND WorkItemUserAuditActionId = 1 -- Created
				ORDER BY WorkItemUserAuditDate
			) AS AuditCreated
		OUTER APPLY 
			(
				SELECT TOP 1 WorkItemUserAuditDate
					, UserFullName = UserFirstName + ' ' + UserLastName
				FROM WorkItemUserAudit
					LEFT JOIN [User] ON WorkItemUserAuditUserId = UserId
				WHERE WorkItemUserAuditWorkItemUserId = WorkItemUserId
					AND WorkItemUserAuditActionId = 2 -- Update
				ORDER BY WorkItemUserAuditDate DESC
			) AS AuditModified
			
	WHERE WorkItemUserIsDeleted = 0;