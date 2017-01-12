IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ClientForDeletionResponse_EtlSessionId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ClientForDeletionResponse]'))
ALTER TABLE [dbo].[ClientForDeletionResponse] DROP CONSTRAINT [FK_ClientForDeletionResponse_EtlSessionId]
GO

/****** Object:  Table [dbo].[ClientForDeletionResponse]    Script Date: 07/22/2013 12:14:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClientForDeletionResponse]') AND type in (N'U'))
DROP TABLE [dbo].[ClientForDeletionResponse]
GO

CREATE TABLE [dbo].[ClientForDeletionResponse](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
 CONSTRAINT [PK_ClientForDeletionResponse] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
GO
ALTER TABLE [dbo].[ClientForDeletionResponse]  WITH CHECK ADD  CONSTRAINT [FK_ClientForDeletionResponse_EtlSessionId] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO