CREATE PROCEDURE [dbo].[spPhoto_GetAll]
AS
begin
  select Id,Title,Photo,UploadDate,Detail from dbo.[Photo];
end
	

