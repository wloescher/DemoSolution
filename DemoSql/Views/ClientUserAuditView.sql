CREATE VIEW ClientUserAuditView AS
	
	SELECT ClientUserAuditId
		, ActionId = ClientUserAuditActionId
		, [Action] = AuditAction.DataDictionaryKey
		, ClientUserId = ClientUserAuditClientUserId
		, ClientId = ClientUserAuditClientUserId
		, UserId = ClientUserAuditUserId
		, ClientName
		, UserFullName = UserFirstName + ' ' + UserLastName
		, UserEmailAddress
		, [Date] = ClientUserAuditDate
		, BeforeJson = ClientUserAuditBeforeJson
		, AfterJson = ClientUserAuditAfterJson
		, AffectedColumns = ClientUserAuditAffectedColumns

	FROM ClientUserAudit
		LEFT JOIN DataDictionary AS AuditAction ON DataDictionaryGroupId = 1 -- AuditAction
			AND ClientUserAuditActionId = AuditAction.DataDictionaryValue
		LEFT JOIN ClientUser ON ClientUserAuditClientUserId = ClientUserId
		LEFT JOIN Client ON ClientUserClientId = ClientId
		LEFT JOIN [User] ON ClientUserAuditUserId = UserId;