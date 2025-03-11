SELECT id = ClientId,
    [guid] = [Guid],
    typeId = TypeId,
    [type] = [Type],
    isActive = IsActive,
    isDeleted = CONVERT(BIT, 0),
    [name] = [Name],
    [address] = IsNull([Address], ''),
    city = IsNull(City, ''),
    region = IsNull(Region, ''),
    postalCode = IsNull(PostalCode, ''),
    country = IsNull(Country, ''),
    [url] = IsNull([Url], '')
FROM ClientView FOR JSON PATH