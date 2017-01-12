if not exists(select * from sys.columns 
            where Name = N'Title' and Object_ID = Object_ID(N'ClientPersonalMessage'))
begin
	ALTER TABLE dbo.ClientPersonalMessage 
	ADD [Title] NVARCHAR(100) NULL
end