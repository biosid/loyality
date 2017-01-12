IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetWishListToNotify]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[GetWishListToNotify]
GO

CREATE PROCEDURE [prod].[GetWishListToNotify] 
	AS
BEGIN
	SELECT items.* 
	FROM [prod].[WishListItems] as items
	LEFT JOIN [prod].[WishListItemNotifications] AS itemNotifications 
		ON items.ClientId = itemNotifications.ClientId AND items.ProductId = itemNotifications.ProductId
	WHERE itemNotifications.ClientId IS NULL
END
