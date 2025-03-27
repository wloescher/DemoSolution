CREATE TABLE [dbo].[WorkItemUser]
(
	[WorkItemUserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [WorkItemUserWorkItemId] INT NOT NULL, 
    [WorkItemUserUserId] INT NOT NULL, 
	[WorkItemUserIsDeleted] BIT NOT NULL DEFAULT 0, 
	
    CONSTRAINT [FK_WorkItemUser_WorkItem] FOREIGN KEY ([WorkItemUserWorkItemId])
		REFERENCES [dbo].[WorkItem]([WorkItemId]),
	CONSTRAINT [FK_WorkItemUser_User] FOREIGN KEY ([WorkItemUserUserId])
		REFERENCES [dbo].[User]([UserId]),
    CONSTRAINT [UQ_WorkItemUser] UNIQUE ([WorkItemUserWorkItemId], [WorkItemUserUserId]),
)
