IF  EXISTS (SELECT * FROM sys.check_constraints 
			WHERE object_id = OBJECT_ID(N'[prod].[CK_DeliveryLocations_PartnerId_LocationName]') 
			  AND parent_object_id = OBJECT_ID(N'[prod].[DeliveryLocations]'))
BEGIN
	ALTER TABLE [prod].[DeliveryLocations] 
	DROP CONSTRAINT [CK_DeliveryLocations_PartnerId_LocationName]
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[CheckDeliveryLocationsPartnerIdLocationName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [prod].[CheckDeliveryLocationsPartnerIdLocationName]
GO

CREATE FUNCTION [prod].[CheckDeliveryLocationsPartnerIdLocationName]
	(@Id int,@PartnerId int,@LocationName nvarchar(max))
	RETURNS BIT
WITH EXECUTE AS CALLER 
AS
BEGIN
	RETURN 
    (
        SELECT CASE WHEN EXISTS 
        (
            SELECT 1 
            FROM [prod].[DeliveryLocations] dl
            WHERE dl.[PartnerId] = @PartnerId
              AND dl.[LocationName] = @LocationName
              AND dl.[Id] ! = @Id
        ) THEN 1 ELSE 0 END
    );
END
GO

ALTER TABLE [prod].[DeliveryLocations]  WITH CHECK 
ADD  CONSTRAINT [CK_DeliveryLocations_PartnerId_LocationName] 
	CHECK  ([prod].[CheckDeliveryLocationsPartnerIdLocationName]([Id],[PartnerId],[LocationName]) != 1)
GO

ALTER TABLE [prod].[DeliveryLocations] CHECK CONSTRAINT [CK_DeliveryLocations_PartnerId_LocationName]
GO

