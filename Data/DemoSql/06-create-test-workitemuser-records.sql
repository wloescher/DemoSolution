-- Insert 100 random WorkItemUser records

-- ASSUMPTIONS
--You have at least 100 WorkItems (WorkItemId from 1–100).
--You have at least 100 Users (UserId from 1–100).
--You want to assign Users to WorkItems in a many-to-many style, avoiding duplicates

---- NOTES
-- IsActive and IsDeleted are randomized.

SET NOCOUNT ON;

PRINT 'Deleting existing records...';

-- Delete records and reseed identity
DELETE [dbo].[WorkItemUserAudit];
DBCC CHECKIDENT ('[WorkItemUserAudit]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[WorkItemUser];
DBCC CHECKIDENT ('[WorkItemUser]', RESEED, 0);

PRINT 'Done!';

------------------------------------------------------------

DECLARE @AuditActionId INT = 1; -- Create
DECLARE @AuditUserId INT = 1;
DECLARE @Now DATETIME2 = GETDATE();
DECLARE @RecordCount INT = 100;
DECLARE @i INT = 1;
PRINT 'Inserting ' + CAST(@RecordCount AS NVARCHAR) + ' WorkItemUser records...';

------------------------------------------------------------
-- WorkItemUser
------------------------------------------------------------

DECLARE @WorkItemId INT;
DECLARE @UserId INT;

-- Temp table to track used combinations
IF OBJECT_ID('tempdb..#UsedWorkItemUsers') IS NOT NULL DROP TABLE #UsedWorkItemUsers;
CREATE TABLE #UsedWorkItemUsers (
    WorkItemId INT,
    UserId INT
);

SET @i = 1;
WHILE @i <= @RecordCount
BEGIN
    -- Generate random WorkItemId (1 to 100) and UserId (1 to 100)
    SET @WorkItemId = ABS(CHECKSUM(NEWID())) % 100 + 1;
    SET @UserId = ABS(CHECKSUM(NEWID())) % 100 + 1;

    -- Check if combination already exists
    IF NOT EXISTS (
        SELECT 1 FROM #UsedWorkItemUsers WHERE WorkItemId = @WorkItemId AND UserId = @UserId
    )
    BEGIN
        -- Insert into actual table
        INSERT INTO [dbo].[WorkItemUser] (
            [WorkItemUserWorkItemId],
            [WorkItemUserUserId],
            [WorkItemUserIsDeleted]
        )
        VALUES (
            @WorkItemId,
            @UserId,
            ABS(CHECKSUM(NEWID())) % 2 -- Random 0 or 1
        );

        -- Track this combo to avoid duplicates
        INSERT INTO #UsedWorkItemUsers (WorkItemId, UserId)
        VALUES (@WorkItemId, @UserId);

        SET @i = @i + 1;
    END
END;

-- Display records
SELECT * FROM [dbo].[WorkItemUser];

------------------------------------------------------------
-- WorkItemUserAudit
------------------------------------------------------------

-- Insert records
SET @i = 1;
WHILE @i <= (SELECT COUNT(WorkItemUserId) FROM [dbo].[WorkItemUser])
BEGIN
    INSERT INTO [dbo].[WorkItemUserAudit]
        (
        [WorkItemUserAuditActionId]
        , [WorkItemUserAuditWorkItemUserId]
        , [WorkItemUserAuditUserId]
        , [WorkItemUserAuditDate]
        , [WorkItemUserAuditBeforeJson]
        , [WorkItemUserAuditAfterJson]
        , [WorkItemUserAuditAffectedColumns])
        VALUES
        (
        @AuditActionId
        , @i
        , @AuditUserId
        , @Now
        , ''
        , (SELECT TOP 1 * FROM [dbo].[WorkItemUser] WHERE WorkItemUserId = @i FOR JSON PATH)
        , (STUFF((SELECT ',' + COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'WorkItemUser' FOR XML PATH('')), 1, 1, ''))
        );
    SET @i = @i + 1;
END;

-- Display records
SELECT * FROM [dbo].[WorkItemUserAudit];

------------------------------------------------------------

PRINT 'Done!';
PRINT 'Inserted ' + CAST(@i - 1 AS NVARCHAR) + ' WorkItemUser records.';

SET NOCOUNT OFF;
