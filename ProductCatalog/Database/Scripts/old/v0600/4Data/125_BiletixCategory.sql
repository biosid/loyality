DECLARE @User nvarchar(50) = 'SCRIPT'

DECLARE @PartnerId [int] = 5

SET IDENTITY_INSERT [prod].[ProductCategories] ON

PRINT '������ ��������� "�����������"'
INSERT [prod].[ProductCategories] 
	([Id], [Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(99, 0, NULL, N'�����������', N'/�����������/', 1, 
	 @User, NULL, NULL, 
	 GETDATE(), NULL, 0, 
	 NULL, NULL)
	 
PRINT '������������ ��������� ��� online �������� "��������" (����������)'
UPDATE [prod].[ProductCategories] 
   SET 	[Type] = 1,
		[ParentId] = 99,
		[Name] = N'����������',
		[NamePath] = N'/�����������/����������',
		[CatOrder] = 0,
		[OnlineCategoryUrl] = N'http://biletix.tais.ru/vtb24/?token={TOKEN}',
		[NotifyOrderStatusUrl] = N'http://biletix.tais.ru/bitrix/components/travelshop/ibe.frontoffice/collection_redirect.php',
		[OnlineCategoryPartnerId] = 5
WHERE [Id] = 100

PRINT '������ ��� ���� ��������� ��� online �������� "��������" (�����)'
INSERT [prod].[ProductCategories] 
	([Id], [Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(101, 1, 99, N'�����', N'/�����������/�����', 1, 
	 @User, NULL, N'http://vtb24.oktogo.ru/?token={TOKEN}', 
	 GETDATE(), NULL, 1, 
	 N'https://vtb24-dev8.oktogotest.ru/OnlinePayment/RapidSoftComplete', 5)
	 
PRINT '������ another 2 categories ��� online �������� "��������"'

INSERT [prod].[ProductCategories] 
	([Id],[Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(102, 1, 99, N'�����', N'/�����������/�����/', 1, 
	 @User, NULL, 'http://collection-vtb24.biletix.ru/transfers/', 
	 GETDATE(), NULL, 0, 
	 'http://collection-vtb24.biletix.ru/transfers/collection_redirect.php', 5)
	 
INSERT [prod].[ProductCategories] 
	([Id], [Type], [ParentId], [Name], [NamePath], [Status], 
	 [InsertedUserId], [UpdatedUserId], [OnlineCategoryUrl],
	 [InsertedDate], [UpdatedDate], [CatOrder], 
	 [NotifyOrderStatusUrl], [OnlineCategoryPartnerId]) 
VALUES 
	(103, 1, 99, N'������ ����', N'/�����������/������ ����/', 1, 
	 @User, NULL, 'http://collection-vtb24.biletix.ru/rentalcars/', 
	 GETDATE(), NULL, 0, 
	 'http://collection-vtb24.biletix.ru/rentalcars/collection_redirect.php', 5)	 

	 
SET IDENTITY_INSERT [prod].[ProductCategories] OFF