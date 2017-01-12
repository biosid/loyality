/****** Object:  StoredProcedure [prod].[GetOrders]    Script Date: 03/19/2015 17:47:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetOrders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[GetOrders]
GO

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

	declare @searchSql nvarchar(MAX);
	
	declare @dateTimeStartCriteria nvarchar(MAX) = '';
	IF (@dateTimeStart is not null)
	SET @dateTimeStartCriteria = 'AND @dateTimeStart <= [InsertedDate] ';
	
	declare @dateTimeEndCriteria nvarchar(MAX) = '';
	IF (@dateTimeEnd is not null)
	SET @dateTimeEndCriteria = 'AND [InsertedDate] <= @dateTimeEnd ';

	declare @statusesCriteria nvarchar(MAX) = '';
	IF (@statuses is not null)
	SET @statusesCriteria = 'AND [Status] in (select value from dbo.ParamParserString(@statuses,'','')) ';
	
	declare @skipStatusesCriteria nvarchar(MAX) = '';
	IF (@skipStatuses is not null)
	SET @skipStatusesCriteria = 'AND [Status] not in (select value from dbo.ParamParserString(@skipStatuses,'','')) ';

set @searchSql = '
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
        select *, row_number() over (order by [InsertedDate] desc) rownum
        from [prod].[Orders]
        where [ClientId] = @clientId 
			' + @dateTimeStartCriteria + '
            ' + @dateTimeEndCriteria + '
            ' + @statusesCriteria + '
            ' + @skipStatusesCriteria+ '
    ) orders
    where rownum > @countToSkip

    if (@calcTotalCount != 0)
    begin
        select @totalCount = count(1)
        from [prod].[Orders]
        where
            [ClientId] = @clientId
            ' + @dateTimeStartCriteria + '
            ' + @dateTimeEndCriteria + '
            ' + @statusesCriteria + '
            ' + @skipStatusesCriteria+ '
    end'

print @searchSql;

EXECUTE sp_executesql 
	@searchSql, 
	N'@clientId nvarchar(255),
    @dateTimeStart [datetime],
    @dateTimeEnd [datetime],
    @countToTake [int],
    @countToSkip [int],
    @calcTotalCount [bit],
    @statuses nvarchar(MAX),
    @skipStatuses nvarchar(MAX),
    @totalCount [int] out', 
	@clientId = @clientId,
    @dateTimeStart = @dateTimeStart,
    @dateTimeEnd = @dateTimeEnd,
    @countToTake = @countToTake,
    @countToSkip = @countToSkip,
    @calcTotalCount = @calcTotalCount,
    @statuses = @statuses,
    @skipStatuses = @skipStatuses,
	@totalCount = @totalCount OUTPUT

END

GO