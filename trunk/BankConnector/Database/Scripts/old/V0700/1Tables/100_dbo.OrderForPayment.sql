ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [POSId] [nvarchar](50) NOT NULL
GO

ALTER TABLE [dbo].[OrderForPayment]
ADD [UnitellerItemsShopId] [nvarchar](50) NOT NULL
GO

ALTER TABLE [dbo].[OrderForPayment]
ADD [UnitellerDeliveryShopId] [nvarchar](50) NOT NULL
GO
