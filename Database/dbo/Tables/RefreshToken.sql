CREATE TABLE [dbo].[RefreshToken]
(
	[RefreshTokenId] INT NOT NULL PRIMARY KEY identity, 
    [UserId] INT NOT NULL unique, 
    [RefreshToken] NVARCHAR(1000) NOT NULL, 
    [Expiry] DATETIME NOT NULL, 
    CONSTRAINT [FK_RefreshToken_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]),

)
