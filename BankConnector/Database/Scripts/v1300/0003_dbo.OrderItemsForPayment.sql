CREATE TABLE [dbo].[OrderItemsForPayment](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[OrderId] [int] NOT NULL,
	[OrderItemId] NVARCHAR(255) NOT NULL DEFAULT(''),
	[PartnerId] [int] NOT NULL,
	[PartnerOrderNum] [nvarchar](50) NULL,
	[ClientId] [nvarchar](255) NOT NULL,
	[ArticleId] [nvarchar](50) NULL,
	[ArticleName] [nvarchar](500) NOT NULL,
	[Amount] [int] NOT NULL,
	[OrderBonusCost] [money] NOT NULL,
	[OrderTotalCost] [money] NOT NULL,
	[OrderDateTime] [datetime] NOT NULL,
	[POSId] [nvarchar](50) NOT NULL,
	[DeliveryRegion] [nvarchar](50) NULL,
	[DeliveryCity] [nvarchar](50) NULL,
	[DeliveryAddress] [nvarchar](500) NULL,
	[ContactName] [nvarchar](255) NULL,
	[ContactPhone] [nvarchar](20) NULL,
	[ContactEmail] [nvarchar](255) NULL,
	[UnitellerItemsShopId] [nvarchar](50) NOT NULL,
	[UnitellerDeliveryShopId] [nvarchar](50) NOT NULL,
	[InsertedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_OrderItemsForPayment] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[EtlSessionId] ASC,
	[OrderItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrderItemsForPayment]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemsForPayment_EtlSessions] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO

ALTER TABLE [dbo].[OrderItemsForPayment] CHECK CONSTRAINT [FK_OrderItemsForPayment_EtlSessions]
GO

ALTER TABLE [dbo].[OrderItemsForPayment] ADD  DEFAULT (getdate()) FOR [InsertedDate]
GO
