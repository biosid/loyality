
/****** Object:  Index [IX_Orders_ClientId_Status]    Script Date: 03/19/2015 17:51:53 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[Orders]') AND name = N'IX_Orders_ClientId_Status')
DROP INDEX [IX_Orders_ClientId_Status] ON [prod].[Orders] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_Orders_ClientId_Status]    Script Date: 03/19/2015 17:51:54 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_ClientId_Status] ON [prod].[Orders] 
(
	[ClientId] ASC,
	[Status] ASC,
	[InsertedDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_Orders_ExternalOrderId]    Script Date: 03/20/2015 12:46:01 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[Orders]') AND name = N'IX_Orders_ExternalOrderId')
DROP INDEX [IX_Orders_ExternalOrderId] ON [prod].[Orders] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_Orders_ExternalOrderId]    Script Date: 03/20/2015 12:46:02 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_ExternalOrderId] ON [prod].[Orders] 
(
	[ExternalOrderId] ASC,
	[PartnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_Orders_InsertedDate]    Script Date: 03/20/2015 17:05:50 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prod].[Orders]') AND name = N'IX_Orders_InsertedDate')
DROP INDEX [IX_Orders_InsertedDate] ON [prod].[Orders] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_Orders_InsertedDate]    Script Date: 03/20/2015 17:05:50 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_InsertedDate] ON [prod].[Orders] 
(
	[InsertedDate] ASC,
	[PartnerId] ASC,
	[Status] ASC
)
INCLUDE ( [PaymentStatus],
[DeliveryPaymentStatus],
[CarrierId]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = N'PXML_Orders_DeliveryInfo')
    DROP INDEX PXML_Orders_DeliveryInfo ON prod.Orders;
GO

CREATE PRIMARY XML INDEX PXML_Orders_DeliveryInfo
    ON prod.Orders (DeliveryInfo);
GO
