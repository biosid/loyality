IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocationsUpdate]'))
	DROP TRIGGER [prod].[DeliveryLocationsUpdate]
GO

CREATE TRIGGER [prod].[DeliveryLocationsUpdate] 
	ON [prod].[DeliveryLocations] FOR UPDATE
AS
DECLARE @NewKladr nvarchar(13)
DECLARE @OldKladr nvarchar(13)

DECLARE @Fake nvarchar(64) = NewId()

SELECT @NewKladr = ISNULL([Kladr],@Fake) FROM INSERTED
SELECT @OldKladr = ISNULL([Kladr],@Fake) FROM DELETED
IF (@NewKladr != @OldKladr)
BEGIN
	INSERT INTO [prod].[DeliveryLocationsHistory]         
	SELECT	'U',
			getdate(),
			getutcdate(),
			*
	from INSERTED
END

GO


