PRINT ' урьер ќзона теперь не имеет особеностей загрузки тарифов доставки'
UPDATE [prod].[Partners]
SET [ImportDeliveryRatesEtlPackageId] = NULL
WHERE [Id] = 6


