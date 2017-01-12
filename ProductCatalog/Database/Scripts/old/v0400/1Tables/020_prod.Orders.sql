IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'InsertedUserId' AND Object_ID = Object_ID(N'[prod].[Orders]'))    
BEGIN
	PRINT 'Удаляем столбец InsertedUserId из таблицы [prod].[Orders], так как заказ всегда создается клиентом'
    ALTER TABLE [prod].[Orders]
    DROP COLUMN [InsertedUserId]
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'InsertedUserId' AND Object_ID = Object_ID(N'[prod].[OrdersHistory]'))    
BEGIN
	PRINT 'Удаляем столбец InsertedUserId из таблицы [prod].[OrdersHistory], так как заказ всегда создается клиентом'
    ALTER TABLE [prod].[OrdersHistory]
    DROP COLUMN [InsertedUserId]
END
GO