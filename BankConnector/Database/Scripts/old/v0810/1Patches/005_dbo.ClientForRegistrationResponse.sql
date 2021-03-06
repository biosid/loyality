IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'RegStatus' AND Object_ID = Object_ID(N'[dbo].[ClientForRegistrationResponse]'))    
BEGIN
	ALTER TABLE [dbo].[ClientForRegistrationResponse]
	ADD [RegStatus] int NULL
END
GO

ALTER TABLE dbo.ClientForRegistrationResponse DROP CONSTRAINT PK_ClientForRegistrationResponse_ClientId_SessionId
GO

ALTER TABLE dbo.ClientForRegistrationResponse ADD
	Id int NOT NULL IDENTITY (1, 1)
GO

ALTER TABLE dbo.ClientForRegistrationResponse ADD CONSTRAINT
	PK_ClientForRegistrationResponse PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

alter table [dbo].[ClientForRegistrationResponse] alter column [Status] int not null
GO