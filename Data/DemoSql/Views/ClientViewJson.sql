CREATE VIEW [dbo].[ClientViewJson] AS

	SELECT [id] = ClientId
		, [guid] = [Guid]
		, [typeId] = TypeId
		, [type] = [Type]
		, [isActive] = IsActive
		, [name] = [Name]
		, [addressLine1] = AddressLine1
		, [addressLine2] = AddressLine2
		, [city] = City
		, [region] = Region
		, [postalCode] = PostalCode
		, [country] = Country
		, [phoneNumber] = PhoneNumber
		, [url] = [Url]
		, [createdDate] = CreatedDate
		, [createdBy] = CreatedBy
		, [modifiedDate] = ModifiedDate
		, [modifiedBy] = ModifiedBy

	FROM [ClientView]