IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'ClientId' AND Object_ID = Object_ID(N'[prod].[Orders]'))    
BEGIN
	PRINT 'Добавляем столбец ClientId таблицы [prod].[Orders]'
	ALTER TABLE [prod].[OrdersHistory]
    ADD [ClientId] [nvarchar](255) NULL
    
    ALTER TABLE [prod].[Orders]
    ADD [ClientId] [nvarchar](255) NULL
END
GO

UPDATE [prod].[Orders]
SET [ClientId] = [InsertedUserId]
WHERE [ClientId] IS NULL
GO

ALTER TABLE [prod].[Orders]
ALTER COLUMN [ClientId] [nvarchar](255) NOT NULL
GO