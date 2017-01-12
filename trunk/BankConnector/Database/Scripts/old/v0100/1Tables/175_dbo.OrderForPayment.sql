CREATE TABLE [dbo].[OrderForPayment](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[OrderId] [int] NOT NULL,	
	[PartnerId] [int] NOT NULL,
	[PartnerOrderNum] [nvarchar](30)  NOT NULL,
	[ClientId] [int] NOT NULL,
	[ArticleId] [nvarchar](30)  NOT NULL,
	[ArticleName] [nvarchar](500)  NOT NULL,
	[Amount] [int] NOT NULL,
	[OrderBonusCost] [money] NOT NULL,
	[OrderTotalCost] [money] NOT NULL,
	[OrderDateTime] [datetime] NOT NULL,
	[POSId] [int] NOT NULL,
	[DeliveryRegion] [nvarchar](50)  NOT NULL,
	[DeliveryCity] [nvarchar](50) NULL,
	[DeliveryAddress] [nvarchar](500) NOT NULL,
	[ContactName] [nvarchar](255) NOT NULL,
	[ContactPhone] [nvarchar](20) NOT NULL,
	[ContactEmail] [nvarchar](255) NULL,
	[OrderManagementStatus] [int] NULL,
	[OrderItemsCost] [money] NOT NULL,
	[OrderDeliveryCost] [money] NOT NULL,
	[CurrierId] [int] NOT NULL,
 	CONSTRAINT [AK_OrderForPayment_OrderId] UNIQUE NONCLUSTERED ([OrderId] ASC),
 CONSTRAINT [PK_OrderForPayment] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[EtlSessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
GO
ALTER TABLE [dbo].[OrderForPayment]  WITH CHECK ADD  CONSTRAINT [FK_OrderForPayment_EtlSessions] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO