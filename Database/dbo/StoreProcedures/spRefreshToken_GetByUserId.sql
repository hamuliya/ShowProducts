CREATE PROCEDURE [dbo].[spRefreshToken_GetByUserId]
	@UserId int
	
AS
begin
    set nocount on;
	SELECT RefreshTokenId,RefreshToken,Expiry from dbo.RefreshToken  where UserId=@UserId;
end;

