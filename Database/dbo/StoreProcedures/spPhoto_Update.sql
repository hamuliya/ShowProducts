CREATE PROCEDURE [dbo].[spPhoto_Update]
    @Id Int,
	@Title NVARCHAR(100),
	@Photo NVARCHAR(100),
	@UploadDate Date,
	@Detail NVARCHAR(1000)
AS
begin
	update dbo.[Photo] set Title=@Title,  Photo=@Photo,UploadDate=@UploadDate,Detail=@Detail where Id=@Id;
end

