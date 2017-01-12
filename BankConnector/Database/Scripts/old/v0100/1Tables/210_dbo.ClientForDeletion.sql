CREATE TABLE [dbo].[ClientForDeletion](
	[InsertEtlSessionId] [uniqueidentifier] NOT NULL,
	[SendEtlSessionId] [uniqueidentifier] NULL,
	[ExternalClientId] NVARCHAR (36) NOT NULL,	
	[Status] [int] NULL,
 CONSTRAINT [PK_ClientForDeletion] PRIMARY KEY CLUSTERED 
(
	[ExternalClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
GO
ALTER TABLE [dbo].[ClientForDeletion]  WITH CHECK ADD  CONSTRAINT [FK_ClientForDeletion_InsertEtlSessionId] FOREIGN KEY([InsertEtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO
ALTER TABLE [dbo].[ClientForDeletion]  WITH CHECK ADD  CONSTRAINT [FK_ClientForDeletion_SendEtlSessionId] FOREIGN KEY([SendEtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO