CREATE PROCEDURE [dbo].[spProduct_Insert]
    @ProductId int output,
	@Title NVARCHAR(100),
	@UploadDate Date,
	@Detail NVARCHAR(1000)
AS
begin
  set nocount on;
  insert into dbo.[Product] (Title,UploadDate,Detail) 
  values(@Title,@UploadDate,@Detail);
  select @@IDENTITY;
end;



