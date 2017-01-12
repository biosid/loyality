ALTER TABLE [dbo].[OrderForPayment]
ALTER COLUMN [PartnerOrderNum] [nvarchar](30) NULL
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderForPayment]') AND name = N'AK_OrderForPayment_OrderId')
ALTER TABLE [dbo].[OrderForPayment] DROP CONSTRAINT [AK_OrderForPayment_OrderId]
GO


