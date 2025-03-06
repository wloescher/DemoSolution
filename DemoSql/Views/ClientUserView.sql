CREATE VIEW ClientUserView AS
	
	SELECT ClientUserId
		, ClientId = ClientUserClientId
		, UserId = ClientUserUserId
		, ClientName
		, UserFullName = UserFirstName + ' ' + UserLastName
		, UserEmailAddress

	FROM ClientUser
		LEFT JOIN Client ON ClientUserClientId = ClientId
			AND ClientIsActive = 1
			AND ClientIsDeleted = 0
		LEFT JOIN [User] ON ClientUserUserId = UserId
			AND UserIsActive = 1
			AND UserIsDeleted = 0;