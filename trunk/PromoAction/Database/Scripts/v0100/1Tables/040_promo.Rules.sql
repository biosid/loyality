CREATE TABLE [promo].[Rules](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[RuleDomainId] [bigint] NOT NULL,
	[Type] [int] NOT NULL,
	[DateTimeFrom] [datetime] NULL,
	[DateTimeTo] [datetime] NULL,
	[Status] [int] NOT NULL  DEFAULT ((0)),
	[IsExclusive] [bit] NOT NULL,
	[IsNotExcludedBy] [bit] NOT NULL,
	[Priority] [int] NOT NULL,
	[Predicate] [xml] NULL,
	[Factor] [decimal](18, 5) NOT NULL,
	[ConditionalFactors] [xml] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UserId] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_mech.Rules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [promo].[Rules]  WITH CHECK ADD  CONSTRAINT [FK_mech.Rules.RuleDomainId_mech.RuleDomains] FOREIGN KEY([RuleDomainId])
REFERENCES [promo].[RuleDomains] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [promo].[Rules] CHECK CONSTRAINT [FK_mech.Rules.RuleDomainId_mech.RuleDomains]
GO

ALTER TABLE [promo].[Rules]  WITH CHECK ADD  CONSTRAINT [FK_mech.Rules.TypeCode_mech.RuleTypes] FOREIGN KEY([Type])
REFERENCES [promo].[RuleTypes] ([Code])
GO

ALTER TABLE [promo].[Rules] CHECK CONSTRAINT [FK_mech.Rules.TypeCode_mech.RuleTypes]
GO