IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'ClientId' AND Object_ID = Object_ID(N'[prod].[BasketItems]'))    
BEGIN
	PRINT 'Добавляем столбец ClientId таблицы [prod].[BasketItems]'
	ALTER TABLE [prod].[BasketItems]
	ADD [ClientId] [nvarchar](64) NULL
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'UserId' AND Object_ID = Object_ID(N'[prod].[BasketItems]')) 
BEGIN
	UPDATE [prod].[BasketItems]
	SET [ClientId] = [UserId]
	WHERE [ClientId] IS NULL
	
	ALTER TABLE [prod].[BasketItems]
	DROP COLUMN [UserId]
END 
GO

ALTER TABLE [prod].[BasketItems]
ALTER COLUMN [ClientId] [nvarchar](64) NOT NULL
GO