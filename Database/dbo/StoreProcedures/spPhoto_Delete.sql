CREATE PROCEDURE [dbo].[spPhoto_Delete]
	@Id int
AS
begin
	delete from dbo.[Photo] where Id=@Id;
end

