SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [prod].[ProductTargetAudiences](
	[ProductId] [nvarchar](256) NOT NULL,
	[TargetAudienceId] [nvarchar](256) NOT NULL,
	[InsertedUserId] [nvarchar](50) NOT NULL,
	[InsertedDate] [datetime2] NOT NULL
) ON [PRIMARY]

GO


