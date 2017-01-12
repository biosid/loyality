IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name ='connect')
	EXEC dbo.sp_executesql @statement=N'CREATE SCHEMA connect';
GO
