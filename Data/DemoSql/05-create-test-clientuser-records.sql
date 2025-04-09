-- Insert 100 random ClientUser records

-- ASSUMPTIONS
--You have at least 100 clients (ClientId from 1–100).
--You have at least 100 users (UserId from 1–100).
--You want to assign users to clients in a many-to-many style, avoiding duplicates

---- NOTES
-- IsActive and IsDeleted are randomized.

SET NOCOUNT ON;

PRINT 'Deleting existing records...';

-- Delete records and reseed identity
DELETE [dbo].[ClientUserAudit];
DBCC CHECKIDENT ('[ClientUserAudit]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[ClientUser];
DBCC CHECKIDENT ('[ClientUser]', RESEED, 0);

PRINT 'Done!';

------------------------------------------------------------

DECLARE @AuditActionId INT = 1; -- Create
DECLARE @AuditUserId INT = 1;
DECLARE @Now DATETIME2 = GETDATE();
DECLARE @RecordCount INT = 100;
DECLARE @i INT = 1;
PRINT 'Inserting ' + CAST(@RecordCount AS NVARCHAR) + ' ClientUser records...';

------------------------------------------------------------
-- ClientUser
------------------------------------------------------------

DECLARE @ClientId INT;
DECLARE @UserId INT;

-- Temp table to track used combinations
IF OBJECT_ID('tempdb..#UsedClientUsers') IS NOT NULL DROP TABLE #UsedClientUsers;
CREATE TABLE #UsedClientUsers (
    ClientId INT,
    UserId INT
);

SET @i = 1;
WHILE @i <= @RecordCount
BEGIN
    -- Generate random ClientId (1 to 100) and UserId (1 to 100)
    SET @ClientId = ABS(CHECKSUM(NEWID())) % 100 + 1;
    SET @UserId = ABS(CHECKSUM(NEWID())) % 100 + 1;

    -- Check if combination already exists
    IF NOT EXISTS (
        SELECT 1 FROM #UsedClientUsers WHERE ClientId = @ClientId AND UserId = @UserId
    )
    BEGIN
        -- Insert into actual table
        INSERT INTO [dbo].[ClientUser] (
            [ClientUserClientId],
            [ClientUserUserId],
            [ClientUserIsDeleted]
        )
        VALUES (
            @ClientId,
            @UserId,
            ABS(CHECKSUM(NEWID())) % 2 -- Random 0 or 1
        );

        -- Track this combo to avoid duplicates
        INSERT INTO #UsedClientUsers (ClientId, UserId)
        VALUES (@ClientId, @UserId);

        SET @i = @i + 1;
    END
END;

-- Display records
SELECT * FROM [dbo].[ClientUser];

------------------------------------------------------------
-- ClientUserAudit
------------------------------------------------------------

-- Insert records
SET @i = 1;
WHILE @i <= (SELECT COUNT(ClientUserId) FROM [dbo].[ClientUser])
BEGIN
    INSERT INTO [dbo].[ClientUserAudit]
        (
        [ClientUserAuditActionId]
        , [ClientUserAuditClientUserId]
        , [ClientUserAuditUserId]
        , [ClientUserAuditDate]
        , [ClientUserAuditBeforeJson]
        , [ClientUserAuditAfterJson]
        , [ClientUserAuditAffectedColumns])
        VALUES
        (
        @AuditActionId
        , @i
        , @AuditUserId
        , @Now
        , ''
        , (SELECT TOP 1 * FROM [dbo].[ClientUser] WHERE ClientUserId = @i FOR JSON PATH)
        , (STUFF((SELECT ',' + COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'ClientUser' FOR XML PATH('')), 1, 1, ''))
        );
    SET @i = @i + 1;
END;

-- Display records
SELECT * FROM [dbo].[ClientUserAudit];

------------------------------------------------------------

PRINT 'Done!';
PRINT 'Inserted ' + CAST(@i - 1 AS NVARCHAR) + ' ClientUser records.';

SET NOCOUNT OFF;
