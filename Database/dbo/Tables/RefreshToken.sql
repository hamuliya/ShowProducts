CREATE TABLE [dbo].[RefreshToken]
(
	 
    [UserId] INT NOT NULL unique, 
    [RefreshToken] NVARCHAR(1000) NOT NULL, 
    [Expiry] DATETIME NOT NULL, 
    CONSTRAINT [FK_RefreshToken_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]), 
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([UserId]),

)
