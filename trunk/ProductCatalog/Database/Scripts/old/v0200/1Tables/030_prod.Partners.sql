IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'CheckOrderSupported' AND Object_ID = Object_ID(N'[prod].[Partners]'))    
BEGIN
	PRINT 'Добавляем столбец CheckOrderSupported таблицы [prod].[Partners]'
	ALTER TABLE [prod].[PartnersHistory]
    ADD [CheckOrderSupported] [bit] NULL
    
    ALTER TABLE [prod].[Partners]
    ADD [CheckOrderSupported] [bit] NULL
END
GO
