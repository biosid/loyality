if not exists(select * from sys.columns
              where Name = N'DeliveryAdvance' and Object_ID = Object_ID(N'prod.Orders'))
begin
    alter table prod.Orders
    add DeliveryAdvance money not null
    constraint DF_Orders_DeliveryAdvance default 0

    alter table prod.OrdersHistory
    add DeliveryAdvance money not null
    constraint DF_OrdersHistory_DeliveryAdvance default 0
end
