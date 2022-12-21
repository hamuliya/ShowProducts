CREATE PROCEDURE [dbo].[spRefreshToken_UpdateByUserId]
	@UserId int,
	@RefreshToken NVARCHAR(1000),
	@Expiry Date
AS
begin
    set nocount on;
	Update dbo.RefreshToken set RefreshToken=@RefreshToken,Expiry=@Expiry where UserId=@UserId;
end

