/****** Object:  UserDefinedFunction [prod].[GetDeliveryRatesForLocation]    Script Date: 02/25/2014 13:16:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetDeliveryRatesForLocation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [prod].[GetDeliveryRatesForLocation]
GO
