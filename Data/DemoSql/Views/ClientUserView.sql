CREATE VIEW ClientUserView AS
	
	SELECT ClientUserId
		, ClientId = ClientUserClientId
		, UserId = ClientUserUserId
		, ClientName
		, UserFullName = UserFirstName + ' ' + UserLastName
		, UserEmailAddress
		, CreatedDate = IsNull(AuditCreated.ClientUserAuditDate, '1/1/1900')
		, CreatedBy = IsNull(AuditCreated.UserFullName, 'System')
		, ModifiedDate = IsNull(AuditModified.ClientUserAuditDate, IsNull(AuditCreated.ClientUserAuditDate, '1/1/1900'))
		, ModifiedBy = IsNull(AuditModified.UserFullName, IsNull(AuditCreated.UserFullName, 'System'))

	FROM ClientUser
		LEFT JOIN Client ON ClientUserClientId = ClientId
			AND ClientIsActive = 1
			AND ClientIsDeleted = 0
		LEFT JOIN [User] ON ClientUserUserId = UserId
			AND UserIsActive = 1
			AND UserIsDeleted = 0
		OUTER APPLY 
			(
				SELECT TOP 1 ClientUserAuditDate
					, UserFullName = UserFirstName + ' ' + UserLastName
				FROM ClientUserAudit
					LEFT JOIN [User] ON ClientUserAuditUserId = UserId
				WHERE ClientUserAuditClientUserId = ClientUserId
					AND ClientUserAuditActionId = 1 -- Created
				ORDER BY ClientUserAuditDate
			) AS AuditCreated
		OUTER APPLY 
			(
				SELECT TOP 1 ClientUserAuditDate
					, UserFullName = UserFirstName + ' ' + UserLastName
				FROM ClientUserAudit
					LEFT JOIN [User] ON ClientUserAuditUserId = UserId
				WHERE ClientUserAuditClientUserId = ClientUserId
					AND ClientUserAuditActionId = 2 -- Update
				ORDER BY ClientUserAuditDate DESC
			) AS AuditModified
			
	WHERE ClientUserIsDeleted = 0;