ALTER TABLE [prod].[BUFFER_DeliveryRates]
ALTER COLUMN [KLADR] [nvarchar](32) NULL
GO

ALTER TABLE [prod].[BUFFER_DeliveryRates]
ALTER COLUMN [MinWeightGram] [int] NULL
GO

ALTER TABLE [prod].[BUFFER_DeliveryRates]
ALTER COLUMN [MaxWeightGram] [int] NULL
GO

ALTER TABLE [prod].[BUFFER_DeliveryRates]
ALTER COLUMN [PriceRur] [decimal](38, 20) NULL
GO