-- Insert 100 random Client records

-- ASSUMPTIONS
-- Valid ClientTypeId values (e.g., 1 to 3) in [dbo].[DataDictionary] table.

-- NOTES
-- The ClientGuid and ClientId fields are auto-generated.
-- ClientName is prefixed with Client_001 through Client_100 to ensure uniqueness.
-- Addresses, cities, and phone numbers are randomly generated in a realistic pattern.
-- Optional fields (AddressLine2) are sometimes set to NULL.

SET NOCOUNT ON;

PRINT 'Deleting existing records...';

-- Delete records and reseed identity
DELETE [dbo].[ClientAudit];
DBCC CHECKIDENT ('[ClientAudit]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[Client];
DBCC CHECKIDENT ('[Client]', RESEED, 0);

PRINT 'Done!';

------------------------------------------------------------

DECLARE @AuditActionId INT = 1; -- Create
DECLARE @AuditUserId INT = 1;
DECLARE @Now DATETIME2 = GETDATE();
DECLARE @RecordCount INT = 100;
DECLARE @i INT = 1;
PRINT 'Inserting ' + CAST(@RecordCount AS NVARCHAR) + ' Client records...';

------------------------------------------------------------
-- Client
------------------------------------------------------------

-- Insert records
SET @i = 1;
WHILE @i <= @RecordCount
BEGIN
    INSERT INTO [dbo].[Client] (
        [ClientTypeId],
        [ClientName],
        [ClientIsActive],
        [ClientIsDeleted],
        [ClientAddressLine1],
        [ClientAddressLine2],
        [ClientCity],
        [ClientRegion],
        [ClientPostalCode],
        [ClientCountry],
        [ClientPhoneNumber],
        [ClientUrl]
    )
    VALUES (
        -- Random ClientTypeId between 1 and 3 (adjust if needed)
        ABS(CHECKSUM(NEWID())) % 3 + 1,
        -- Unique ClientName
        'Client_' + RIGHT('000' + CAST(@i AS NVARCHAR), 4),
        -- ClientIsActive: 0 or 1
        ABS(CHECKSUM(NEWID())) % 2,
        -- ClientIsDeleted: 0 or 1
        ABS(CHECKSUM(NEWID())) % 2,
        -- AddressLine1
        '123 Main St Apt ' + CAST(ABS(CHECKSUM(NEWID())) % 9999 + 1 AS NVARCHAR),
        -- AddressLine2
        CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE 'Suite ' + CAST(ABS(CHECKSUM(NEWID())) % 999 AS NVARCHAR) END,
        -- City
        'City_' + CAST(@i AS NVARCHAR),
        -- Region
        'Region_' + CAST(ABS(CHECKSUM(NEWID())) % 10 + 1 AS NVARCHAR),
        -- PostalCode
        CAST(ABS(CHECKSUM(NEWID())) % 90000 + 10000 AS NVARCHAR),
        -- Country
        CASE (ABS(CHECKSUM(NEWID())) % 5)
            WHEN 0 THEN 'USA'
            WHEN 1 THEN 'Canada'
            WHEN 2 THEN 'UK'
            WHEN 3 THEN 'Germany'
            ELSE 'Australia'
        END,
        -- PhoneNumber
        '+1-' + CAST(ABS(CHECKSUM(NEWID())) % 900 AS NVARCHAR) + '-' + CAST(ABS(CHECKSUM(NEWID())) % 9000 + 1000 AS NVARCHAR),
        -- URL
        'https://www.client' + CAST(@i AS NVARCHAR) + '.com'
    );

    SET @i = @i + 1;
END;

-- Display records
SELECT * FROM [dbo].[Client];

------------------------------------------------------------
-- UserAudit
------------------------------------------------------------

-- Insert records
SET @i = 1;
WHILE @i <= (SELECT COUNT(ClientId) FROM [dbo].[Client])
BEGIN
    INSERT INTO [dbo].[ClientAudit]
        (
        [ClientAuditActionId]
        , [ClientAuditClientId]
        , [ClientAuditUserId]
        , [ClientAuditDate]
        , [ClientAuditBeforeJson]
        , [ClientAuditAfterJson]
        , [ClientAuditAffectedColumns])
        VALUES
        (
        @AuditActionId
        , @i
        , @AuditUserId
        , @Now
        , ''
        , (SELECT TOP 1 * FROM [dbo].[Client] WHERE ClientId = @i FOR JSON PATH)
        , (STUFF((SELECT ',' + COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'Client' FOR XML PATH('')), 1, 1, ''))
        );
    SET @i = @i + 1;
END;

-- Display records
SELECT * FROM [dbo].[ClientAudit];

------------------------------------------------------------

PRINT 'Done!';
PRINT 'Inserted ' + CAST(@i - 1 AS NVARCHAR) + ' User records.';

SET NOCOUNT OFF;