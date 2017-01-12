ALTER TABLE [dbo].[RegisterBankOffers] ADD CONSTRAINT
UNIQUE_EtlSessionId_PartnerOrderNum UNIQUE NONCLUSTERED
(
EtlSessionId,
PartnerOrderNum
) ON [PRIMARY]