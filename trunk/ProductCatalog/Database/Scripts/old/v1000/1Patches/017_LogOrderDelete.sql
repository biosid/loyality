if exists (select * from sys.triggers where name = 'LogOrdersDelete')
begin
	drop trigger [prod].[LogOrdersDelete]
end
GO

CREATE trigger [prod].[LogOrdersDelete] on [prod].[Orders] for DELETE 
as
insert into prod.[OrdersHistory]
select 
'D',
getdate(),
getutcdate(),
case 
	when UPDATE(Status) then 0
	when UPDATE(PaymentStatus) then 1
	when UPDATE(DeliveryPaymentStatus) then 2
end
,
*
from DELETED