IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryRates]') AND name = 'LocationId')
BEGIN
	ALTER TABLE [prod].[DeliveryRates]
	ADD [LocationId] int NULL
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryRates]') AND name = 'KLADR')
BEGIN
	-- Создаем псевдо привязки
	INSERT INTO [prod].[DeliveryLocations]
	( [PartnerId],[LocationName],[Kladr],[Status]
	 ,[InsertDateTime],[UpdateDateTime],[UpdateUserId],[EtlSessionId])
	(SELECT DISTINCT [PartnerId],[KLADR],[KLADR],1,GETDATE(),NULL,NULL,NULL
	  FROM [prod].[DeliveryRates]
	  WHERE [LocationId] IS NULL)

	UPDATE tab
	SET [LocationId] = loc.Id
	FROM [prod].[DeliveryRates] tab
	JOIN [prod].[DeliveryLocations] loc ON loc.[Kladr] = tab.[KLADR]
	WHERE tab.[LocationId] IS NULL
END
GO

ALTER TABLE [prod].[DeliveryRates]
ALTER COLUMN [LocationId] int NOT NULL
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'FK_DeliveryRates_DeliveryLocations') 
		AND parent_object_id = OBJECT_ID(N'[prod].[DeliveryRates]'))
BEGIN
ALTER TABLE [prod].[DeliveryRates]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryRates_DeliveryLocations] FOREIGN KEY([LocationId])
REFERENCES [prod].[DeliveryLocations] ([Id])

ALTER TABLE [prod].[DeliveryRates] CHECK CONSTRAINT [FK_DeliveryRates_DeliveryLocations]
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryRates]') AND name = 'KLADR')
BEGIN
	ALTER TABLE [prod].[DeliveryRates]
	DROP COLUMN [KLADR]
END
GO