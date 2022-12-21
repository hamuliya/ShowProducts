CREATE PROCEDURE [dbo].[spProduct_Update]
    @ProductId Int,
	@Title NVARCHAR(100),
	@UploadDate Date,
	@Detail NVARCHAR(1000)
AS
begin
    set nocount on;
	update dbo.[Product] set Title=@Title, UploadDate=@UploadDate,Detail=@Detail where ProductId=@ProductId;
end

