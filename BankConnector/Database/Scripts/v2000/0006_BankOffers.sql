
ALTER TABLE [dbo].[RegisterBankOffers] DROP CONSTRAINT [FK_RegisterBankOffers_EtlSessionsRequest]
GO

DROP TABLE [dbo].[RegisterBankOffers]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RegisterBankOffers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PartnerOrderNum] [nvarchar](50) NOT NULL,
	[ArticleName] [nvarchar](500) NOT NULL,
	[OrderBonusCost] [money] NOT NULL,
	[ExpirationDate] [date] NOT NULL,
	[ProductId] [nvarchar](50) NOT NULL,
	[OfferId] [nvarchar](50) NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[CardLast4Digits] [nvarchar](4) NULL,
	[OrderAction] [nvarchar](20) NOT NULL,
	[EtlSessionId] [uniqueidentifier] NULL,
	[InsertedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RegisterBankOffers_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_EtlSessionId_PartnerOrderNum] UNIQUE NONCLUSTERED 
(
	[EtlSessionId] ASC,
	[PartnerOrderNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RegisterBankOffers] ADD  CONSTRAINT [DF__RegisterBankOffers__InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO

ALTER TABLE [dbo].[RegisterBankOffers]  WITH CHECK ADD  CONSTRAINT [FK_RegisterBankOffers_EtlSessionsRequest] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO

ALTER TABLE [dbo].[RegisterBankOffers] CHECK CONSTRAINT [FK_RegisterBankOffers_EtlSessionsRequest]
GO


