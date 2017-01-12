
/****** Object:  StoredProcedure [prod].[GetOrders]    Script Date: 03/20/2015 17:29:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetOrders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[GetOrders]
GO

/****** Object:  StoredProcedure [prod].[GetOrders]    Script Date: 03/20/2015 17:29:16 ******/
CREATE PROCEDURE [prod].[GetOrders]
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

	if (@calcTotalCount != 0)
    begin
        select @totalCount = count(Id)
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

    set nocount on

    select
         o.[Id]
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
		,[ItemsAdvance]
        ,[DeliveryCost]
        ,[BonusDeliveryCost]
        ,[DeliveryAdvance]
        ,[TotalCost]
        ,[BonusTotalCost]
		,[TotalAdvance]
        ,[PartnerId]
        ,[PaymentStatus]
        ,[DeliveryPaymentStatus]
        ,[CarrierId]
        ,[DeliveryInstructions]
    from
    (
        select Id, row_number() over (order by [InsertedDate] desc) rownum
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
    JOIN [prod].[Orders] o ON o.Id = orders.Id
    where rownum between (@countToSkip + 1) and (@countToSkip + @countToTake)

END


GO

