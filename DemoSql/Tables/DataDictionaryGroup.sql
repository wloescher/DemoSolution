CREATE TABLE [dbo].[DataDictionaryGroup]
(
	[DataDictionaryGroupId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DataDictionaryGroupName] NVARCHAR(50) NOT NULL,
	[DataDictionaryGroupIsActive] BIT NOT NULL DEFAULT 0, 
	[DataDictionaryGroupIsDeleted] BIT NOT NULL DEFAULT 0, 

    CONSTRAINT [UQ_DataDictionaryGroupName] UNIQUE ([DataDictionaryGroupName]),
)