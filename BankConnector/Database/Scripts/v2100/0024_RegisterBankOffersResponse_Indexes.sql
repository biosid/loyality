DROP INDEX [IX_RegisterBankOffersResponse_EtlSessionId] ON [dbo].[RegisterBankOffersResponse] WITH ( ONLINE = OFF )
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_RegisterBankOffersResponse_EtlSessionId_PartnerOrderId] ON [dbo].[RegisterBankOffersResponse] 
(
	[EtlSessionId] ASC,
	[PartnerOrderNum] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
