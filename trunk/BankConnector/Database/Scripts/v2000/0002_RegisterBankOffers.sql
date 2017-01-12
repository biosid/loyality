SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RegisterBankOffers](
	[Id] bigint identity(1, 1),
	[PartnerOrderNum] nvarchar(50) NOT NULL,
	[ArticleName] [nvarchar](500) NOT NULL,
	[OrderBonusCost] [money] NOT NULL,
	[ValidityOfTheOffer] [date] NOT NULL,
	[ProductId] [nvarchar](50) NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[CardLast4Digits] [nvarchar](4) NULL,
	[OrderAction] [nvarchar](20) NOT NULL,
	[EtlSessionId] [uniqueidentifier] NULL,
	[InsertedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RegisterBankOffers_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RegisterBankOffers] ADD  DEFAULT (getdate()) FOR [InsertedDate]
GO

ALTER TABLE [dbo].[RegisterBankOffers]  WITH CHECK ADD  CONSTRAINT [FK_RegisterBankOffers_EtlSessionsRequest] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO

ALTER TABLE [dbo].[RegisterBankOffers] CHECK CONSTRAINT [FK_RegisterBankOffers_EtlSessionsRequest]
GO

CREATE NONCLUSTERED INDEX [IX_RegisterBankOffers_EtlSessionId] ON [dbo].[RegisterBankOffers]
(
	[EtlSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
