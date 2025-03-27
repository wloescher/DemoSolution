CREATE TABLE [dbo].[WorkItemUserAudit]
(
	[WorkItemUserAuditId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [WorkItemUserAuditActionId] INT NOT NULL, 
    [WorkItemUserAuditWorkItemUserId] INT NOT NULL, 
    [WorkItemUserAuditUserId] INT NOT NULL, 
    [WorkItemUserAuditDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
	[WorkItemUserAuditBeforeJson] NVARCHAR(MAX) NOT NULL, 
	[WorkItemUserAuditAfterJson] NVARCHAR(MAX) NOT NULL,
	[WorkItemUserAuditAffectedColumns] NVARCHAR(MAX) NOT NULL,

	CONSTRAINT [FK_WorkItemUserAudit_DataDictionary] FOREIGN KEY ([WorkItemUserAuditActionId])
		REFERENCES [dbo].[DataDictionary]([DataDictionaryId]),
	CONSTRAINT [FK_WorkItemUserAudit_WorkItemUser] FOREIGN KEY ([WorkItemUserAuditWorkItemUserId])
		REFERENCES [dbo].[WorkItemUser]([WorkItemUserId]),
	CONSTRAINT [FK_WorkItemUserAudit_User] FOREIGN KEY ([WorkItemUserAuditUserId])
		REFERENCES [dbo].[User]([UserId]),
)
