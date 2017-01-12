if not exists(select * from sys.columns
              where Name = N'ShopId' and Object_ID = Object_ID(N'UnitellerPayments'))
begin
    alter table dbo.UnitellerPayments
    add
        [ShopId] nvarchar(64) not null constraint DF_ShopId default '',
        [FormDate] datetime2(7) null,
        [ConfirmDate] datetime2(7) null,
        [CancelDate] datetime2(7) null

    alter table dbo.UnitellerPayments
    alter column [BillNumber] nvarchar(12) null
end
