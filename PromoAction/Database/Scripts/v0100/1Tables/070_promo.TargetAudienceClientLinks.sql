CREATE TABLE [promo].[TargetAudienceClientLinks](
	[TargetAudienceId] [nvarchar](255) NOT NULL,
	[ClientId] [nvarchar](255) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[CreateDateTimeUtc] [datetime] NOT NULL,
	[CreateUserId] [nvarchar](50) NULL,
 CONSTRAINT [PK_promo.TargetAudienceClientLinks] PRIMARY KEY CLUSTERED 
(
	[TargetAudienceId] ASC,
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [promo].[TargetAudienceClientLinks]  WITH CHECK ADD  CONSTRAINT [FK_promo.TargetAudienceClientLinks.TargetAudienceId_promo.TargetAudiences] FOREIGN KEY([TargetAudienceId])
REFERENCES [promo].[TargetAudiences] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [promo].[TargetAudienceClientLinks] CHECK CONSTRAINT [FK_promo.TargetAudienceClientLinks.TargetAudienceId_promo.TargetAudiences]
GO