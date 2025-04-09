CREATE VIEW [dbo].[UserViewJson] AS

	SELECT [id] = [UserId]
		, [guid] = [Guid]
		, [typeId] = [TypeId]
		, [type] = [Type] 
		, [isActive] = [IsActive]
		, [emailAddress] = [EmailAddress]
		, [firstName] = FirstName
		, [middleName] = MiddleName
		, [lastName] = LastName
		, [fullName] = FullName
		, [addressLine1] = AddressLine1
		, [addressLine2] = AddressLine2
		, [city] = City
		, [region] = Region
		, [postalCode] = PostalCode
		, [country] = Country
		, [phoneNumber] = PhoneNumber
		, [createdDate] = CreatedDate
		, [createdBy] = CreatedBy
		, [modifiedDate] = ModifiedDate
		, [modifiedBy] = ModifiedBy

	FROM [UserView]
	FOR JSON PATH;