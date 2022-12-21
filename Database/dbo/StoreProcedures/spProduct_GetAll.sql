CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
begin
  set nocount on;
  select ProductId,Title,UploadDate,Detail from dbo.[Product];
end
	

