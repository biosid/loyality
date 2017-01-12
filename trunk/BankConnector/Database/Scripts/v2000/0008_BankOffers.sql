
DROP TABLE [dbo].[BankOffers]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BankOffers](
	[Id] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[BonusCost] [money] NOT NULL,
	[ProductId] [nvarchar](50) NOT NULL,
	[OfferId] [nvarchar](50) NOT NULL,
	[ExpirationDate] [date] NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[CardLast4Digits] [nvarchar](4) NULL,
	[Status] [int] NOT NULL,
	[InsertedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_BankOffers_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BankOffers] ADD  DEFAULT (getdate()) FOR [InsertedDate]
GO
