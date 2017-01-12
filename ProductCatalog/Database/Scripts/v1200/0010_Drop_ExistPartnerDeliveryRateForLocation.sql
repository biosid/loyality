/****** Object:  UserDefinedFunction [prod].[ExistPartnerDeliveryRateForLocation]    Script Date: 02/25/2014 13:22:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[ExistPartnerDeliveryRateForLocation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [prod].[ExistPartnerDeliveryRateForLocation]
GO
