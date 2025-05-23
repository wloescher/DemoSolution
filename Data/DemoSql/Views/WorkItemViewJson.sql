﻿CREATE VIEW [dbo].[WorkItemViewJson] AS

	SELECT [id] = [WorkItemId]
		, [guid] = [Guid]
		, [clientId] = [ClientId]
		, [clientName] = [ClientName]
		, [typeId] = [TypeId]
		, [type] = [Type]
		, [statusId] = [StatusId]
		, [status] = [Status]
		, [isActive] = [IsActive]
		, [title] = [Title]
		, [subTitle]= [SubTitle]
		, [summary] = [Summary]
		, [body] = [Body]
		, [createdDate] = [CreatedDate]
		, [createdBy] = [CreatedBy]
		, [modifiedDate] = [ModifiedDate]
		, [modifiedBy] = [ModifiedBy]
	FROM [dbo].[WorkItemView]
	FOR JSON PATH;
