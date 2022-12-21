if not exists (select 1 from dbo.[Product])
begin
 insert into dbo.[Product] (Title,UploadDate,Detail) 
 values ('Test',GETDATE(),'Test'),
 ('Test1',GETDATE(),'Test1');
end

if not exists (select 1 from dbo.[Role])
begin 
insert into dbo.[Role] (Role)
values ('Visitor');
insert into dbo.[Role] (Role)
values ('Admin');
insert into dbo.[Role] (Role)
values ('Member');
end