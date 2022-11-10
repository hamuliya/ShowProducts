CREATE PROCEDURE [dbo].[spPhoto_Get]
	@Id int
AS
begin
  select Id,Title,Photo,UploadDate,Detail from dbo.[Photo] where Id=@Id;
end
