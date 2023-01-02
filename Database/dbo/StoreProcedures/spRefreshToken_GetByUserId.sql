CREATE PROCEDURE [dbo].[spRefreshToken_GetByUserId]
	@UserId int
	
AS
begin
    set nocount on;
	SELECT RefreshToken,Expiry from dbo.RefreshToken  where UserId=@UserId;
end;

