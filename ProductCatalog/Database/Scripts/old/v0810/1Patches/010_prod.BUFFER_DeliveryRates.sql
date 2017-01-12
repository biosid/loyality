IF (EXISTS(SELECT * FROM sys.columns WHERE Name = N'PartnerDeliveryRateId' AND Object_ID = Object_ID(N'[prod].[BUFFER_DeliveryRates]')))
BEGIN
	EXEC sp_rename '[prod].[BUFFER_DeliveryRates].PartnerDeliveryRateId', 'ExternalLocationId', 'COLUMN';
END
GO