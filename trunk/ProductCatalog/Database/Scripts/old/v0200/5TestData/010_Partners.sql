PRINT 'Тестовый партнер с ид 2 НЕ поддерживает поверку заказа'
UPDATE [prod].[Partners]
SET [CheckOrderSupported] = 0
WHERE [Id] = 2


