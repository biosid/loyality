ALTER TABLE dbo.LoyaltyClientUpdates ADD
	ChangedBy nvarchar(255) NULL
GO
ALTER TABLE dbo.LoyaltyClientUpdates
	DROP COLUMN UpdateProperties
GO
