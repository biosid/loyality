if (exists(select * from sys.columns where Name = N'ProductBonusCost' and Object_ID = Object_ID(N'prod.WishListItemNotifications')))
begin
	EXEC sp_rename '[prod].[WishListItemNotifications].ProductBonusCost', 'ItemBonusCost', 'COLUMN';
end

if (exists(select * from sys.columns where Name = N'ProductTotalCost' and Object_ID = Object_ID(N'prod.WishListItemNotifications')))
begin
	EXEC sp_rename '[prod].[WishListItemNotifications].ProductTotalCost', 'TotalBonusCost', 'COLUMN';
end