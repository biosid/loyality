/****** Object:  Index [IX_ClientForBankRegistration_MobilePhone_IsDeleted]    Script Date: 03/31/2015 15:32:42 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ClientForBankRegistration]') AND name = N'IX_ClientForBankRegistration_MobilePhone_IsDeleted')
DROP INDEX [IX_ClientForBankRegistration_MobilePhone_IsDeleted] ON [dbo].[ClientForBankRegistration] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [IX_ClientForBankRegistration_MobilePhone_IsDeleted]    Script Date: 03/31/2015 15:32:43 ******/
CREATE NONCLUSTERED INDEX [IX_ClientForBankRegistration_MobilePhone_IsDeleted] ON [dbo].[ClientForBankRegistration] 
(
	[MobilePhone] ASC,
	[IsDeleted] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

