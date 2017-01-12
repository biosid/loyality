CREATE TABLE [dbo].[ClientForDeletionResponse](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[ClientCount] [int] NOT NULL,
 CONSTRAINT [PK_ClientForDeletionResponse] PRIMARY KEY CLUSTERED 
(
	[EtlSessionId] ASC,
	[ClientCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
GO
ALTER TABLE [dbo].[ClientForDeletionResponse]  WITH CHECK ADD  CONSTRAINT [FK_ClientForDeletionResponse_EtlSessionId] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO