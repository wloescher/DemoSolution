CREATE TABLE [dbo].[User]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserGuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [UserTypeId] INT NOT NULL,
    [UserEmailAddress] NVARCHAR(100) NOT NULL, 
	[UserIsActive] BIT NOT NULL DEFAULT 0, 
	[UserIsDeleted] BIT NOT NULL DEFAULT 0, 
    [UserPassword] NVARCHAR(50) NULL, 
    [UserFirstName] NVARCHAR(50) NULL, 
    [UserMiddleName] NVARCHAR(50) NULL, 
    [UserLastName] NVARCHAR(50) NULL, 

	CONSTRAINT [FK_User_DataDictionary] FOREIGN KEY ([UserTypeId])
		REFERENCES [dbo].[DataDictionary]([DataDictionaryId]),
)
