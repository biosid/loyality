if not exists(select * from sys.columns
              where Name = N'Advance' and Object_ID = Object_ID(N'OrderItemsForPayment'))
begin
    alter table dbo.OrderItemsForPayment
    add
        [Advance] money null,
        [AdvancePOSId] nvarchar(64) null,
        [AdvanceRRN] nvarchar(12) null
end
