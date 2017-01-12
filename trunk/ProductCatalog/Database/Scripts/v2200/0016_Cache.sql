
/****** Object:  Index [IX_Cache_ContextKeyHash_DisableDate]    Script Date: 03/22/2015 00:15:48 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[Cache]') AND name = N'IX_Cache_ContextKeyHash_DisableDate')
DROP INDEX [IX_Cache_ContextKeyHash_DisableDate] ON [prod].[Cache] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_Cache_ContextKeyHash_DisableDate]    Script Date: 03/22/2015 00:15:49 ******/
CREATE NONCLUSTERED INDEX [IX_Cache_ContextKeyHash_DisableDate] ON [prod].[Cache] 
(
	[ContextKeyHash] ASC,
	[DisableDate] ASC
)
INCLUDE ( [ContextId],
[ContextKey]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

