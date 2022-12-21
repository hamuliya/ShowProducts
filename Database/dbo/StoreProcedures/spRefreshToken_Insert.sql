CREATE PROCEDURE [dbo].[spRefreshToken_Insert]
	@UserId int,
	@RefreshToken NVARCHAR(1000),
	@Expiry Date
AS
begin
    set nocount on;
	Insert into dbo.RefreshToken (UserId,RefreshToken,Expiry) values(@UserId,@RefreshToken,@Expiry);
end
