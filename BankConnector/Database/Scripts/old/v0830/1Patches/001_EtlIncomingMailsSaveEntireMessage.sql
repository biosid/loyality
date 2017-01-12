alter table [etl].[EtlIncomingMails] alter column [MessageRaw] nvarchar(MAX)
GO

alter table [etl].[EtlIncomingMails] drop column [IsTrim]
GO
