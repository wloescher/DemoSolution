CREATE VIEW ClientView AS
	
	SELECT ClientId
		, [Guid] = ClientGuid
		, TypeId = ClientTypeId
		, [Type] = ClientType.DataDictionaryKey
		, IsActive = ClientIsActive
		, [Name] = ClientName
		, AddressLine1 = ClientAddressLine1
		, AddressLine2 = ClientAddressLine2
		, City = ClientCity
		, Region = ClientRegion
		, PostalCode = ClientPostalCode
		, Country = ClientCountry
		, PhoneNumber = ClientPhoneNumber
		, [Url] = ClientUrl

	FROM Client
		LEFT JOIN DataDictionary AS ClientType ON DataDictionaryGroupId = 3 -- ClientType
			AND ClientTypeId = ClientType.DataDictionaryValue

	WHERE ClientIsDeleted = 0;