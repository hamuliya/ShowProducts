CREATE PROCEDURE [dbo].[spRefreshToken_Insert]
	@UserId int,
	@RefreshToken NVARCHAR(1000),
	@RefreshTokenExpiry Date,
	@AccessToken NVARCHAR(1000),
	@AccessTokenExpiry Date
AS
begin
    set nocount on;
	Insert into dbo.Token (UserId,RefreshToken,RefreshTokenExpiry,AccessToken,AccessTokenExpiry) 
	values(@UserId,@RefreshToken,@RefreshTokenExpiry,@AccessToken,@AccessTokenExpiry);
end
