CREATE PROCEDURE [dbo].[spProduct_Delete]
	@ProductId int
AS
begin
    set nocount on;
	delete from dbo.[Product] where ProductId=@ProductId;
end

