
IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Name] = N'Rapidsoft.Loaders.KLADR')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'2be81bc3-e917-4712-bb15-cabd56f82961', N'Rapidsoft.Loaders.KLADR', 1
END;