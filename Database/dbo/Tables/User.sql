CREATE TABLE [dbo].[User]
(
	[UserId] INT NOT NULL PRIMARY KEY identity, 
    [UserName] NVARCHAR(50) NOT NULL unique, 
    [PasswordHash] NVARCHAR(300) NOT NULL, 
    [Salt] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(100) NOT NULL, 
    [RoleId] INT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [CreateDate] DATETIME NOT NULL, 
    CONSTRAINT [FK_User_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [Role]([RoleId])
  
)



