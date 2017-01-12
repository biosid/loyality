if (exists(select * from sys.columns where Name = N'ExternalOrderStatusDescription' and Object_ID = Object_ID(N'prod.Orders')))
begin
	EXEC sp_rename '[prod].[Orders].ExternalOrderStatusDescription', 'OrderStatusDescription', 'COLUMN';
end
go

if (exists(select * from sys.columns where Name = N'ExternalOrderStatusDescription' and Object_ID = Object_ID(N'prod.OrdersHistory')))
begin
	EXEC sp_rename '[prod].[OrdersHistory].ExternalOrderStatusDescription', 'OrderStatusDescription', 'COLUMN';
end
go
