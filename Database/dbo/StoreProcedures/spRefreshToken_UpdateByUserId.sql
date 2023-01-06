CREATE PROCEDURE [dbo].[spToken_UpdateByUserId]
	@UserId int,
	@RefreshToken NVARCHAR(1000),
	@RefreshTokenExpiry Date
	

AS
begin
    set nocount on;
	Update dbo.RefreshToken  set RefreshToken=@RefreshToken,RefreshTokenExpiry=@RefreshTokenExpiry
	 where UserId=@UserId;
end

