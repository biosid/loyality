IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'Line' AND Object_ID = Object_ID(N'[prod].[BUFFER_DeliveryRates]'))    
BEGIN
	PRINT 'Удаляем столбец Line из таблицы [prod].[BUFFER_DeliveryRates]'
	ALTER TABLE [prod].[BUFFER_DeliveryRates]
	ADD [Line] [int] NULL DEFAULT(0)
END
GO

IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'AddInfo' AND Object_ID = Object_ID(N'[prod].[BUFFER_DeliveryRates]'))    
BEGIN
	PRINT 'Удаляем столбец AddInfo из таблицы [prod].[BUFFER_DeliveryRates]'
	ALTER TABLE [prod].[BUFFER_DeliveryRates]
	ADD [AddInfo] [nvarchar](255) NULL
END
GO
