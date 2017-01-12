IF EXISTS(	SELECT * FROM sys.columns 
			WHERE Name = N'ProductType' AND Object_ID = Object_ID(N'[prod].[BUFFER_DeliveryRates_Ozon]'))    
BEGIN
	PRINT '������� ������� ProductType ������� [prod].[BUFFER_DeliveryRates_Ozon] '
    ALTER TABLE [prod].[BUFFER_DeliveryRates_Ozon] 
    DROP COLUMN [ProductType]
END
GO