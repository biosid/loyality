/****** Object:  StoredProcedure [prod].[GetWishListToNotify]    Script Date: 30.12.2013 17:44:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [prod].[GetWishListToNotify] 
	AS
BEGIN
	SELECT items.* 
	FROM 
	[prod].[WishListItems] as items
	LEFT JOIN 
	[prod].[WishListItemNotifications] AS itemNotifications 
	ON 
	items.ClientId = itemNotifications.ClientId 
	AND 
	items.ProductId = itemNotifications.ProductId
	inner join 
	[prod].[Products] p
	on items.ProductId = p.ProductId
	WHERE itemNotifications.ClientId IS NULL
END
