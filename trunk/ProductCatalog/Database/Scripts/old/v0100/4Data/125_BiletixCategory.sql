DECLARE @User nvarchar(50) = 'SCRIPT'

DECLARE @PartnerId [int] = 5

print 'Делаем категорию для online партнера "Билетикс"'
SET IDENTITY_INSERT [prod].[ProductCategories] ON
INSERT [prod].[ProductCategories] 
	([Id], [Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl], 
	 [InsertedDate], [UpdatedDate], [CatOrder], [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(100, 1, NULL, N'Билетикс', N'/Билетикс/', 1, 
	 @User, NULL, 'http://loyalty-win0:3139/catalog/DummyOnlineCategory?token={TOKEN}', 
	 CAST(0x0000A19D00E8D500 AS DateTime), NULL, 0, NULL, 5)
SET IDENTITY_INSERT [prod].[ProductCategories] OFF