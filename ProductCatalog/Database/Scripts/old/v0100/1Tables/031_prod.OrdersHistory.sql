
CREATE TABLE [prod].[OrdersHistory](
	[Action] [char](1) NOT NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	[StatusChanged] [int] NULL,
	[Id] [int] NOT NULL,
	[ExternalOrderId] [nvarchar](36) NULL,
	[PartnerId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ExternalOrderStatusCode] [nvarchar](50) NULL,
	[ExternalOrderStatusDescription] [nvarchar](1000) NULL,
	[ExternalOrderStatusDateTime] [datetime] NULL,
	[StatusChangedDate] [datetime] NOT NULL,
	[StatusUtcChangedDate] [datetime] NOT NULL,
	[DeliveryInfo] [xml] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[InsertedUtcDate] [datetime] NOT NULL,
	[InsertedUserId] [nvarchar](255) NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedUtcDate] [datetime] NOT NULL,
	[UpdatedUserId] [nvarchar](255) NOT NULL,
	[Items] [xml] NOT NULL,
	[TotalWeight] [int] NOT NULL,
	[ItemsCost] [money] NOT NULL,
	[BonusItemsCost] [int] NOT NULL,
	[DeliveryCost] [money] NOT NULL,
	[BonusDeliveryCost] [int] NOT NULL,
	[TotalCost] [money] NOT NULL,
	[BonusTotalCost] [int] NOT NULL,
	[PaymentStatus] [int] NOT NULL,
	[DeliveryPaymentStatus] [int] NOT NULL,
	[CarrierId] int null,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO