CREATE TABLE [dbo].[ClientUserAudit]
(
	[ClientUserAuditId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ClientUserAuditActionId] INT NOT NULL, 
    [ClientUserAuditClientUserId] INT NOT NULL, 
    [ClientUserAuditUserId] INT NOT NULL, 
    [ClientUserAuditDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
	[ClientUserAuditBeforeJson] NVARCHAR(MAX) NOT NULL, 
	[ClientUserAuditAfterJson] NVARCHAR(MAX) NOT NULL,
	[ClientUserAuditAffectedColumns] NVARCHAR(MAX) NOT NULL,

	CONSTRAINT [FK_ClientUserAudit_DataDictionary] FOREIGN KEY ([ClientUserAuditActionId])
		REFERENCES [dbo].[DataDictionary]([DataDictionaryId]),
	CONSTRAINT [FK_ClientUserAudit_ClientUser] FOREIGN KEY ([ClientUserAuditClientUserId])
		REFERENCES [dbo].[ClientUser]([ClientUserId]),
	CONSTRAINT [FK_ClientUserAudit_User] FOREIGN KEY ([ClientUserAuditUserId])
		REFERENCES [dbo].[User]([UserId]),
)
