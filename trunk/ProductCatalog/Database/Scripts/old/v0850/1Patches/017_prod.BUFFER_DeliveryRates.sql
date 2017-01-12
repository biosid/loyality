IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[BUFFER_DeliveryRates]') AND name = 'LocationName')
BEGIN
	DELETE FROM [prod].[BUFFER_DeliveryRates]

	ALTER TABLE [prod].[BUFFER_DeliveryRates]
	ADD [LocationName] [nvarchar](max) NULL
END
GO