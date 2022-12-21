
CREATE PROCEDURE [dbo].[spProduct_GetById]
   @ProductId nvarchar(128)
AS
begin
set nocount on;
  select ProductId,Title,UploadDate,Detail from dbo.[Product] where ProductId=@ProductId;
end;
	
RETURN 0

