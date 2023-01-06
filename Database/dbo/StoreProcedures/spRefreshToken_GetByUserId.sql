﻿CREATE PROCEDURE [dbo].[spRefreshToken_GetByUserId]
	@UserId int
	
AS
begin
    set nocount on;
	SELECT a.RefreshToken,a.RefreshTokenExpiry from dbo.RefreshToken a where UserId=@UserId;
end;

