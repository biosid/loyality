
/****** Object:  Index [IX_WishListItems_CreatedDate]    Script Date: 03/22/2015 00:23:08 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[WishListItems]') AND name = N'IX_WishListItems_CreatedDate')
DROP INDEX [IX_WishListItems_CreatedDate] ON [prod].[WishListItems] WITH ( ONLINE = OFF )
GO


/****** Object:  Index [IX_WishListItems_CreatedDate]    Script Date: 03/22/2015 00:23:09 ******/
CREATE NONCLUSTERED INDEX [IX_WishListItems_CreatedDate] ON [prod].[WishListItems] 
(
	[CreatedDate] ASC
)
INCLUDE ( [ProductId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

