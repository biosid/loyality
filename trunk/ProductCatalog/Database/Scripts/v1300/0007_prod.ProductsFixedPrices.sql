IF NOT EXISTS(SELECT * FROM sys.tables where [name] = 'ProductsFixedPrices')
BEGIN
	CREATE TABLE [prod].[ProductsFixedPrices](
		[ProductId] [nvarchar](256) NOT NULL,
		[FixedPriceDate] datetime NOT NULL
	) ON [PRIMARY]
END


