CREATE VIEW WorkItemView AS
	
	SELECT WorkItemId
		, [Guid] = WorkItemGuid
		, TypeId = WorkItemTypeId
		, [Type] = WorkItemType.DataDictionaryKey
		, StatusId = WorkItemStatusId
		, [Status] = WorkItemStatus.DataDictionaryKey
		, IsActive = WorkItemIsActive
		, Title = WorkItemTitle
		, SubTitle = WorkItemSubTitle
		, Summary = WorkItemSummary
		, Body = WorkItemBody

	FROM WorkItem
		LEFT JOIN DataDictionary AS WorkItemType ON WorkItemType.DataDictionaryGroupId = 4 -- WorkItemType
			AND WorkItemTypeId = WorkItemType.DataDictionaryValue
		LEFT JOIN DataDictionary AS WorkItemStatus ON WorkItemStatus.DataDictionaryGroupId = 5 -- WorkItemStatus
			AND WorkItemTypeId = WorkItemStatus.DataDictionaryValue

	WHERE WorkItemIsDeleted = 0;