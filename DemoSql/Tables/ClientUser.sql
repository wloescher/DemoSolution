CREATE TABLE [dbo].[ClientUser]
(
	[ClientUserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ClientUserClientId] INT NOT NULL, 
    [ClientUserUserId] INT NOT NULL, 

	CONSTRAINT [FK_ClientUser_Client] FOREIGN KEY ([ClientUserClientId])
		REFERENCES [dbo].[Client]([ClientId]),
	CONSTRAINT [FK_ClientUser_User] FOREIGN KEY ([ClientUserUserId])
		REFERENCES [dbo].[User]([UserId]),
    CONSTRAINT [UQ_ClientUser] UNIQUE ([ClientUserClientId], [ClientUserUserId]),
)
