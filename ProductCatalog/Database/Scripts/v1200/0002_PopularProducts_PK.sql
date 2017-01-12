if not exists(select * from sys.objects where name = 'PK_PopularProducts')
ALTER TABLE prod.PopularProducts ADD CONSTRAINT
	PK_PopularProducts PRIMARY KEY CLUSTERED 
	(
	PopularType,
	ProductId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]