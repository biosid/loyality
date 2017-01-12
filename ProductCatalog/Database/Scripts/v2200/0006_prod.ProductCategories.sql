/****** Object:  Index [IX_ProductCategories_ParentId_Status]    Script Date: 03/19/2015 20:28:30 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[ProductCategories]') AND name = N'IX_ProductCategories_ParentId_Status')
DROP INDEX [IX_ProductCategories_ParentId_Status] ON [prod].[ProductCategories] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_ProductCategories_ParentId_Status]    Script Date: 03/19/2015 20:28:30 ******/
CREATE NONCLUSTERED INDEX [IX_ProductCategories_ParentId_Status] ON [prod].[ProductCategories] 
(
	[ParentId] ASC,
	[Status] ASC
)
INCLUDE ( [Id],
[Type],
[Name],
[NamePath],
[InsertedUserId],
[UpdatedUserId],
[OnlineCategoryUrl],
[InsertedDate],
[UpdatedDate],
[CatOrder],
[NotifyOrderStatusUrl],
[OnlineCategoryPartnerId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
