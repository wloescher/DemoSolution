CREATE VIEW UserView AS
	
	SELECT UserId
		, [Guid] = UserGuid
		, TypeId = UserTypeId
		, [Type] = UserType.DataDictionaryKey
		, IsActive = UserIsActive
		, EmailAddress = UserEmailAddress
		, FirstName = UserFirstName
		, MiddleName = UserMiddleName
		, LastName = UserLastName
		, FullName = CASE
			WHEN Len(UserMiddleName) = 1 THEN UserFirstName + ' ' + UserMiddleName + '. ' + UserLastName
			WHEN Len(UserMiddleName) > 1 THEN UserFirstName + ' ' + UserMiddleName + ' ' + UserLastName
			ELSE UserFirstName + ' ' + UserLastName
			END
		, [Address] = UserAddress
		, City = UserCity
		, Region = UserRegion
		, PostalCode = UserPostalCode
		, Country = UserCountry

	FROM [User]
		LEFT JOIN DataDictionary AS UserType ON DataDictionaryGroupId = 2 -- UserType
			AND UserTypeId = UserType.DataDictionaryValue

	WHERE UserIsDeleted = 0;