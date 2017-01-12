IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PromoActionResponse]') AND type in (N'U'))
	DROP TABLE [dbo].[PromoActionResponse]
GO

CREATE TABLE [dbo].[PromoActionResponse](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EtlSessionId] [nvarchar](64) NOT NULL,
	[PromoId] [bigint] NOT NULL,
	[Status] [int] NOT NULL,
	CONSTRAINT [PK_PromoActionResponse] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO


