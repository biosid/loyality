IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[etl].[EtlOutcomingMails]') AND type in (N'U'))
	DROP TABLE [etl].[EtlOutcomingMails]
GO

CREATE TABLE [etl].[EtlOutcomingMails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[From] [nvarchar](max) NOT NULL,
	[To] [nvarchar](max) NOT NULL,
	[InsertedDate] [datetime] DEFAULT(GETDATE())
	CONSTRAINT [PK_EtlOutcomingMails] PRIMARY KEY CLUSTERED ( [Id] ASC )
)
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[etl].[EtlOutcomingMailAttachments]') AND type in (N'U'))
	DROP TABLE [etl].[EtlOutcomingMailAttachments]
GO

CREATE TABLE [etl].[EtlOutcomingMailAttachments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EtlOutcomingMailId] [bigint] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_EtlOutcomingMailAttachments] PRIMARY KEY CLUSTERED ( [Id] ASC )
)
GO

ALTER TABLE [etl].[EtlOutcomingMailAttachments]  WITH CHECK ADD  CONSTRAINT [FK_EtlOutcomingMailAttachments_EtlOutcomingMails] FOREIGN KEY([Id])
REFERENCES [etl].[EtlOutcomingMailAttachments] ([Id])
GO

ALTER TABLE [etl].[EtlOutcomingMailAttachments] CHECK CONSTRAINT [FK_EtlOutcomingMailAttachments_EtlOutcomingMails]
GO