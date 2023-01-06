CREATE PROCEDURE [dbo].[spRefreshToken_Insert]
	@UserId int,
	@RefreshToken NVARCHAR(1000),
	@RefreshTokenExpiry Date
	
AS
begin
    set nocount on;
	Insert into dbo.RefreshToken(UserId,RefreshToken,RefreshTokenExpiry) 
	values(@UserId,@RefreshToken,@RefreshTokenExpiry);
end
