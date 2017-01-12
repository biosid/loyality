ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [ClientId] [nvarchar](255) NOT NULL

ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [DeliveryRegion] [nvarchar](50) NULL
ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [DeliveryCity] [nvarchar](50) NULL
ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [DeliveryAddress] [nvarchar](500) NULL

ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [ContactName] [nvarchar](255) NULL

ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [ContactPhone] [nvarchar](20) NULL

ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [ContactEmail] [nvarchar](255) NULL
