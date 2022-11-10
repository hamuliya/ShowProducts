if not exists (select 1 from dbo.[Photo])
begin
 insert into dbo.[Photo] (Title,Photo,UploadDate,Detail) 
 values (Test,test,GETDATE(),Test),
 (Test1,test1,GETDATE(),Test1);
end