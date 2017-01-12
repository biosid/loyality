ALTER PROCEDURE [prod].[GetOrdersNotifications]
    @EtlSessionId uniqueidentifier
AS
BEGIN
    set NOCOUNT ON;

    select
        o.Id as OrderId,
        o.InsertedDate as CreateDate,
        o.ExternalOrderId as ExternalOrderId,
        o.PartnerId as PartnerId,
        o.TotalCost as TotalCost,
        o.DeliveryInfo as DeliveryInfo
    from prod.OrdersNotifications n
    left join prod.Orders o on o.Id = n.OrderId
    where n.EtlSessionId = @EtlSessionId
    order by o.Id
END
