/****** Object:  Table [prod].[PartnersHistory]    Script Date: 25.04.2013 9:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [prod].[PartnersHistory](
	[Action] [char](1) NOT NULL,
	[TriggerDate] [datetime] NOT NULL,
	[TriggerUtcDate] [datetime] NOT NULL,
	[Id] [int] NOT NULL,
	[Description] [nvarchar](256) NULL,
	[InsertedUserId] [nvarchar](64) NOT NULL,
	[UpdatedUserId] [nvarchar](64) NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ThrustLevel] [int] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[CarrierId] [int] NULL,
	[IsCarrier] [bit] NOT NULL DEFAULT ((0)),
	[ReportRecipients] [nvarchar](max) NULL,
	[FixPriceSupported] [bit] NULL,
	[ImportDeliveryRatesEtlPackageId] [uniqueidentifier] NULL,
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO