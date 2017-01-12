IF EXISTS(SELECT * FROM sys.tables where [name] = 'RecommendedPrices')
BEGIN
	INSERT INTO [prod].[RecommendedPrices]([Balance], [MinPrice], [MaxPrice])
	VALUES
		(2400, 0, 2000),
		(3400, 0, 2400),
		(4400, 0, 3400),
		(5500, 0, 4400),
		(6500, 0, 5400),
		(7500, 0, 6400),
		(9000, 0, 7400),
		(999999999, 3400, 999999999)
END


