if exists(select * from sys.indexes where name = 'IX_DeliveryLocations_Status')
DROP INDEX [IX_DeliveryLocations_Status] ON [prod].[DeliveryLocations] WITH ( ONLINE = OFF )
GO

if not exists(select * from sys.indexes where name = 'IX_DeliveryLocations_PartnerId_Status')
CREATE NONCLUSTERED INDEX [IX_DeliveryLocations_PartnerId_Status] ON [prod].[DeliveryLocations] 
(
	[PartnerId] ASC,
	[Status] ASC
)
INCLUDE ( [Id]) WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
