CREATE TABLE [promo].[RuleDomainHistories](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[RuleDomainId] [bigint] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Metadata] [xml] NULL,
	[Event] [int] NOT NULL,
	[LimitType] [int] NOT NULL DEFAULT ((0)),
	[LimitFactor] [decimal](18, 5) NOT NULL DEFAULT ((1)),
	[StopLimitType] [int] NOT NULL DEFAULT ((0)),
	[StopLimitFactor] [decimal](18, 5) NOT NULL DEFAULT ((0)),
	[DefaultBaseAdditionFactor] [decimal](18, 5) NOT NULL DEFAULT ((0)), -- NOTE: 0 - число которое при сложении не изменяет исходное число
	[DefaultBaseMultiplicationFactor] [decimal](18, 5) NOT NULL DEFAULT ((1)), -- NOTE: 1- число которое при умножении не изменяет исходное число
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UserId] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_mech.RuleDomainHistories] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO