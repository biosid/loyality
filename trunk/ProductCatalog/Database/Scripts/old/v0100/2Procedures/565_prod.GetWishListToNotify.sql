create procedure [prod].[GetWishListToNotify] 
	as
BEGIN
	select items.* 
	from [prod].[WishListItems] as items
	left join [prod].[WishListItemNotifications] as itemNotifications on items.UserId = itemNotifications.UserId and items.ProductId = itemNotifications.ProductId
	where itemNotifications.UserId is null
END
