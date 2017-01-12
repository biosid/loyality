/****** Object:  Index [IX_BankOffers_ClientId]    Script Date: 03/31/2015 15:29:12 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BankOffers]') AND name = N'IX_BankOffers_ClientId')
DROP INDEX [IX_BankOffers_ClientId] ON [dbo].[BankOffers] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_BankOffers_ClientId]    Script Date: 03/31/2015 15:29:13 ******/
CREATE NONCLUSTERED INDEX [IX_BankOffers_ClientId] ON [dbo].[BankOffers] 
(
	[ClientId] ASC,
	[ExpirationDate] ASC
)
INCLUDE ( [Status]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

