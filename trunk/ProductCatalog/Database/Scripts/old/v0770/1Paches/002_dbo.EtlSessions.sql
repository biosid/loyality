IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'InsertDateTime' AND Object_ID = Object_ID(N'[dbo].[EtlSessions]'))    
BEGIN
	ALTER TABLE [dbo].[EtlSessions]
	ADD [InsertDateTime] [datetime] not null default (GETDATE())
	
	ALTER TABLE [dbo].[EtlSessions]
	ADD [InsertUtcDateTime] [datetime] not null default (GETUTCDATE())
END
GO
