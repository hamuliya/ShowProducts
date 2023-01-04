CREATE PROCEDURE [dbo].[spToken_GetByUserId]
	@UserId int
	
AS
begin
    set nocount on;
	SELECT a.RefreshToken,a.RefreshTokenExpiry,a.AccessToken,a.AccessTokenExpiry from dbo.Token a where UserId=@UserId;
end;

