CREATE TABLE [dbo].[WorkItemAudit]
(
	[WorkItemAuditId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [WorkItemAuditActionId] INT NOT NULL, 
    [WorkItemAuditWorkItemId] INT NOT NULL, 
    [WorkItemAuditUserId] INT NOT NULL, 
    [WorkItemAuditDate] DATETIME2 NOT NULL, 
	[WorkItemAuditBeforeJson] NVARCHAR(MAX) NOT NULL, 
	[WorkItemAuditAfterJson] NVARCHAR(MAX) NOT NULL,
	[WorkItemAuditAffectedColumns] NVARCHAR(MAX) NOT NULL,

	CONSTRAINT [FK_WorkItemAudit_DataDictionary] FOREIGN KEY ([WorkItemAuditActionId])
		REFERENCES [dbo].[DataDictionary]([DataDictionaryId]),
	CONSTRAINT [FK_WorkItemAudit_WorkItem] FOREIGN KEY ([WorkItemAuditWorkItemId])
		REFERENCES [dbo].[WorkItem]([WorkItemId]),
	CONSTRAINT [FK_WorkItemAudit_User] FOREIGN KEY ([WorkItemAuditUserId])
		REFERENCES [dbo].[User]([UserId]),
)
