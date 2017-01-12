IF NOT EXISTS(SELECT * FROM sys.tables where [name] = 'RecommendedPrices')
BEGIN
	CREATE TABLE [prod].[RecommendedPrices](
		[Balance] [money] NOT NULL,
		[MinPrice] [money] NOT NULL,
		[MaxPrice] [money] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE [prod].[RecommendedPrices] ADD  CONSTRAINT [DF_RecommendedPrices_Balance]  DEFAULT ((0)) FOR [Balance]
	ALTER TABLE [prod].[RecommendedPrices] ADD  CONSTRAINT [DF_RecommendedPrices_MinPrice]  DEFAULT ((0)) FOR [MinPrice]
	ALTER TABLE [prod].[RecommendedPrices] ADD  CONSTRAINT [DF_RecommendedPrices_MaxPrice]  DEFAULT ((0)) FOR [MaxPrice]
END


