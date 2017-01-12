-- установить параметры онлайн категории билетикса
UPDATE [prod].[ProductCategories] 
SET 
	OnlineCategoryUrl = N'http://biletix.tais.ru/vtb24/?token={TOKEN}'
	,[NotifyOrderStatusUrl] = N'http://biletix.tais.ru/bitrix/components/travelshop/ibe.frontoffice/collection_redirect.php'
where [Id]=100