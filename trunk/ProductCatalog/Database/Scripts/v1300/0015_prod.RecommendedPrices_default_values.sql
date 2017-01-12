IF EXISTS(SELECT * FROM sys.tables where [name] = 'RecommendedPrices')
BEGIN
	INSERT INTO [prod].[RecommendedPrices]([Balance], [MinPrice], [MaxPrice])
	VALUES
		(2399, 0, 2000),
		(3400, 0, 2400),
		(4399, 0, 3400),
		(5499, 0, 4400),
		(6499, 0, 5400),
		(7499, 0, 6400),
		(9000, 0, 7400),
		(9999999, 0, 3400)
END


