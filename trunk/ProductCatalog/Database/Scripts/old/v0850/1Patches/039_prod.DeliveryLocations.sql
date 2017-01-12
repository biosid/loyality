IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocations]') AND name = 'UpdateSource')
BEGIN
	ALTER TABLE [prod].[DeliveryLocations]
	ADD [UpdateSource]  [int] NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocationsHistory]') AND name = 'UpdateSource')
BEGIN
	ALTER TABLE [prod].[DeliveryLocationsHistory]
	ADD [UpdateSource]  [int] NOT NULL DEFAULT(0)
END
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[prod].[DeliveryLocationsUpdate]'))
	DROP TRIGGER [prod].[DeliveryLocationsUpdate]
GO

CREATE TRIGGER [prod].[DeliveryLocationsUpdate] 
	ON [prod].[DeliveryLocations] FOR UPDATE
AS
	DECLARE @ArmUpdateSourceId int = 1;
	DECLARE @NewKladr nvarchar(13)
	DECLARE @OldKladr nvarchar(13)
	DECLARE @NewSource int

	DECLARE @Fake nvarchar(64) = NewId()

	SELECT @NewKladr = ISNULL([Kladr],@Fake), @NewSource = UpdateSource FROM INSERTED
	SELECT @OldKladr = ISNULL([Kladr],@Fake) FROM DELETED


	IF (@NewKladr != @OldKladr AND @NewSource = @ArmUpdateSourceId)
	BEGIN
		INSERT INTO [prod].[DeliveryLocationsHistory]         
		SELECT	'U',
				getdate(),
				getutcdate(),
				*
		from INSERTED
	END

GO
