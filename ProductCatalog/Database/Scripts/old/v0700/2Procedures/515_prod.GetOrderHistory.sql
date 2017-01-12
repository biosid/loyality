--[prod].[GetOrderHistory] @id = 3, @countToTake = 1000, @countToSkip = 0, @calcTotalCount = 0, @totalCount = 0

ALTER PROCEDURE [prod].[GetOrderHistory]
 @Id int,
 @countToTake [int],
 @countToSkip [int],
 @calcTotalCount [bit],
 @totalCount [int] out as
BEGIN
SET NOCOUNT ON

;with History(Id, StatusChanged, UpdatedUserId, [Status], PaymentStatus, DeliveryPaymentStatus, [OrderStatusDescription], StatusChangedDate, [Rank]) as
(
	select top 1 --inserted
		 [Id]
		,[StatusChanged]
		,[UpdatedUserId]
		,[Status]	
		,[PaymentStatus]
		,[DeliveryPaymentStatus]
		,[OrderStatusDescription]
		,[StatusChangedDate]
		,0
	from 
		[prod].[OrdersHistory]
	where 
		[Id] = @Id and Action = 'I'
		
	union all 
	
	select --updated
		 [Id]	
		,[StatusChanged]
		,[UpdatedUserId]
		,[Status]	
		,[PaymentStatus]
		,[DeliveryPaymentStatus]
		,[OrderStatusDescription]
		,Min([StatusChangedDate]) as StatusChangedDate
		,ROW_NUMBER() OVER (ORDER BY Min(StatusChangedDate))
	from 
		[prod].[OrdersHistory]
	where 
		[Id] = @Id and Action = 'U'
	group by Id, StatusChanged, [UpdatedUserId], [Status], PaymentStatus, DeliveryPaymentStatus, [OrderStatusDescription]
) 
SELECT TOP (@countToTake) *
FROM (
	select 
		H1.Id, 
		H1.UpdatedUserId,
		H1.StatusChanged, --0 is for Status, 1 for Payment, 2 for payment delivery
		H1.Status as NewStatus, 
		H2.Status as OldStatus,
		H1.PaymentStatus as NewPaymentStatus, 
		H2.PaymentStatus as OldPaymentStatus,
		H1.DeliveryPaymentStatus as NewDeliveryStatus, 
		H2.DeliveryPaymentStatus as OldDeliveryStatus,
		H1.OrderStatusDescription as NewOrderStatusDescription,
		H2.OrderStatusDescription as OldOrderStatusDescription,
		H1.StatusChangedDate,		
		ROW_NUMBER() OVER (ORDER BY H1.Id DESC) rownum
	from History H1
		left join History H2
		on H1.Rank = H2.Rank + 1
	where H1.StatusChanged is not null	
	) temp
WHERE rownum > @countToSkip
order by StatusChangedDate desc

IF (@calcTotalCount != 0)
BEGIN
	;with History(Id, StatusChanged, UpdatedUserId, [Status], PaymentStatus, DeliveryPaymentStatus, StatusChangedDate, [Rank]) as
	(
		select top 1 --inserted
			 [Id]
			,[StatusChanged]
			,[UpdatedUserId]
			,[Status]	
			,[PaymentStatus]
			,[DeliveryPaymentStatus]
			,[StatusChangedDate]
			,0
		from 
			[prod].[OrdersHistory]
		where 
			[Id] = @Id and Action = 'I'
			
		union all 
		
		select --updated
			 [Id]	
			,[StatusChanged]
			,[UpdatedUserId]
			,[Status]	
			,[PaymentStatus]
			,[DeliveryPaymentStatus]
			,Min([StatusChangedDate]) as StatusChangedDate
			,ROW_NUMBER() OVER (ORDER BY Min(StatusChangedDate))
		from 
			[prod].[OrdersHistory]
		where 
			[Id] = @Id and Action = 'U'
		group by Id, StatusChanged, [UpdatedUserId], [Status], PaymentStatus, DeliveryPaymentStatus
	) 
	SELECT @totalCount = count(1)
	FROM History
END

END