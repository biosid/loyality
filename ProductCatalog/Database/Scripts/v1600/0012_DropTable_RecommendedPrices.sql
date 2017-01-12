IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RecommendedPrices_Balance]') AND type = 'D')
BEGIN
ALTER TABLE [prod].[RecommendedPrices] DROP CONSTRAINT [DF_RecommendedPrices_Balance]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RecommendedPrices_MinPrice]') AND type = 'D')
BEGIN
ALTER TABLE [prod].[RecommendedPrices] DROP CONSTRAINT [DF_RecommendedPrices_MinPrice]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RecommendedPrices_MaxPrice]') AND type = 'D')
BEGIN
ALTER TABLE [prod].[RecommendedPrices] DROP CONSTRAINT [DF_RecommendedPrices_MaxPrice]
END

GO

/****** Object:  Table [prod].[RecommendedPrices]    Script Date: 10/22/2014 20:35:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[RecommendedPrices]') AND type in (N'U'))
DROP TABLE [prod].[RecommendedPrices]
GO
