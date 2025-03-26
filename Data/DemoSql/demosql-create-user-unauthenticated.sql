SET IDENTITY_INSERT [User] ON;

INSERT INTO [User] (UserId, UserGuid, UserTypeId, UserEmailAddress, UserFirstName, UserLastName)
SELECT UserId = 0, UserGuid = CAST(0x0 AS UNIQUEIDENTIFIER), UserTypeId = 1, UserEmailAddress = '###@###.###', UserFirstName = 'Unauthenticated', UserLastName = 'User'

SET IDENTITY_INSERT [User] OFF;