CREATE VIEW ClientView AS
	
	SELECT ClientId
		, [Guid] = ClienttGuid
		, TypeId = ClientTypeId
		, [Type] = ClientType.DataDictionaryKey
		, IsActive = ClientIsActive
		, [Name] = ClientName
		, [Address] = ClientAddress
		, City = ClientCity
		, Region = ClientRegion
		, PostalCode = ClientPostalCode
		, Country = ClientCountry
		, [Url] = ClientUrl

	FROM Client
		LEFT JOIN DataDictionary AS ClientType ON DataDictionaryGroupId = 3 -- ClientType
			AND ClientTypeId = ClientType.DataDictionaryValue

	WHERE ClientIsDeleted = 0;