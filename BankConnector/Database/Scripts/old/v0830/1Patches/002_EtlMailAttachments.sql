CREATE TABLE [etl].[EtlIncomingMailAttachments](
	[SeqId] [int] NOT NULL IDENTITY(1,1),
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_EtlIncomingMailAttachments] PRIMARY KEY CLUSTERED 
(
	[SeqId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO