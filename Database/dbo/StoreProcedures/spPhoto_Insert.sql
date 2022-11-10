CREATE PROCEDURE [dbo].[spPhoto_Insert]
	@Title NVARCHAR(100),
	@Photo NVARCHAR(100),
	@UploadDate Date,
	@Detail NVARCHAR(1000)
AS
begin
  insert into dbo.[Photo] (Title,Photo,UploadDate,Detail) values(@Title,@Photo,@UploadDate,@Detail);	
end

