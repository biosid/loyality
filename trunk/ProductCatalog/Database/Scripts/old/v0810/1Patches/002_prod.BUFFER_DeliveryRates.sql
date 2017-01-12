IF NOT EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'PartnerDeliveryRateId' AND Object_ID = Object_ID(N'[prod].[BUFFER_DeliveryRates]'))    
BEGIN
	ALTER TABLE [prod].[BUFFER_DeliveryRates]
	ADD [PartnerDeliveryRateId] nvarchar(250) NULL
END