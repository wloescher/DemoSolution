CREATE TABLE [dbo].[Client]
(
	[ClientId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ClienttGuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [ClientTypeId] INT NOT NULL,
    [ClientName] NVARCHAR(100) NOT NULL, 
	[ClientIsActive] BIT NOT NULL DEFAULT 0, 
	[ClientIsDeleted] BIT NOT NULL DEFAULT 0, 
    [ClientUrl] NVARCHAR(150) NULL, 

    CONSTRAINT [FK_Client_DataDictionary] FOREIGN KEY ([ClientTypeId])
		REFERENCES [dbo].[DataDictionary]([DataDictionaryId])
)
