SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [prod].[GetOrders]
    @clientId nvarchar(255),
    @dateTimeStart [datetime] = null,
    @dateTimeEnd [datetime] = null,
    @countToTake [int],
    @countToSkip [int],
    @calcTotalCount [bit],
    @statuses nvarchar(MAX) = null,
    @skipStatuses nvarchar(MAX) = null,
    @totalCount [int] out
AS
BEGIN

    set nocount on

    select top (@countToTake)
         [Id]
        ,[ClientId]
        ,[ExternalOrderId]
        ,[Status]
        ,[ExternalOrderStatusCode]
        ,[OrderStatusDescription]
        ,[ExternalOrderStatusDateTime]
        ,[StatusChangedDate]
        ,[StatusUtcChangedDate]
        ,[DeliveryInfo]
        ,[InsertedDate]
        ,[InsertedUtcDate]
        ,[UpdatedDate]
        ,[UpdatedUtcDate]
        ,[UpdatedUserId]
        ,[Items]
        ,[TotalWeight]
        ,[ItemsCost]
        ,[BonusItemsCost]
        ,[DeliveryCost]
        ,[BonusDeliveryCost]
        ,[DeliveryAdvance]
        ,[TotalCost]
        ,[BonusTotalCost]
        ,[PartnerId]
        ,[PaymentStatus]
        ,[DeliveryPaymentStatus]
        ,[CarrierId]
        ,[DeliveryInstructions]
    from
    (
        select *, row_number() over (order by [InsertedDate] desc) rownum
        from [prod].[Orders]
        where
            [ClientId] = @clientId
            and
            (@dateTimeStart is null or @dateTimeStart <= [InsertedDate])
            and
            (@dateTimeEnd is null or [InsertedDate] <= @dateTimeEnd)
            and
            (@statuses is null or [Status] in (select value from dbo.ParamParserString(@statuses,',')))
            and
            (@skipStatuses is null or [Status] not in (select value from dbo.ParamParserString(@skipStatuses,',')))
    ) orders
    where rownum > @countToSkip

    if (@calcTotalCount != 0)
    begin
        select @totalCount = count(1)
        from [prod].[Orders]
        where
            [ClientId] = @clientId
            and
            (@dateTimeStart is null or @dateTimeStart <= [InsertedDate])
            and
            (@dateTimeEnd is null or [InsertedDate] <= @dateTimeEnd)
            and
            (@statuses is null or [Status] in (select value from dbo.ParamParserString(@statuses,',')))
            and
            (@skipStatuses is null or [Status] not in (select value from dbo.ParamParserString(@skipStatuses,',')))
    end

END
