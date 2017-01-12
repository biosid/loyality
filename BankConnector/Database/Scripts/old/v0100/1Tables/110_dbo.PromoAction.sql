IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PromoAction]') AND type in (N'U'))
	DROP TABLE [dbo].[PromoAction]
GO

CREATE TABLE [dbo].[PromoAction](
	[EtlSessionId] [nvarchar](64) NOT NULL,
	[PromoId] [bigint] NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[DateSent] [date] NOT NULL DEFAULT(GETDATE()),
	[IndexSent] [int] NOT NULL DEFAULT((1)),
	CONSTRAINT [PK_PromoActionSend] PRIMARY KEY CLUSTERED 
	(
		[EtlSessionId] ASC,
		[PromoId] ASC
	)
)
GO


