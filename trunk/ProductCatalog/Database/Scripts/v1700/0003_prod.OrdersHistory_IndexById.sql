if exists (select top 1 1 from sys.indexes where name='IX_OrderHistory_Id' and object_id = object_id('prod.OrdersHistory'))
   return

create nonclustered index [IX_OrderHistory_Id] ON [prod].[OrdersHistory]
(
    [Id] ASC
)
with
(
    PAD_INDEX = OFF,
    STATISTICS_NORECOMPUTE = OFF,
    SORT_IN_TEMPDB = OFF,
    IGNORE_DUP_KEY = OFF,
    DROP_EXISTING = OFF,
    ONLINE = OFF,
    ALLOW_ROW_LOCKS = ON,
    ALLOW_PAGE_LOCKS = ON
)
on [PRIMARY]
