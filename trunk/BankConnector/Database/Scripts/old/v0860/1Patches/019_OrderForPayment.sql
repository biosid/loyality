ALTER TABLE [dbo].OrderForPayment DROP COLUMN [ExternalOrderId]
GO

ALTER TABLE [dbo].OrderForPayment ALTER COLUMN [PartnerOrderNum] nvarchar(50) NULL
GO

ALTER TABLE [dbo].OrderForPayment ALTER COLUMN ArticleId nvarchar(50) NULL
GO