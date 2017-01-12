IF NOT EXISTS(SELECT * FROM sys.indexes WHERE NAME = 'IX_ClientForBankRegistration_SessionId_ClientId')    
BEGIN

CREATE NONCLUSTERED INDEX [IX_ClientForBankRegistration_SessionId_ClientId] ON [dbo].[ClientForBankRegistration]
(
	[SessionId] ASC,
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END


IF NOT EXISTS(SELECT * FROM sys.indexes WHERE NAME = 'IX_ClientForBankRegistrationResponse_SessionId_ClientId')    
BEGIN

CREATE NONCLUSTERED INDEX [IX_ClientForBankRegistrationResponse_SessionId_ClientId] ON [dbo].[ClientForBankRegistrationResponse]
(
	[SessionId] ASC,
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


END
