-- Insert 1000 random WorkItem records

-- ASSUMPTIONS
-- You already have at least 50 valid ClientId values in [dbo].[Client] (e.g., 1–50).
-- Valid WorkItemTypeId values exist in [dbo].[DataDictionary] (e.g., 1–5).
-- Valid WorkItemStatusId values exist in [dbo].[DataDictionary] (e.g., 1–8).

---- NOTES
-- Titles are unique per Client (e.g., WorkItem_2_45), satisfying the composite unique constraint.
-- IsActive and IsDeleted are randomized.
-- Summary and Body are filled with realistic placeholder text.
-- You can adjust the number of available ClientId, TypeId, and StatusId as needed.

SET NOCOUNT ON;

PRINT 'Deleting existing records...';

-- Delete records and reseed identity
DELETE [dbo].[WorkItemAudit];
DBCC CHECKIDENT ('[WorkItemAudit]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[WorkItem];
DBCC CHECKIDENT ('[WorkItem]', RESEED, 0);

PRINT 'Done!';

------------------------------------------------------------

DECLARE @AuditActionId INT = 1; -- Create
DECLARE @AuditUserId INT = 1;
DECLARE @Now DATETIME2 = GETDATE();
DECLARE @RecordCount INT = 1000;
DECLARE @i INT = 1;
PRINT 'Inserting ' + CAST(@RecordCount AS NVARCHAR) + ' WorkItem records...';

------------------------------------------------------------
-- WorkItem
------------------------------------------------------------

-- Insert records
SET @i = 1;
WHILE @i <= @RecordCount
BEGIN
    DECLARE @ClientId INT = ABS(CHECKSUM(NEWID())) % 50 + 1; -- assuming 50 clients
    DECLARE @TypeId INT = ABS(CHECKSUM(NEWID())) % 5 + 1;    -- assuming 5 types
    DECLARE @StatusId INT = ABS(CHECKSUM(NEWID())) % 5 + 1;  -- assuming 8 statuses
    DECLARE @UniqueTitle NVARCHAR(150) = 'WorkItem_' + CAST(@ClientId AS NVARCHAR) + '_' + CAST(@i AS NVARCHAR);

    INSERT INTO [dbo].[WorkItem] (
        [WorkItemClientId],
        [WorkItemTypeId],
        [WorkItemStatusId],
        [WorkItemIsActive],
        [WorkItemIsDeleted],
        [WorkItemTitle],
        [WorkItemSubTitle],
        [WorkItemSummary],
        [WorkItemBody]
    )
    VALUES (
        @ClientId,
        @TypeId,
        @StatusId,
        ABS(CHECKSUM(NEWID())) % 2, -- IsActive
        ABS(CHECKSUM(NEWID())) % 2, -- IsDeleted
        @UniqueTitle,
        'SubTitle for ' + @UniqueTitle,
        'This is a brief summary of ' + @UniqueTitle + '. It describes the work item briefly.',
        'This is the detailed body for ' + @UniqueTitle + '. It contains more comprehensive information about the work item. ' +
        'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.'
    );

    SET @i += 1;
END;

-- Display records
SELECT * FROM [dbo].[User];

------------------------------------------------------------
-- WorkItemAudit
------------------------------------------------------------

-- Insert records
SET @i = 1;
WHILE @i <= (SELECT COUNT(WorkItemId) FROM [dbo].[WorkItem])
BEGIN
    INSERT INTO [dbo].[WorkItemAudit]
        (
        [WorkItemAuditActionId]
        , [WorkItemAuditWorkItemId]
        , [WorkItemAuditUserId]
        , [WorkItemAuditDate]
        , [WorkItemAuditBeforeJson]
        , [WorkItemAuditAfterJson]
        , [WorkItemAuditAffectedColumns])
        VALUES
        (
        @AuditActionId
        , @i
        , @AuditUserId
        , @Now
        , ''
        , (SELECT TOP 1 * FROM [dbo].[WorkItem] WHERE WorkItemId = @i FOR JSON PATH)
        , (STUFF((SELECT ',' + COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'WorkItem' FOR XML PATH('')), 1, 1, ''))
        );
    SET @i = @i + 1;
END;

-- Display records
SELECT * FROM [dbo].[WorkItemAudit];

------------------------------------------------------------

PRINT 'Done!';
PRINT 'Inserted ' + CAST(@i - 1 AS NVARCHAR) + ' WorkItem records.';

SET NOCOUNT OFF;
