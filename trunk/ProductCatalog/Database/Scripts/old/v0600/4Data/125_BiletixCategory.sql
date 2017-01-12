DECLARE @User nvarchar(50) = 'SCRIPT'

DECLARE @PartnerId [int] = 5

SET IDENTITY_INSERT [prod].[ProductCategories] ON

PRINT 'Делаем категорию "Путешествия"'
INSERT [prod].[ProductCategories] 
	([Id], [Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(99, 0, NULL, N'Путешествия', N'/Путешествия/', 1, 
	 @User, NULL, NULL, 
	 GETDATE(), NULL, 0, 
	 NULL, NULL)
	 
PRINT 'Переделываем категорию для online партнера "Билетикс" (Авиабилеты)'
UPDATE [prod].[ProductCategories] 
   SET 	[Type] = 1,
		[ParentId] = 99,
		[Name] = N'Авиабилеты',
		[NamePath] = N'/Путешествия/Авиабилеты',
		[CatOrder] = 0,
		[OnlineCategoryUrl] = N'http://biletix.tais.ru/vtb24/?token={TOKEN}',
		[NotifyOrderStatusUrl] = N'http://biletix.tais.ru/bitrix/components/travelshop/ibe.frontoffice/collection_redirect.php',
		[OnlineCategoryPartnerId] = 5
WHERE [Id] = 100

PRINT 'Делаем еще одну категорию для online партнера "Билетикс" (Отели)'
INSERT [prod].[ProductCategories] 
	([Id], [Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(101, 1, 99, N'Отели', N'/Путешествия/Отели', 1, 
	 @User, NULL, N'http://vtb24.oktogo.ru/?token={TOKEN}', 
	 GETDATE(), NULL, 1, 
	 N'https://vtb24-dev8.oktogotest.ru/OnlinePayment/RapidSoftComplete', 5)
	 
PRINT 'Делаем another 2 categories для online партнера "Билетикс"'

INSERT [prod].[ProductCategories] 
	([Id],[Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(102, 1, 99, N'Такси', N'/Путешествия/Такси/', 1, 
	 @User, NULL, 'http://collection-vtb24.biletix.ru/transfers/', 
	 GETDATE(), NULL, 0, 
	 'http://collection-vtb24.biletix.ru/transfers/collection_redirect.php', 5)
	 
INSERT [prod].[ProductCategories] 
	([Id], [Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(103, 1, 99, N'Аренда авто', N'/Путешествия/Аренда авто/', 1, 
	 @User, NULL, 'http://collection-vtb24.biletix.ru/rentalcars/', 
	 GETDATE(), NULL, 0, 
	 'http://collection-vtb24.biletix.ru/rentalcars/collection_redirect.php', 5)	 

	 
SET IDENTITY_INSERT [prod].[ProductCategories] OFF