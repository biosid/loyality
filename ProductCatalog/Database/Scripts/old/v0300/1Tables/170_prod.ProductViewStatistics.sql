IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'ClientId' AND Object_ID = Object_ID(N'[prod].[ProductViewStatistics]'))    
BEGIN
	PRINT 'Добавляем столбец ClientId таблицы [prod].[ProductViewStatistics]'
	ALTER TABLE [prod].[ProductViewStatistics]
	ADD [ClientId] [nvarchar](64) NULL
END
GO

IF EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'UserId' AND Object_ID = Object_ID(N'[prod].[ProductViewStatistics]')) 
BEGIN
	UPDATE [prod].[ProductViewStatistics]
	SET [ClientId] = [UserId]
	WHERE [ClientId] IS NULL
	
	ALTER TABLE [prod].[ProductViewStatistics]
	DROP COLUMN [UserId]
END 
GO

ALTER TABLE [prod].[ProductViewStatistics]
ALTER COLUMN [ClientId] [nvarchar](64) NOT NULL
GO