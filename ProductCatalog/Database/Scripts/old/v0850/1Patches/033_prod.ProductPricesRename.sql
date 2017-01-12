DROP TABLE [prod].[ProductPrices]
GO

/****** Object:  Table [prod].[Cache]    Script Date: 27.09.2013 15:45:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [prod].[Cache](
	[ContextId] [int] IDENTITY(1,1) NOT NULL,
	[ContextKey] [nvarchar](max) NOT NULL,
	[ContextKeyHash] [varbinary](16) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductPrices] PRIMARY KEY CLUSTERED 
(
	[ContextId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [prod].[Cache] ADD  CONSTRAINT [DF_ProductPrices_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO


