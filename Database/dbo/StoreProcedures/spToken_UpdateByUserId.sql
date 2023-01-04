CREATE PROCEDURE [dbo].[spToken_UpdateByUserId]
	@UserId int,
	@RefreshToken NVARCHAR(1000),
	@RefreshTokenExpiry Date,
	@AccessToken NVARCHAR(1000),
	@AccessTokenExpiry Date

AS
begin
    set nocount on;
	Update dbo.Token  set RefreshToken=@RefreshToken,RefreshTokenExpiry=@RefreshTokenExpiry,
	AccessToken=@AccessToken,AccessTokenExpiry=@AccessTokenExpiry where UserId=@UserId;
end

