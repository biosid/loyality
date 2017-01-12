SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RegisterBankOffersResponse](
	[Id] bigint identity(1, 1),
	[PartnerOrderNum] nvarchar(50) NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[OrderActionResult] int NOT NULL,
	[EtlSessionId] [uniqueidentifier] NULL,
	[InsertedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RegisterBankOffersResponse_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RegisterBankOffersResponse] ADD  DEFAULT (getdate()) FOR [InsertedDate]
GO

ALTER TABLE [dbo].[RegisterBankOffersResponse]  WITH CHECK ADD  CONSTRAINT [FK_RegisterBankOffersResponse_EtlSessionsRequest] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO

ALTER TABLE [dbo].[RegisterBankOffersResponse] CHECK CONSTRAINT [FK_RegisterBankOffersResponse_EtlSessionsRequest]
GO

CREATE NONCLUSTERED INDEX [IX_RegisterBankOffersResponse_EtlSessionId] ON [dbo].[RegisterBankOffersResponse]
(
	[EtlSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
