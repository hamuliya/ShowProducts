CREATE TABLE [dbo].[Token]
(
	[Id] INT NOT NULL identity, 
    [UserId] INT NOT NULL unique, 
    [RefreshToken] NVARCHAR(1000) NOT NULL unique, 
    [RefreshTokenExpiry] DATETIME NOT NULL, 
    [AccessToken] NVARCHAR(1000) NOT NULL unique, 
    [AccessTokenExpiry] DATETIME NOT NULL, 
  
    CONSTRAINT [PK_Token] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Token_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]), 


)
