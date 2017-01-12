ALTER TABLE [etl].[EtlSessions]
ALTER COLUMN [StartDateTime] [datetime] NULL
GO

ALTER TABLE [etl].[EtlSessions]
ALTER COLUMN [StartUtcDateTime] [datetime] NULL
GO

IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'InsertDateTime' AND Object_ID = Object_ID(N'[etl].[EtlSessions]'))    
BEGIN
	ALTER TABLE [etl].[EtlSessions]
	ADD [InsertDateTime] [datetime] not null default (GETDATE())
	
	ALTER TABLE [etl].[EtlSessions]
	ADD [InsertUtcDateTime] [datetime] not null default (GETUTCDATE())
END
GO
