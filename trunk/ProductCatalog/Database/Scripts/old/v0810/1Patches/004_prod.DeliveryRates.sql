IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'PartnerDeliveryRateId' AND Object_ID = Object_ID(N'[prod].[DeliveryRates]'))    
BEGIN
	ALTER TABLE [prod].[DeliveryRates]
	ADD [PartnerDeliveryRateId] nvarchar(250) NULL
END