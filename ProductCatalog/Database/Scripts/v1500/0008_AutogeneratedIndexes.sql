SET ANSI_PADDING ON
go

CREATE NONCLUSTERED INDEX [IX_autogen_prod_DeliveryRates_1] ON [prod].[DeliveryRates]
(
	[LocationId] ASC,
	[PartnerId] ASC,
	[MinWeightGram] ASC,
	[MaxWeightGram] ASC,
	[PriceRur] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE STATISTICS [ST_autogen_prod_DeliveryRates_1] ON [prod].[DeliveryRates]([MinWeightGram], [MaxWeightGram], [PriceRur], [PartnerId])
go

CREATE NONCLUSTERED INDEX [IX_autogen_prod_DeliveryLocations_1] ON [prod].[DeliveryLocations]
(
	[Status] ASC,
	[Kladr] ASC,
	[Id] ASC,
	[ExternalLocationId] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_autogen_prod_DeliveryLocations_2] ON [prod].[DeliveryLocations]
(
	[Kladr] ASC,
	[Status] ASC,
	[Id] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE STATISTICS [ST_autogen_prod_DeliveryLocations_1] ON [prod].[DeliveryLocations]([Id], [Status])
go

CREATE STATISTICS [ST_autogen_prod_DeliveryLocations_2] ON [prod].[DeliveryLocations]([Kladr], [ExternalLocationId], [Status])
go

CREATE STATISTICS [ST_autogen_prod_DeliveryLocations_3] ON [prod].[DeliveryLocations]([Id], [Kladr], [Status], [ExternalLocationId])
go
