if not exists(select * from sys.indexes where name = 'IX_EtlMessages_by_package_session')
CREATE NONCLUSTERED INDEX IX_EtlMessages_by_package_session ON etl.EtlMessages
	(
	EtlPackageId,
	EtlSessionId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
