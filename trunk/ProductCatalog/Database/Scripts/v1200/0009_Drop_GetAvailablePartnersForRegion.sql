/****** Object:  UserDefinedFunction [prod].[GetAvailablePartnersForRegion]    Script Date: 02/25/2014 13:17:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetAvailablePartnersForRegion]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [prod].[GetAvailablePartnersForRegion]
GO
