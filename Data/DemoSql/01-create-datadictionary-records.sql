-- Insert DataDictionary records

SET NOCOUNT ON;

PRINT 'Deleting existing records...';

-- Delete records and reseed identity
DELETE [dbo].[DataDictionaryAudit];
DBCC CHECKIDENT ('[DataDictionaryAudit]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[DataDictionaryGroupAudit];
DBCC CHECKIDENT ('[DataDictionaryGroupAudit]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[DataDictionary];
DBCC CHECKIDENT ('[DataDictionary]', RESEED, 0);

-- Delete records and reseed identity
DELETE [dbo].[DataDictionaryGroup];
DBCC CHECKIDENT ('[DataDictionaryGroup]', RESEED, 0);

PRINT 'Done!';

------------------------------------------------------------

DECLARE @AuditActionId INT = 1; -- Create
DECLARE @AuditUserId INT = 1;
DECLARE @Now DATETIME2 = GETDATE();
DECLARE @i INT = 1;

------------------------------------------------------------
-- DataDictionaryGroup
------------------------------------------------------------

-- Insert records
INSERT INTO [dbo].[DataDictionaryGroup] ([DataDictionaryGroupName], [DataDictionaryGroupIsActive]) VALUES ('AuditAction', 1);
INSERT INTO [dbo].[DataDictionaryGroup] ([DataDictionaryGroupName], [DataDictionaryGroupIsActive]) VALUES ('UserType', 1);
INSERT INTO [dbo].[DataDictionaryGroup] ([DataDictionaryGroupName], [DataDictionaryGroupIsActive]) VALUES ('ClientType', 1);
INSERT INTO [dbo].[DataDictionaryGroup] ([DataDictionaryGroupName], [DataDictionaryGroupIsActive]) VALUES ('WorkItemType', 1);
INSERT INTO [dbo].[DataDictionaryGroup] ([DataDictionaryGroupName], [DataDictionaryGroupIsActive]) VALUES ('WorkItemStatus', 1);

-- Display records
SELECT * FROM [dbo].[DataDictionaryGroup];

------------------------------------------------------------
-- DataDictionary
------------------------------------------------------------

-- Insert records - AuditAction
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (1, 'Create',	1, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (1, 'Update',	2, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (1, 'Delete',	3, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (1, 'Read',   4, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (1, 'Login',	5, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (1, 'Logout',	6, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (1, 'Error',	7, 1);

-- Insert records - UserType
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (2, 'Admin',		1, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (2, 'Developer',	2, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (2, 'Sales',		3, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (2, 'Marketing',	4, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (2, 'Accounting',	5, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (2, 'Executive',	6, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (2, 'Client',		7, 1);

-- Insert records - ClientType
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (3, 'Internal',	1, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (3, 'External',	2, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (3, 'Lead',		3, 1);

-- Insert records - WorkItemType
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryDescription], [DataDictionaryIsActive]) VALUES (4, 'User Story',	1, 'A feature or requirement from an end user''s perspective.',					1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryDescription], [DataDictionaryIsActive]) VALUES (4, 'Task',			2, 'A specific piece of work that needs to be completed.',						1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryDescription], [DataDictionaryIsActive]) VALUES (4, 'Bug',			3, 'A defect or issue that needs to be fixed.',									1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryDescription], [DataDictionaryIsActive]) VALUES (4, 'Epic',			4, 'A large body of work that can be broken down into multiple user stories.',	1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryDescription], [DataDictionaryIsActive]) VALUES (4, 'Feature',		5, 'A high-level functionality that delivers value to users.',					1);

-- Insert DataDictionary records - WorkItemStatus
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'New',			1, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'In Planning',	2, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'In Progress',	3, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'Approved',		4, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'Rejected',		5, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'Staged',			6, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'Completed',		7, 1);
INSERT INTO [dbo].[DataDictionary] ([DataDictionaryGroupId], [DataDictionaryKey], [DataDictionaryValue], [DataDictionaryIsActive]) VALUES (5, 'On Holde',		8, 1);

-- Display records
SELECT * FROM [dbo].[DataDictionary];

------------------------------------------------------------
-- DataDictionaryGroupAudit
------------------------------------------------------------

-- Insert records
ALTER TABLE [dbo].[DataDictionaryGroupAudit] NOCHECK CONSTRAINT [FK_DataDictionaryGroupAudit_User]
SET @i = 1;
WHILE @i <= (SELECT COUNT(DataDictionaryGroupId) FROM [dbo].DataDictionaryGroup)
BEGIN
    INSERT INTO [dbo].[DataDictionaryGroupAudit]
        (
        [DataDictionaryGroupAuditActionId]
        , [DataDictionaryGroupAuditDataDictionaryGroupId]
        , [DataDictionaryGroupAuditUserId]
        , [DataDictionaryGroupAuditDate]
        , [DataDictionaryGroupAuditBeforeJson]
        , [DataDictionaryGroupAuditAfterJson]
        , [DataDictionaryGroupAuditAffectedColumns])
        VALUES
        (
        @AuditActionId
        , @i
        , @AuditUserId
        , @Now
        , ''
        , (SELECT TOP 1 * FROM [dbo].DataDictionaryGroup WHERE DataDictionaryGroupId = @i FOR JSON PATH)
        , (STUFF((SELECT ',' + COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'DataDictionaryGroup' FOR XML PATH('')), 1, 1, ''))
        );
    SET @i = @i + 1;
END;
ALTER TABLE [dbo].[DataDictionaryGroupAudit] CHECK CONSTRAINT [FK_DataDictionaryGroupAudit_User]

-- Display records
SELECT * FROM [dbo].[DataDictionaryGroupAudit];

------------------------------------------------------------
-- DataDictionaryAudit
------------------------------------------------------------

-- Insert records
ALTER TABLE [dbo].[DataDictionaryAudit] NOCHECK CONSTRAINT [FK_DataDictionaryAudit_User]
SET @i = 1;
WHILE @i <= (SELECT COUNT(DataDictionaryId) FROM [dbo].DataDictionary)
BEGIN
    INSERT INTO [dbo].[DataDictionaryAudit]
        (
        [DataDictionaryAuditActionId]
        , [DataDictionaryAuditDataDictionaryId]
        , [DataDictionaryAuditUserId]
        , [DataDictionaryAuditDate]
        , [DataDictionaryAuditBeforeJson]
        , [DataDictionaryAuditAfterJson]
        , [DataDictionaryAuditAffectedColumns])
        VALUES
        (
        @AuditActionId
        , @i
        , @AuditUserId
        , @Now
        , ''
        , (SELECT TOP 1 * FROM [dbo].DataDictionary WHERE DataDictionaryId = @i FOR JSON PATH)
        , (STUFF((SELECT ',' + COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'DataDictionary' FOR XML PATH('')), 1, 1, ''))
        );
    SET @i = @i + 1;
END;
ALTER TABLE [dbo].[DataDictionaryAudit] CHECK CONSTRAINT [FK_DataDictionaryAudit_User]

-- Display records
SELECT * FROM [dbo].[DataDictionaryAudit];

SET NOCOUNT OFF;