IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocations]') AND name = 'ExternalLocationId')
BEGIN
	ALTER TABLE [prod].[DeliveryLocations]
	ADD [ExternalLocationId]  [nvarchar](250) NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocationsHistory]') AND name = 'ExternalLocationId')
BEGIN
	ALTER TABLE [prod].[DeliveryLocationsHistory]
	ADD [ExternalLocationId]  [nvarchar](250) NULL
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryRates]') AND name = 'ExternalLocationId')
BEGIN
	ALTER TABLE [prod].[DeliveryRates]
	DROP COLUMN [ExternalLocationId]
END
GO