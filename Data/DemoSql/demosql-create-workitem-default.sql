SET IDENTITY_INSERT WorkItem ON;

INSERT INTO WorkItem (WorkItemId, WorkItemGuid, WorkItemClientId, WorkItemTypeId, WorkItemStatusId, WorkItemTitle)
SELECT WorkItemId = 0, WorkItemGuid = CAST(0x0 AS UNIQUEIDENTIFIER), WorkItemClientId = 0, WorkItemTypeId = 1, WorkItemStatusId = 1, WorkItemName = 'DefaultWorkItem';

SET IDENTITY_INSERT WorkItem OFF;