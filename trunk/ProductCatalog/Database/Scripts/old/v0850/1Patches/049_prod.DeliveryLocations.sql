IF  EXISTS (	SELECT * FROM sys.check_constraints 
				WHERE object_id = OBJECT_ID(N'[prod].[CK_DeliveryLocations_PartnerId_LocationName]') 
				  AND parent_object_id = OBJECT_ID(N'[prod].[DeliveryLocations]'))
	ALTER TABLE [prod].[DeliveryLocations] DROP CONSTRAINT [CK_DeliveryLocations_PartnerId_LocationName]
GO


IF  EXISTS (	SELECT * FROM sys.objects 
				WHERE object_id = OBJECT_ID(N'[prod].[CheckDeliveryLocationsPartnerIdLocationName]') 
				  AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [prod].[CheckDeliveryLocationsPartnerIdLocationName]
GO


