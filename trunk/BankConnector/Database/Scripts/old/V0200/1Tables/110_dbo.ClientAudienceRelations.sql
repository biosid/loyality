CREATE TABLE [dbo].[ClientAudienceRelations](
	[SessionId] [uniqueidentifier] NOT NULL,
	[ClientId] [nvarchar](256) NOT NULL,
	[PromoId] [nvarchar](256) NOT NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ClientAudienceRelations_SessionId_ClientId_PromoId] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[ClientId] ASC,
	[PromoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[StepsBuffer]  WITH CHECK ADD  CONSTRAINT [FK_ClientAudienceRelations_EtlSessions] FOREIGN KEY([SessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO

ALTER TABLE [dbo].[StepsBuffer] CHECK CONSTRAINT [FK_ClientAudienceRelations_EtlSessions]
GO