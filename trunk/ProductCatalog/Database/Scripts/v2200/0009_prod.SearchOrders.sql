
/****** Object:  StoredProcedure [prod].[SearchOrders]    Script Date: 03/20/2015 17:04:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[SearchOrders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[SearchOrders]
GO


/****** Object:  StoredProcedure [prod].[SearchOrders]    Script Date: 03/20/2015 17:04:51 ******/
CREATE PROCEDURE [prod].[SearchOrders]
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

	declare @searchSql nvarchar(max);
	
	declare @statusesCriteria nvarchar(100) = N'';
	if (@statuses is not null)
	SET @statusesCriteria = N'AND [Status] in (select value from dbo.ParamParserString(@statuses,'',''))';
	
	declare @skipStatusesCriteria nvarchar(100) = N'';
	if (@skipStatuses is not null)
	SET @skipStatusesCriteria = N'AND [Status] not in (select value from dbo.ParamParserString(@skipStatuses,'',''))';
	
	declare @paymentStatusCriteria nvarchar(100) = N'';
	if (@paymentStatus is not null)
	SET @paymentStatusCriteria = N'AND [PaymentStatus] in (select value from dbo.ParamParserString(@paymentStatus,'',''))';
	
	declare @deliveryPaymentStatusCriteria nvarchar(100) = N'';
	if (@deliveryPaymentStatus is not null)
	SET @deliveryPaymentStatusCriteria = N'AND [DeliveryPaymentStatus] in (select value from dbo.ParamParserString(@deliveryPaymentStatus,'',''))';
	
	declare @partnerIdsCriteria nvarchar(100) = N'';
	if (@partnerIds is not null)
	SET @partnerIdsCriteria = N'AND [PartnerId] in (select value from dbo.ParamParserString(@partnerIds,'',''))';
	
	declare @orderIdsCriteria nvarchar(100) = N'';
	if (@orderIds is not null)
	SET @orderIdsCriteria = N'AND [Id] in (select value from dbo.ParamParserString(@orderIds,'',''))';
	
	declare @carrierIdsCriteria nvarchar(100) = N'';
	if (@carrierIds is not null)
	SET @carrierIdsCriteria = N'AND [CarrierId] in (select value from dbo.ParamParserString(@carrierIds,'',''))';
	
    set @searchSql = '
    if (@calcTotalCount != 0)
    begin
        select @totalCount = count(Id)
        from [prod].[Orders]
        where
            [InsertedDate] between @dateTimeStart and @dateTimeEnd
            ' + @statusesCriteria + '
            ' + @skipStatusesCriteria + '
            ' + @paymentStatusCriteria + '
            ' + @deliveryPaymentStatusCriteria + '
            ' + @partnerIdsCriteria + '
            ' + @orderIdsCriteria + ' 
            ' + @carrierIdsCriteria + '
    end
    
    select top (@countToTake)
         o1.[Id]
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
        select [Id], row_number() over (order by [InsertedDate] desc) rownum
        from [prod].[Orders]
        where
            [InsertedDate] between @dateTimeStart and @dateTimeEnd
            ' + @statusesCriteria + '
            ' + @skipStatusesCriteria + '
            ' + @paymentStatusCriteria + '
            ' + @deliveryPaymentStatusCriteria + '
            ' + @partnerIdsCriteria + '
            ' + @orderIdsCriteria + ' 
            ' + @carrierIdsCriteria + '
    ) orders
    JOIN [prod].[Orders] o1 ON orders.Id = o1.Id
    where rownum between (@countToSkip + 1) and (@countToSkip + @countToTake)
    order by rownum'

    execute sp_executesql @searchSql
    ,N'@dateTimeStart [datetime],
    @dateTimeEnd [datetime],
    @countToTake [int],
    @countToSkip [int],
    @calcTotalCount [bit],
    @statuses nvarchar(MAX),
    @skipStatuses nvarchar(MAX),
    @paymentStatus nvarchar(MAX),
    @deliveryPaymentStatus nvarchar(MAX),
    @partnerIds nvarchar(MAX),
    @orderIds nvarchar(MAX),
    @carrierIds nvarchar(max),
    @totalCount [int] out',
	@dateTimeStart = @dateTimeStart,
    @dateTimeEnd = @dateTimeEnd,
    @countToTake = @countToTake,
    @countToSkip = @countToSkip,
    @calcTotalCount = @calcTotalCount,
    @statuses = @statuses,
    @skipStatuses = @skipStatuses,
    @paymentStatus = @paymentStatus,
    @deliveryPaymentStatus = @deliveryPaymentStatus,
    @partnerIds = @partnerIds,
    @orderIds = @orderIds,
    @carrierIds = @carrierIds,
    @totalCount = @totalCount out;

END


GO

