if not exists(select * from sys.columns
              where Name = N'DeliveryInstructions' and Object_ID = Object_ID(N'prod.Orders'))
begin
    alter table prod.Orders
    add DeliveryInstructions nvarchar(1000) null

    alter table prod.OrdersHistory
    add DeliveryInstructions nvarchar(1000) null
end
