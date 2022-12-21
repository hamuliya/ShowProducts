CREATE PROCEDURE [dbo].[spUser_GetByName]
   @UserName nvarchar(50)
As
begin
   declare @RoleId int;
   set nocount on;
	SELECT @RoleId= RoleId from dbo.[User]  where UserName=@UserName;
	if @RoleId is null
	begin
	 SELECT a.UserId,a.UserName,a.PasswordHash,a.Salt,a.EmailAddress,a.FirstName, a.LastName, a.CreateDate from dbo.[User] a where a.UserName=@UserName;
	end
	else
	begin
	 SELECT a.UserId,UserName,a.PasswordHash,a.Salt,a.EmailAddress,a.RoleId,a.FirstName, a.LastName, a.CreateDate,b.Role from dbo.[User] a,dbo.[Role] b where a.UserName=@UserName and a.RoleId=b.RoleId;
	end
end;



