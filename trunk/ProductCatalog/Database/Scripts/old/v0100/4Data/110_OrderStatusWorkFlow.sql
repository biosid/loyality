DECLARE @Draft int = 0
DECLARE @Registration int = 5
DECLARE @Processing int = 10
DECLARE @CancelledByPartner int = 20
DECLARE @DeliveryWaiting int = 30
DECLARE @Delivery int = 40
DECLARE @Delivered int = 50
DECLARE @DeliveredWithDelay int = 51
DECLARE @NotDelivered int = 60

DELETE FROM [prod].[OrderStatusWorkFlow]

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Draft,@CancelledByPartner)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Draft,@Registration)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Registration,@Processing)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Processing,@DeliveryWaiting)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Processing,@CancelledByPartner)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@DeliveryWaiting,@Delivery)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Delivery,@NotDelivered)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Delivery,@Delivered)

INSERT INTO [prod].[OrderStatusWorkFlow] VALUES (@Delivery,@DeliveredWithDelay)

GO