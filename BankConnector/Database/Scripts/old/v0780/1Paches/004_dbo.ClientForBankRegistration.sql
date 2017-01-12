if (exists(select * from sys.columns where Name = N'IsDelete' and Object_ID = Object_ID(N'[dbo].[ClientForBankRegistration]')))
begin
	EXEC sp_rename '[dbo].[ClientForBankRegistration].IsDelete', 'IsDeleted', 'COLUMN';
end