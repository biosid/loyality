ALTER TABLE [dbo].OrderForPayment
ALTER COLUMN ArticleId nvarchar(30) NULL
GO

ALTER TABLE [dbo].OrderForPayment
ADD ExternalOrderId [nvarchar](50) NULL
