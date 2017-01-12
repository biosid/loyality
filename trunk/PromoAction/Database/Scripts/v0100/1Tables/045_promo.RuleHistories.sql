CREATE TABLE [promo].[RuleHistories](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[RuleId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[RuleDomainId] [bigint] NOT NULL,
	[Type] [int] NOT NULL,
	[DateTimeFrom] [datetime] NULL,
	[DateTimeTo] [datetime] NULL,
	[Status] [int] NOT NULL,
	[IsExclusive] [bit] NOT NULL,
	[IsNotExcludedBy] [bit] NOT NULL,
	[Priority] [int] NOT NULL,
	[Predicate] [xml] NULL,
	[Factor] [decimal](18, 5) NOT NULL,
	[ConditionalFactors] [xml] NULL,
	[Event] [int] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UserId] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_mech.RuleHistories] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO