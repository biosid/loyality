IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetOrderHistory]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [prod].[GetOrderHistory]
GO

CREATE PROCEDURE [prod].[GetOrderHistory]
 @Id int,
 @countToTake [int],
 @countToSkip [int],
 @calcTotalCount [bit],
 @totalCount [int] out as
BEGIN
SET NOCOUNT ON

;with History(Id, UpdatedUserId, [Status], PaymentStatus, DeliveryPaymentStatus, [OrderStatusDescription], [TriggerDate], [Rank]) as
(
	select top 1 --inserted
		 [Id]
		,[UpdatedUserId]
		,[Status]	
		,[PaymentStatus]
		,[DeliveryPaymentStatus]
		,[OrderStatusDescription]
		,[TriggerDate] as [TriggerDate]
		,0 AS [Rank]
	from 
		[prod].[OrdersHistory]
	where 
		[Id] = @Id and Action = 'I'
		
	union all 
	
	select --updated
		 [Id]	
		,[UpdatedUserId]
		,[Status]	
		,[PaymentStatus]
		,[DeliveryPaymentStatus]
		,[OrderStatusDescription]
		,Min([TriggerDate]) as [TriggerDate]
		,ROW_NUMBER() OVER (ORDER BY Min([TriggerDate]))
	from 
		[prod].[OrdersHistory]
	where 
		[Id] = @Id and Action = 'U'
	group by Id, [UpdatedUserId], [Status], PaymentStatus, DeliveryPaymentStatus, [OrderStatusDescription]
) 
SELECT TOP (@countToTake) *
FROM (
	select 
		H1.Id, 
		H1.[RANK],
		H1.UpdatedUserId,
		H1.[Status] as NewStatus, 
		H2.[Status] as OldStatus,
		H1.PaymentStatus as NewPaymentStatus, 
		H2.PaymentStatus as OldPaymentStatus,
		H1.DeliveryPaymentStatus as NewDeliveryStatus, 
		H2.DeliveryPaymentStatus as OldDeliveryStatus,
		H1.OrderStatusDescription as NewOrderStatusDescription,
		H2.OrderStatusDescription as OldOrderStatusDescription,
		H1.[TriggerDate],
		ROW_NUMBER() OVER (ORDER BY H1.[RANK] DESC) rownum
	from History H1
		left join History H2
		on H1.Rank = H2.Rank + 1
	) temp
WHERE rownum > @countToSkip
order by [TriggerDate] DESC, [RANK] desc

IF (@calcTotalCount != 0)
BEGIN

	;with History(Id, StatusChanged, UpdatedUserId, [Status], PaymentStatus, DeliveryPaymentStatus, [TriggerDate], [Rank]) as
	(
		select top 1 --inserted
			 [Id]
			,[UpdatedUserId]
			,[Status]	
			,[PaymentStatus]
			,[DeliveryPaymentStatus]
			,[OrderStatusDescription]
			,[TriggerDate] as [TriggerDate]
			,0 AS [Rank]
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
			,Min([TriggerDate]) as [TriggerDate]
			,ROW_NUMBER() OVER (ORDER BY Min(StatusChangedDate))
		from 
			[prod].[OrdersHistory]
		where 
			[Id] = @Id and Action = 'U'
		group by Id, StatusChanged, [UpdatedUserId], [Status], PaymentStatus, DeliveryPaymentStatus
	) 
	SELECT @totalCount =count(1)
	FROM History

END

END

GO


