IF NOT EXISTS(SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'ProductsFromAllPartners' AND [COLUMN_NAME] = 'AgrDescription' AND [TABLE_SCHEMA] = 'prod')
BEGIN
 ALTER TABLE  prod.ProductsFromAllPartners ADD AgrDescription AS (Name + ' ' + ProductId + ' ' + ISNULL(Description, ''''));
END
GO

/****** Object:  Index [ProductsFromAllPartners_PartnerId_CategoryId]    Script Date: 03/19/2015 17:25:08 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[ProductsFromAllPartners]') AND name = N'ProductsFromAllPartners_PartnerId_CategoryId')
DROP INDEX [ProductsFromAllPartners_PartnerId_CategoryId] ON [prod].[ProductsFromAllPartners] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [ProductsFromAllPartners_PartnerId_CategoryId]    Script Date: 03/19/2015 17:25:09 ******/
CREATE NONCLUSTERED INDEX [ProductsFromAllPartners_PartnerId_CategoryId] ON [prod].[ProductsFromAllPartners] 
(
	[PartnerId] ASC,
	[CategoryId] ASC
)
INCLUDE ( [ProductId],
[InsertedDate],
[PartnerProductId],
[Name],
[PriceRUR],
[CurrencyId],
[Vendor],
[Delivery],
[Description],
[Status],
[ModerationStatus],
[Weight],
[IsRecommended],
[BasePriceRUR],
[IsDeliveredByEmail]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [ProductsFromAllPartners_CategoryId]    Script Date: 03/19/2015 21:04:25 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[ProductsFromAllPartners]') AND name = N'ProductsFromAllPartners_CategoryId')
DROP INDEX [ProductsFromAllPartners_CategoryId] ON [prod].[ProductsFromAllPartners] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [ProductsFromAllPartners_CategoryId]    Script Date: 03/19/2015 21:04:26 ******/
CREATE NONCLUSTERED INDEX [ProductsFromAllPartners_CategoryId] ON [prod].[ProductsFromAllPartners] 
(
	[CategoryId] ASC
)
INCLUDE ( [ProductId],
[PartnerId],
[InsertedDate],
[PartnerProductId],
[Name],
[PriceRUR],
[CurrencyId],
[Vendor],
[Delivery],
[Description],
[Status],
[ModerationStatus],
[Weight],
[IsRecommended],
[BasePriceRUR],
[IsDeliveredByEmail]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_ProductsFromAllPartners_PartnerProductId]    Script Date: 03/23/2015 14:47:11 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[ProductsFromAllPartners]') AND name = N'IX_ProductsFromAllPartners_PartnerProductId')
DROP INDEX [IX_ProductsFromAllPartners_PartnerProductId] ON [prod].[ProductsFromAllPartners] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_ProductsFromAllPartners_PartnerProductId]    Script Date: 03/23/2015 14:47:11 ******/
CREATE NONCLUSTERED INDEX [IX_ProductsFromAllPartners_PartnerProductId] ON [prod].[ProductsFromAllPartners] 
(
	[PartnerProductId] ASC
)
INCLUDE ( [ProductId],
[PartnerId],
[InsertedDate],
[Name],
[PriceRUR],
[CurrencyId],
[Vendor],
[Delivery],
[Description],
[Status],
[ModerationStatus],
[Weight],
[IsRecommended],
[BasePriceRUR],
[IsDeliveredByEmail]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_ProductsFromAllPartners_InsertedDate_Name]    Script Date: 03/23/2015 14:50:48 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[ProductsFromAllPartners]') AND name = N'IX_ProductsFromAllPartners_InsertedDate_Name')
DROP INDEX [IX_ProductsFromAllPartners_InsertedDate_Name] ON [prod].[ProductsFromAllPartners] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_ProductsFromAllPartners_InsertedDate_Name]    Script Date: 03/23/2015 14:50:49 ******/
CREATE NONCLUSTERED INDEX [IX_ProductsFromAllPartners_InsertedDate_Name] ON [prod].[ProductsFromAllPartners] 
(
	[InsertedDate] DESC,
	[Name] ASC
)
INCLUDE ( [ProductId],
[PartnerId],
[PartnerProductId],
[PriceRUR],
[CurrencyId],
[Vendor],
[Delivery],
[Description],
[Status],
[ModerationStatus],
[Weight],
[IsRecommended],
[BasePriceRUR],
[IsDeliveredByEmail],
[AgrDescription]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_ProductsFromAllPartners_IsRecommended]    Script Date: 03/23/2015 14:54:27 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[ProductsFromAllPartners]') AND name = N'IX_ProductsFromAllPartners_IsRecommended')
DROP INDEX [IX_ProductsFromAllPartners_IsRecommended] ON [prod].[ProductsFromAllPartners] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_ProductsFromAllPartners_IsRecommended]    Script Date: 03/23/2015 14:54:27 ******/
CREATE NONCLUSTERED INDEX [IX_ProductsFromAllPartners_IsRecommended] ON [prod].[ProductsFromAllPartners] 
(
	[IsRecommended] DESC
)
INCLUDE ( [ProductId],
[PartnerId],
[InsertedDate],
[PartnerProductId],
[Name],
[PriceRUR],
[CurrencyId],
[Vendor],
[Delivery],
[Description],
[Status],
[ModerationStatus],
[Weight],
[BasePriceRUR],
[IsDeliveredByEmail]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

