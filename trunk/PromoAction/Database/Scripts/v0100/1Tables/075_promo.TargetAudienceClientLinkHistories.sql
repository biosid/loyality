CREATE TABLE [promo].[TargetAudienceClientLinkHistories](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Event] [int] NOT NULL,
	[TargetAudienceId] [nvarchar](255) NOT NULL,
	[ClientId] [nvarchar](255) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[CreateDateTimeUtc] [datetime] NOT NULL,
	[CreateUserId] [nvarchar](max) NOT NULL,
	[DeleteDateTime] [datetime] NULL,
	[DeleteDateTimeUtc] [datetime] NULL,
	[DeleteUserId] [nvarchar](max) NULL,
 CONSTRAINT [PK_promo.TargetAudienceClientLinkHistories] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

