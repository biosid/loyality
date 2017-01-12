SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [prod].[SearchOrders]
    @dateTimeStart [datetime],
    @dateTimeEnd [datetime],
    @countToTake [int],
    @countToSkip [int],
    @calcTotalCount [bit],
    @statuses nvarchar(MAX) = null,
    @skipStatuses nvarchar(MAX) = null,
    @paymentStatus nvarchar(MAX) = null,
    @deliveryPaymentStatus nvarchar(MAX) = null,
    @partnerIds nvarchar(MAX) = null,
    @orderIds nvarchar(MAX) = null,
    @carrierIds nvarchar(max) = null,
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
    from
    (
        select *, row_number() over (order by [InsertedDate] desc) rownum
        from [prod].[Orders]
        where
            [InsertedDate] between @dateTimeStart and @dateTimeEnd
            and
            (@statuses is null or [Status] in (select value from dbo.ParamParserString(@statuses,',')))
            and
            (@skipStatuses is null or [Status] not in (select value from dbo.ParamParserString(@skipStatuses,',')))
            and
            (@paymentStatus is null or [PaymentStatus] in (select value from dbo.ParamParserString(@paymentStatus,',')))
            and
            (@deliveryPaymentStatus is null or [DeliveryPaymentStatus] in (select value from dbo.ParamParserString(@deliveryPaymentStatus,',')))
            and
            (@partnerIds is null or [PartnerId] in (select value from dbo.ParamParserString(@partnerIds,',')))
            and
            (@orderIds is null or [Id] in (select value from dbo.ParamParserString(@orderIds,',')))
            and
            (@carrierIds is null or [CarrierId] in (select value from dbo.ParamParserString(@carrierIds,',')))
    ) orders
    where rownum > @countToSkip
    order by rownum

    if (@calcTotalCount != 0)
    begin
        select @totalCount = count(1)
        from [prod].[Orders]
        where
            [InsertedDate] between @dateTimeStart and @dateTimeEnd
            and
            (@statuses is null or [Status] in (select value from dbo.ParamParserString(@statuses,',')))
            and
            (@skipStatuses is null or [Status] not in (select value from dbo.ParamParserString(@skipStatuses,',')))
            and
            (@paymentStatus is null or [PaymentStatus] in (select value from dbo.ParamParserString(@paymentStatus,',')))
            and
            (@deliveryPaymentStatus is null or [DeliveryPaymentStatus] in (select value from dbo.ParamParserString(@deliveryPaymentStatus,',')))
            and
            (@partnerIds is null or [PartnerId] in (select value from dbo.ParamParserString(@partnerIds,',')))
            and
            (@orderIds is null or [Id] in (select value from dbo.ParamParserString(@orderIds,',')))
            and
            (@carrierIds is null or [CarrierId] in (select value from dbo.ParamParserString(@carrierIds,',')))
    end

END
