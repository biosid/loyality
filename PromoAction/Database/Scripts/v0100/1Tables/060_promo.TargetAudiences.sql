CREATE TABLE [promo].[TargetAudiences](
	[Id] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[CreateDateTimeUtc] [datetime] NOT NULL,
	[CreateUserId] [nvarchar](50) NOT NULL,
	[UpdateDateTime] [datetime] NULL,
	[UpdateDateTimeUtc] [datetime] NULL,
	[UpdateUserId] [nvarchar](50) NULL,
	[IsSegment] [bit] not null default 0
 CONSTRAINT [PK_promo.TargetAudiences] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO