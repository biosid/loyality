IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RegisterBankOffers]') AND name = N'IX_RegisterBankOffers_PartnerOrderNum')
CREATE NONCLUSTERED INDEX [IX_RegisterBankOffers_PartnerOrderNum] ON [dbo].[RegisterBankOffers]
(
	[PartnerOrderNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RegisterBankOffersResponse]') AND name = N'IX_RegisterBankOffersResponse_PartnerOrderNum')
CREATE NONCLUSTERED INDEX [IX_RegisterBankOffersResponse_PartnerOrderNum] ON [dbo].[RegisterBankOffersResponse]
(
	[PartnerOrderNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
