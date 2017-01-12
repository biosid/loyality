CREATE TABLE [prod].[ProductPrices](
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

ALTER TABLE [prod].[ProductPrices] ADD  CONSTRAINT [DF_ProductPrices_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]
GO