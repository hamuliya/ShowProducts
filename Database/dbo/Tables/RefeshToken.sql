CREATE TABLE [dbo].[RefreshToken]
(
	[Id] INT NOT NULL identity, 
    [UserId] INT NOT NULL unique, 
    [RefreshToken] NVARCHAR(1000) NOT NULL unique, 
    [RefreshTokenExpiry] DATETIME NOT NULL, 
  
    CONSTRAINT [PK_Token] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_RefreshToken_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]), 
    


)
