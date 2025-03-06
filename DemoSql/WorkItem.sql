CREATE TABLE [dbo].[WorkItem]
(
	[WorkItemId] INT NOT NULL PRIMARY KEY, 
    [WorkItemGuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [WorkItemTypeId] INT NOT NULL,
    [WorkItemStatusId] INT NOT NULL,
	[WorkItemIsActive] BIT NOT NULL DEFAULT 0, 
	[WorkItemIsDeleted] BIT NOT NULL DEFAULT 0, 
	[WorkItemTitle] NVARCHAR(150) NULL, 
    [WorkItemSubTitle] NVARCHAR(150) NULL, 
    [WorkItemSummary] NVARCHAR(500) NULL, 
    [WorkItemBody] NVARCHAR(MAX) NULL, 

    CONSTRAINT [FK_WorkItem_DataDictionary_Type] FOREIGN KEY ([WorkItemTypeId])
		REFERENCES [dbo].[DataDictionary]([DataDictionaryId]),
    CONSTRAINT [FK_WorkItem_DataDictionary_Status] FOREIGN KEY ([WorkItemStatusId])
		REFERENCES [dbo].[DataDictionary]([DataDictionaryId]),
)
