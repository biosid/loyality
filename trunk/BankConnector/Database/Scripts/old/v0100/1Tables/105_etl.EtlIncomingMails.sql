IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[etl].[EtlIncomingMails]') AND type in (N'U'))
	DROP TABLE [etl].[EtlIncomingMails]
GO

CREATE TABLE [etl].[EtlIncomingMails](
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[MessageUid] [bigint] NOT NULL,
	[MessageRaw] [nvarchar](4000) NOT NULL,
	[IsTrim] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_EtlIncomingMails] PRIMARY KEY CLUSTERED 
	(
		[EtlSessionId] ASC,
		[MessageUid] ASC
	)
)
GO


