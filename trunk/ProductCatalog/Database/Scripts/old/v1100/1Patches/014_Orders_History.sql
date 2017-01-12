BEGIN TRANSACTION
GO
ALTER TABLE prod.OrdersHistory
	DROP CONSTRAINT DF__OrdersHis__Payme__66603565
GO
ALTER TABLE prod.OrdersHistory
	DROP CONSTRAINT DF__OrdersHis__Deliv__6754599E
GO
CREATE TABLE prod.Tmp_OrdersHistory
	(
	Action char(1) NOT NULL,
	TriggerUserId nvarchar(255) NULL,
	TriggerDate datetime NOT NULL,
	TriggerUtcDate datetime NOT NULL,
	StatusChanged int NULL,
	Id int NOT NULL,
	ExternalOrderId nvarchar(36) NULL,
	PartnerId int NOT NULL,
	Status int NOT NULL,
	ExternalOrderStatusCode nvarchar(50) NULL,
	OrderStatusDescription nvarchar(1000) NULL,
	ExternalOrderStatusDateTime datetime NULL,
	StatusChangedDate datetime NOT NULL,
	StatusUtcChangedDate datetime NOT NULL,
	DeliveryInfo xml NULL,
	InsertedDate datetime NOT NULL,
	InsertedUtcDate datetime NOT NULL,
	UpdatedDate datetime NOT NULL,
	UpdatedUtcDate datetime NOT NULL,
	UpdatedUserId nvarchar(255) NOT NULL,
	Items xml NOT NULL,
	TotalWeight int NOT NULL,
	ItemsCost money NOT NULL,
	BonusItemsCost int NOT NULL,
	DeliveryCost money NOT NULL,
	BonusDeliveryCost int NOT NULL,
	TotalCost money NOT NULL,
	BonusTotalCost int NOT NULL,
	PaymentStatus int NOT NULL,
	DeliveryPaymentStatus int NOT NULL,
	CarrierId int NULL,
	ClientId nvarchar(255) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE prod.Tmp_OrdersHistory SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE prod.Tmp_OrdersHistory ADD CONSTRAINT
	DF__OrdersHis__Payme__66603565 DEFAULT ((0)) FOR PaymentStatus
GO
ALTER TABLE prod.Tmp_OrdersHistory ADD CONSTRAINT
	DF__OrdersHis__Deliv__6754599E DEFAULT ((0)) FOR DeliveryPaymentStatus
GO
IF EXISTS(SELECT * FROM prod.OrdersHistory)
	 EXEC('INSERT INTO prod.Tmp_OrdersHistory (Action, TriggerDate, TriggerUtcDate, StatusChanged, Id, ExternalOrderId, PartnerId, Status, ExternalOrderStatusCode, OrderStatusDescription, ExternalOrderStatusDateTime, StatusChangedDate, StatusUtcChangedDate, DeliveryInfo, InsertedDate, InsertedUtcDate, UpdatedDate, UpdatedUtcDate, UpdatedUserId, Items, TotalWeight, ItemsCost, BonusItemsCost, DeliveryCost, BonusDeliveryCost, TotalCost, BonusTotalCost, PaymentStatus, DeliveryPaymentStatus, CarrierId, ClientId)
		SELECT Action, TriggerDate, TriggerUtcDate, StatusChanged, Id, ExternalOrderId, PartnerId, Status, ExternalOrderStatusCode, OrderStatusDescription, ExternalOrderStatusDateTime, StatusChangedDate, StatusUtcChangedDate, DeliveryInfo, InsertedDate, InsertedUtcDate, UpdatedDate, UpdatedUtcDate, UpdatedUserId, Items, TotalWeight, ItemsCost, BonusItemsCost, DeliveryCost, BonusDeliveryCost, TotalCost, BonusTotalCost, PaymentStatus, DeliveryPaymentStatus, CarrierId, ClientId FROM prod.OrdersHistory WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE prod.OrdersHistory
GO
EXECUTE sp_rename N'prod.Tmp_OrdersHistory', N'OrdersHistory', 'OBJECT' 
GO
COMMIT

GO

ALTER trigger [prod].[LogOrdersDelete] on [prod].[Orders] for DELETE 
as
insert into prod.[OrdersHistory]
select 
'D',
dbo.GetCurrentUserId(),
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

GO

ALTER trigger [prod].[LogOrdersInsert] on [prod].[Orders]
for insert
as
insert into prod.[OrdersHistory]
select 
'I',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
case 
	when UPDATE(Status) then 0
	when UPDATE(PaymentStatus) then 1
	when UPDATE(DeliveryPaymentStatus) then 2
end
,
*
from INSERTED

GO

ALTER trigger [prod].[LogOrdersUpdate] on [prod].[Orders]
for update
as

insert into prod.[OrdersHistory]            
select 
'U',
dbo.GetCurrentUserId(),
getdate(),
getutcdate(),
case 
	when UPDATE(Status) then 0
	when UPDATE(PaymentStatus) then 1
	when UPDATE(DeliveryPaymentStatus) then 2
end
,
*
from INSERTED

GO
