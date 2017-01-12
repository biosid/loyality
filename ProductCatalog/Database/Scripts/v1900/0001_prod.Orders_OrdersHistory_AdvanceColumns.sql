
ALTER TABLE [prod].[OrdersHistory] ADD [ItemsAdvance] [money] NOT NULL CONSTRAINT [DF_OrdersHistory_ItemsAdvance]  DEFAULT ((0))
GO
ALTER TABLE [prod].[OrdersHistory] ADD [TotalAdvance] [money] NOT NULL CONSTRAINT [DF_OrdersHistory_TotalAdvance]  DEFAULT ((0))
GO

UPDATE [prod].[OrdersHistory] SET [TotalAdvance] = [DeliveryAdvance]+[ItemsAdvance] WHERE [TotalAdvance]=0;

ALTER TABLE [prod].[Orders] ADD [ItemsAdvance] [money] NOT NULL CONSTRAINT [DF_Orders_ItemsAdvance]  DEFAULT ((0))
GO
ALTER TABLE [prod].[Orders] ADD [TotalAdvance] [money] NOT NULL CONSTRAINT [DF_Orders_TotalAdvance]  DEFAULT ((0))
GO

DISABLE TRIGGER [prod].[LogOrdersUpdate] ON [prod].[Orders]

UPDATE [prod].[Orders] SET [TotalAdvance] = [DeliveryAdvance]+[ItemsAdvance] WHERE [TotalAdvance]=0;

ENABLE TRIGGER [prod].[LogOrdersUpdate] ON [prod].[Orders]

