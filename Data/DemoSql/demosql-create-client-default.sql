SET IDENTITY_INSERT Client ON;

INSERT INTO Client (ClientId, ClientGuid, ClientTypeId, ClientName)
SELECT ClientId = 0, ClientGuid = CAST(0x0 AS UNIQUEIDENTIFIER), ClientTypeId = 1, ClientName = 'DefaultClient';

SET IDENTITY_INSERT Client OFF;