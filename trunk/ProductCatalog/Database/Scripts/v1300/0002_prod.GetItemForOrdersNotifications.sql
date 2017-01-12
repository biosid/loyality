CREATE PROCEDURE [prod].[GetItemsForOrdersNotifications]
    @EtlSessionId uniqueidentifier
AS
BEGIN
    set NOCOUNT ON;

    select
        o.Id as OrderId,
        oi.Item.value('./Product[1]/ProductId[1]', 'nvarchar(256)') as ProductId,
        oi.Item.value('./Product[1]/Name[1]', 'nvarchar(256)') as ProductName,
        oi.Item.value('./Amount[1]', 'int') as ProductQuantity
    from
        prod.OrdersNotifications n
        left join prod.Orders o on n.OrderId = o.Id
        cross apply o.Items.nodes('/ArrayOfOrderItem/OrderItem') as oi(Item)
    where n.EtlSessionId = @EtlSessionId

END
