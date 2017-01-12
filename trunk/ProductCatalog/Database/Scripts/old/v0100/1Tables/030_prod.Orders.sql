/****** Object:  Table [prod].[Orders]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [prod].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
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
	[BonusItemsCost] [money] NOT NULL,
	[DeliveryCost] [money] NOT NULL,
	[BonusDeliveryCost] [money] NOT NULL,
	[TotalCost] [money] NOT NULL,
	[BonusTotalCost] [money] NOT NULL,
	[PaymentStatus] [int] NOT NULL,
	[DeliveryPaymentStatus] [int] NOT NULL,
	[CarrierId] int null,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Trigger [prod].[LogOrdersInsert]    Script Date: 30.04.2013 18:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [prod].[LogOrdersInsert] on [prod].[Orders]
for insert
as
insert into prod.[OrdersHistory]
select 
'I',
getdate(),
getutcdate(),
case 
	when UPDATE(Status) then 0
	when UPDATE(PaymentStatus) then 1
	when UPDATE(DeliveryPaymentStatus) then 2
end
,
*
from INSERTED


/****** Object:  Trigger [prod].[LogOrdersUpdate]    Script Date: 30.04.2013 18:34:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [prod].[LogOrdersUpdate] on [prod].[Orders]
for update
as

insert into prod.[OrdersHistory]            
select 
'U',
getdate(),
getutcdate(),
case 
	when UPDATE(Status) then 0
	when UPDATE(PaymentStatus) then 1
	when UPDATE(DeliveryPaymentStatus) then 2
end
,
*
from INSERTED

GO