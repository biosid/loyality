IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[prod].[WishListItemNotifications]') AND name = 'ItemBonusCost')
BEGIN
	ALTER TABLE [prod].[WishListItemNotifications]
	ADD [ItemBonusCost] [money] NULL
END
ELSE
BEGIN
	ALTER TABLE [prod].[WishListItemNotifications]
	ALTER COLUMN [ItemBonusCost] [money] NULL
END
GO
