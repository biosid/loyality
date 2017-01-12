IF (EXISTS(SELECT * FROM sys.columns WHERE Name = N'PartnerDeliveryRateId' AND Object_ID = Object_ID(N'[prod].[DeliveryRates]')))
BEGIN
	EXEC sp_rename '[prod].[DeliveryRates].PartnerDeliveryRateId', 'ExternalLocationId', 'COLUMN';
END
GO