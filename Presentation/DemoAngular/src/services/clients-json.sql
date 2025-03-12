SELECT id = ClientId,
    [guid] = [Guid],
    typeId = TypeId,
    [type] = [Type],
    isActive = IsActive,
    isDeleted = CONVERT(BIT, 0),
    [name] = [Name],
    addressLine1 = IsNull(AddressLine1, ''),
    addressLine2 = IsNull(AddressLine2, ''),
    city = IsNull(City, ''),
    region = IsNull(Region, ''),
    postalCode = IsNull(PostalCode, ''),
    country = IsNull(Country, ''),
    phoneNumber = IsNull(PhoneNumber, '')
    [url] = IsNull([Url], '')
FROM ClientView FOR JSON PATH
