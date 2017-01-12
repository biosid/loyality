-- установить параметры онлайн категории билетикса
UPDATE [prod].[ProductCategories] 
SET [NotifyOrderStatusUrl]= N'http://199.199.199.95:8100/bitrix/components/travelshop/ibe.frontoffice/collection_redirect.php'
where [Id]=100