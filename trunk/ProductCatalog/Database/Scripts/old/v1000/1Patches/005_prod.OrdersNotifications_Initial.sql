SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [prod].[OrdersNotifications] ******/
CREATE TABLE [prod].[OrdersNotifications] (
    [OrderId] [int] NOT NULL,
    [EtlSessionId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_OrdersNotifications]
        PRIMARY KEY CLUSTERED ([OrderId] ASC)
        WITH (
            PAD_INDEX = OFF,
            STATISTICS_NORECOMPUTE = OFF,
            IGNORE_DUP_KEY = OFF,
            ALLOW_ROW_LOCKS = ON,
            ALLOW_PAGE_LOCKS = ON
        ) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [prod].[OrdersNotificationsEmails] ******/
CREATE TABLE [prod].[OrdersNotificationsEmails] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [EtlSessionId] [nvarchar](64) NOT NULL,
    [Recipients] [nvarchar](1000) NOT NULL,
    [Subject] [nvarchar](1000) NOT NULL,
    [Body] [nvarchar](max) NOT NULL,
    [Status] [int] NOT NULL,
    CONSTRAINT [PK_OrdersNotificationsEmails]
        PRIMARY KEY CLUSTERED ([Id] ASC)
        WITH (
            PAD_INDEX = OFF,
            STATISTICS_NORECOMPUTE = OFF,
            IGNORE_DUP_KEY = OFF,
            ALLOW_ROW_LOCKS = ON,
            ALLOW_PAGE_LOCKS  = ON
        ) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [prod].[FillOrdersNotifications] ******/
CREATE PROCEDURE [prod].[FillOrdersNotifications]
    @EtlSessionId uniqueidentifier,
    @MaxOrdersCount int
AS
BEGIN
    SET NOCOUNT ON;

    insert into prod.OrdersNotifications
        select top (@MaxOrdersCount) Id, @EtlSessionId
        from prod.Orders o
        left join prod.OrdersNotifications n
        on o.Id = n.OrderId
        where n.OrderId is null
        order by o.Id

    select @@ROWCOUNT
END
GO

/****** Object:  StoredProcedure [prod].[GetOrdersNotifications] ******/
CREATE PROCEDURE [prod].[GetOrdersNotifications]
    @EtlSessionId uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;

    select
        o.Id as OrderId,
        o.InsertedDate as CreateDate,
        o.ExternalOrderId as ExternalOrderId,
        o.PartnerId as PartnerId,
        o.Items.value('/ArrayOfOrderItem[1]/OrderItem[1]/Product[1]/ProductId[1]', 'nvarchar(256)') as ProductId,
        o.Items.value('/ArrayOfOrderItem[1]/OrderItem[1]/Product[1]/Name[1]', 'nvarchar(256)') as ProductName,
        o.Items.value('/ArrayOfOrderItem[1]/OrderItem[1]/Amount[1]', 'int') as Quantity,
        o.TotalCost as TotalCost,
        o.DeliveryInfo as DeliveryInfo
    from prod.OrdersNotifications n
    left join prod.Orders o
    on o.Id = n.OrderId
    where n.EtlSessionId = @EtlSessionId
    order by o.Id
END
GO
