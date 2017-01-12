IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Name] = N'Rapidsoft.Loaders.KLADR')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'8247c354-ebdd-4886-8e7a-8da38a1a612e', N'Rapidsoft.Loaders.Geocoder', 1
END;