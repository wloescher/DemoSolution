-- Insert 100 random User records

-- ASSUMPTIONS
-- UserTypeId values exist in [dbo].[DataDictionary] (e.g., IDs 1–7).

-- NOTES
-- Unique emails like user1@example.com to user100@example.com.
-- Optional MiddleName and AddressLine2 are occasionally NULL.
-- Varying countries, phone numbers, and names.
-- Random active/deleted flags and password attempts.

SET NOCOUNT ON;

PRINT 'Deleting existing records...';

-- Delete records and reseed identity
DELETE [dbo].[UserAudit];
DBCC CHECKIDENT ('[UserAudit]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[User];
DBCC CHECKIDENT ('[User]', RESEED, 0);

PRINT 'Done!';

------------------------------------------------------------

DECLARE @AuditActionId INT = 1; -- Create
DECLARE @AuditUserId INT = 1;
DECLARE @Now DATETIME2 = GETDATE();
DECLARE @RecordCount INT = 100;
DECLARE @i INT = 1;
PRINT 'Inserting ' + CAST(@RecordCount AS NVARCHAR) + ' User records...';

------------------------------------------------------------
-- User
------------------------------------------------------------

ALTER TABLE [dbo].[DataDictionaryGroupAudit] NOCHECK CONSTRAINT [FK_DataDictionaryGroupAudit_User]
ALTER TABLE [dbo].[DataDictionaryAudit] NOCHECK CONSTRAINT [FK_DataDictionaryAudit_User]

-- Insert records
SET @i = 1;
WHILE @i <= @RecordCount
BEGIN
    INSERT INTO [dbo].[User] (
        [UserTypeId],
        [UserEmailAddress],
        [UserIsActive],
        [UserIsDeleted],
        [UserPassword],
        [UserFirstName],
        [UserMiddleName],
        [UserLastName],
        [UserAddressLine1],
        [UserAddressLine2],
        [UserCity],
        [UserRegion],
        [UserPostalCode],
        [UserCountry],
        [UserPhoneNumber],
        [UserPasswordHash],
        [UserPasswordAttemptCount]
    )
    VALUES (
        -- UserTypeId between 1 and 7
        ABS(CHECKSUM(NEWID())) % 7 + 1,
        -- Unique Email
        'user' + CAST(@i AS NVARCHAR) + '@example.com',
        -- IsActive: 0 or 1
        ABS(CHECKSUM(NEWID())) % 2,
        -- IsDeleted: 0 or 1
        ABS(CHECKSUM(NEWID())) % 2,
        -- Password
        'Pass' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS NVARCHAR),
        -- FirstName
        'FirstName' + CAST(@i AS NVARCHAR),
        -- MiddleName (optional)
        CASE WHEN ABS(CHECKSUM(NEWID())) % 3 = 0 THEN NULL ELSE 'M' + CAST(@i AS NVARCHAR) END,
        -- LastName
        'LastName' + CAST(@i AS NVARCHAR),
        -- AddressLine1
        '456 Elm St Apt ' + CAST(@i AS NVARCHAR),
        -- AddressLine2
        CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE 'Building ' + CAST(ABS(CHECKSUM(NEWID())) % 10 AS NVARCHAR) END,
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
        '+1-555-' + RIGHT('0000' + CAST(ABS(CHECKSUM(NEWID())) % 10000 AS NVARCHAR), 4),
        -- PasswordHash (simulated)
        CONVERT(NVARCHAR(255), NEWID()),
        -- PasswordAttemptCount
        ABS(CHECKSUM(NEWID())) % 3
    );

    SET @i = @i + 1;
END;
ALTER TABLE [dbo].[DataDictionaryGroupAudit] CHECK CONSTRAINT [FK_DataDictionaryGroupAudit_User]
ALTER TABLE [dbo].[DataDictionaryAudit] CHECK CONSTRAINT [FK_DataDictionaryAudit_User]

-- Display records
SELECT * FROM [dbo].[User];

------------------------------------------------------------
-- UserAudit
------------------------------------------------------------

-- Insert records
SET @i = 1;
WHILE @i <= (SELECT COUNT(UserId) FROM [dbo].[User])
BEGIN
    INSERT INTO [dbo].[UserAudit]
        (
        [UserAuditActionId]
        , [UserAuditUserId]
        , [UserAuditUserId_Source]
        , [UserAuditDate]
        , [UserAuditBeforeJson]
        , [UserAuditAfterJson]
        , [UserAuditAffectedColumns])
        VALUES
        (
        @AuditActionId
        , @i
        , @AuditUserId
        , @Now
        , ''
        , (SELECT TOP 1 * FROM [dbo].[User] WHERE UserId = @i FOR JSON PATH)
        , (STUFF((SELECT ',' + COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'User' FOR XML PATH('')), 1, 1, ''))
        );
    SET @i = @i + 1;
END;

-- Display records
SELECT * FROM [dbo].[UserAudit];

------------------------------------------------------------

PRINT 'Done!';
PRINT 'Inserted ' + CAST(@i - 1 AS NVARCHAR) + ' User records.';

SET NOCOUNT OFF;