IF NOT EXISTS(SELECT * FROM sys.tables where [name] = 'ProductCategoriesBlackList')
BEGIN
	CREATE TABLE [prod].[ProductCategoriesBlackList](
		[CategoryId] [int] NOT NULL
	 CONSTRAINT [PK_ProductCategoriesBlackList_1] PRIMARY KEY CLUSTERED 
	(
		[CategoryId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


