CREATE TABLE [promo].[TargetAudienceHistories](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Event] [int] NOT NULL,
	[TargetAudienceId] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[CreateDateTimeUtc] [datetime] NOT NULL,
	[CreateUserId] [nvarchar](50) NOT NULL,
	[UpdateDateTime] [datetime] NULL,
	[UpdateDateTimeUtc] [datetime] NULL,
	[UpdateUserId] [nvarchar](50) NULL,
	[IsSegment] bit not null,
	[DeleteDateTime] [datetime] NULL,
	[DeleteDateTimeUtc] [datetime] NULL,
	[DeleteUserId] [nvarchar](50) NULL,
 CONSTRAINT [PK_promo.TargetAudienceHistories] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO