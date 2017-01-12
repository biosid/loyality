IF EXISTS(SELECT * FROM sys.tables where [name] = 'ProductCategoriesBlackList')
BEGIN
	INSERT INTO [prod].[ProductCategoriesBlackList]([CategoryId])
	VALUES
		(1008),
		(1009),
		(1013),
		(575),
		(1017)
END


