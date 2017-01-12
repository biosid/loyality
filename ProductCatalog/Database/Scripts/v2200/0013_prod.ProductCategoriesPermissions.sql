
/****** Object:  Index [IX_ProductCategoriesPermissions_Revert]    Script Date: 03/21/2015 23:55:58 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[ProductCategoriesPermissions]') AND name = N'IX_ProductCategoriesPermissions_Revert')
DROP INDEX [IX_ProductCategoriesPermissions_Revert] ON [prod].[ProductCategoriesPermissions] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_ProductCategoriesPermissions_Revert]    Script Date: 03/21/2015 23:55:59 ******/
CREATE NONCLUSTERED INDEX [IX_ProductCategoriesPermissions_Revert] ON [prod].[ProductCategoriesPermissions] 
(
	[CategoryId] ASC,
	[PartnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

