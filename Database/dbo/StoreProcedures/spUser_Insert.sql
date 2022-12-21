CREATE PROCEDURE [dbo].[spUser_Insert]
    @UserId int output,
	@UserName nvarchar(50),
	@PasswordHash nvarchar(300),
	@Salt nvarchar(50),
	@EmailAddress nvarchar(100),
	@FirstName nvarchar(50),
	@LastName nvarChar(50)
AS
begin
    set nocount on;
	insert into dbo.[User] (UserName,PasswordHash,Salt,EmailAddress,FirstName,LastName,CreateDate)
	values(@UserName,@PasswordHash,@Salt,@EmailAddress,@FirstName,@LastName,GETDATE());
	select @@IDENTITY;
end;



