if not exists(select * from sys.indexes where name = 'IX_DeliveryLocations_Status')
CREATE NONCLUSTERED INDEX IX_DeliveryLocations_Status ON prod.DeliveryLocations
	(
	Status
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO