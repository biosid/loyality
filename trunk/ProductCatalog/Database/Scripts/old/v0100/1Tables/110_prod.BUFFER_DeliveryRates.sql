IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[BUFFER_DeliveryRates]') AND type in (N'U'))
	DROP TABLE [prod].[BUFFER_DeliveryRates]
GO

CREATE TABLE [prod].[BUFFER_DeliveryRates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EtlSessionId] [nvarchar](64) NOT NULL,
	[KLADR] [nvarchar](32) NOT NULL,
	[MinWeightGram] [int] NOT NULL,
	[MaxWeightGram] [int] NOT NULL,
	[PriceRur] [decimal](38, 20) NOT NULL,
	[DeliveryPeriod] [int] NULL,
	[Status] [int] NOT NULL DEFAULT ((0))
)
GO

