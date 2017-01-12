IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ClientForRegistration]') AND name = N'AK_ClientForRegistration_MobilePhone')
	ALTER TABLE [dbo].[ClientForRegistration] DROP CONSTRAINT [AK_ClientForRegistration_MobilePhone]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ClientForRegistration]') AND name = N'PK_ClientForRegistration_ClientId')
begin
	ALTER TABLE [dbo].[ClientForRegistration] DROP CONSTRAINT [PK_ClientForRegistration_ClientId]
end

if not exists(select * from sys.columns 
            where Name = N'Id' and Object_ID = Object_ID(N'dbo.ClientForRegistration'))    
begin
	ALTER TABLE [dbo].[ClientForRegistration]
	add [Id] int IDENTITY(1,1)
	
	ALTER TABLE [dbo].[ClientForRegistration]
	add constraint [PK_ClientForRegistration_Id] primary key(Id)    
end

go




