MergeUtilsDropSPIfExist '[Geopoints].RecreateBaseKlandTable'
GO

CREATE PROCEDURE [Geopoints].[RecreateBaseKlandTable]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier
AS
BEGIN
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN DROP TABLE [Geopoints].[KLADR]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[KLADR]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[KLADR]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END DROP TABLE [Geopoints].[KLADR]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[KLADR]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[KLADR](
		[NAME] [nvarchar](40) NOT NULL,
		[SOCR] [nvarchar](10) NOT NULL,
		[CODE] [nvarchar](13) NOT NULL,
		[INDEX] [nvarchar](6) NULL,
		[GNINMB] [nvarchar](4) NULL,
		[UNO] [nvarchar](4) NULL,
		[OCATD] [nvarchar](11) NULL,
		[STATUS] [nvarchar](1) NULL
	) ON [PRIMARY]';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[KLADR]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN DROP TABLE [Geopoints].[STREET]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[STREET]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[STREET]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END DROP TABLE [Geopoints].[STREET]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[STREET]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[STREET](
		[NAME] [nvarchar](40) NOT NULL,
		[SOCR] [nvarchar](10) NOT NULL,
		[CODE] [nvarchar](17) NOT NULL,
		[INDEX] [nvarchar](6) NULL,
		[GNINMB] [nvarchar](4) NULL,
		[UNO] [nvarchar](4) NULL,
		[OCATD] [nvarchar](11) NULL
	) ON [PRIMARY]';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[STREET]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN DROP TABLE [Geopoints].[ALTNAMES]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[ALTNAMES]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[ALTNAMES]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END DROP TABLE [Geopoints].[ALTNAMES]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[ALTNAMES]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[ALTNAMES](
		[OLDCODE] [nvarchar](19) NOT NULL,
		[NEWCODE] [nvarchar](19) NOT NULL,
		[LEVEL] [int] NOT NULL
	) ON [PRIMARY]';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[ALTNAMES]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN DROP TABLE [Geopoints].[BufferCityLocation]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BufferCityLocation]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BufferCityLocation]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END DROP TABLE [Geopoints].[BufferCityLocation]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[BufferCityLocation]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BufferCityLocation](
		[Id] [uniqueidentifier] NULL,
		[ParentId] [uniqueidentifier] NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](13) NULL,
		[CountryId] [uniqueidentifier] NULL,
		[RegionName] [nvarchar](255) NULL,
		[RegionCode] [nvarchar](255) NULL,
		[RegionId] [uniqueidentifier] NULL,
		[RegionToponym] [nvarchar](10) NULL,
		[DistrictName] [nvarchar](255) NULL,
		[DistrictCode] [nvarchar](255) NULL,
		[DistrictId] [uniqueidentifier] NULL,
		[DistrictToponym] [nvarchar](10) NULL,
		[CityName] [nvarchar](255) NULL,
		[CityCode] [nvarchar](255) NULL,
		[CityId] [uniqueidentifier] NULL,
		[CityToponym] [nvarchar](10) NULL,
		[TownName] [nvarchar](255) NULL,
		[TownCode] [nvarchar](255) NULL,
		[TownId] [uniqueidentifier] NULL,
		[TownToponym] [nvarchar](10) NULL,
		[CreatedDateTime] [datetime] NULL,
		[CreatedUtcDateTime] [datetime] NULL
	)';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[BufferCityLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN DROP TABLE [Geopoints].[BufferStreetLocation]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BufferStreetLocation]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BufferStreetLocation]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END DROP TABLE [Geopoints].[BufferStreetLocation]');
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[BufferStreetLocation]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BufferStreetLocation](
		[Id] [uniqueidentifier] NULL,
		[ParentId] [uniqueidentifier] NULL,
		[ParentCode] [nvarchar](255) NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](17) NULL,
		[CountryId] [uniqueidentifier] NULL,
		[RegionName] [nvarchar](255) NULL,
		[RegionCode] [nvarchar](255) NULL,
		[RegionId] [uniqueidentifier] NULL,
		[RegionToponym] [nvarchar](10) NULL,
		[DistrictName] [nvarchar](255) NULL,
		[DistrictCode] [nvarchar](255) NULL,
		[DistrictId] [uniqueidentifier] NULL,
		[DistrictToponym] [nvarchar](10) NULL,
		[CityName] [nvarchar](255) NULL,
		[CityCode] [nvarchar](255) NULL,
		[CityId] [uniqueidentifier] NULL,
		[CityToponym] [nvarchar](10) NULL,
		[TownName] [nvarchar](255) NULL,
		[TownCode] [nvarchar](255) NULL,
		[TownId] [uniqueidentifier] NULL,
		[TownToponym] [nvarchar](10) NULL,
		[CreatedDateTime] [datetime] NULL,
		[CreatedUtcDateTime] [datetime] NULL
	)';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[BufferStreetLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN DROP TABLE [Geopoints].[BufferDestinationLocation]');
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[BufferDestinationLocation]') AND type in (N'U'))
	BEGIN
		EXEC sp_executesql N'DROP TABLE [Geopoints].[BufferDestinationLocation]';
	END;
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END DROP TABLE [Geopoints].[BufferDestinationLocation]');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[BufferDestinationLocation]');
	EXEC sp_executesql N'CREATE TABLE [Geopoints].[BufferDestinationLocation](
		[Id] [uniqueidentifier] NULL,
		[ParentId] [uniqueidentifier] NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](20) NULL,
		[CountryId] [uniqueidentifier] NULL,
		[RegionName] [nvarchar](255) NULL,
		[RegionCode] [nvarchar](255) NULL,
		[RegionId] [uniqueidentifier] NULL,
		[RegionToponym] [nvarchar](10) NULL,
		[DistrictName] [nvarchar](255) NULL,
		[DistrictCode] [nvarchar](255) NULL,
		[DistrictId] [uniqueidentifier] NULL,
		[DistrictToponym] [nvarchar](10) NULL,
		[CityName] [nvarchar](255) NULL,
		[CityCode] [nvarchar](255) NULL,
		[CityId] [uniqueidentifier] NULL,
		[CityToponym] [nvarchar](10) NULL,
		[TownName] [nvarchar](255) NULL,
		[TownCode] [nvarchar](255) NULL,
		[TownId] [uniqueidentifier] NULL,
		[TownToponym] [nvarchar](10) NULL,
		[EtlPackageId] [uniqueidentifier] NULL,
		[EtlSessionId] [uniqueidentifier] NULL,
		[CreatedDateTime] [datetime] NOT NULL,
		[CreatedUtcDateTime] [datetime] NOT NULL,
		[ModifiedDateTime] [datetime] NOT NULL,
		[ModifiedUtcDateTime] [datetime] NOT NULL
	)';
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[BufferDestinationLocation]');
END
GO


MergeUtilsDropSPIfExist '[Geopoints].ProcessingKladr'
GO

CREATE PROCEDURE [Geopoints].[ProcessingKladr]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier
AS
BEGIN


	-- Перенос новых данных КЛАДР в таблицу [BufferCityLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[KLADR]->[BufferCityLocation]');

	INSERT INTO [Geopoints].[BufferCityLocation]
			   ([LocationType],  [Name], [KladrCode], 
			   [Toponym], [RegionCode],
			   [DistrictCode], [CityCode], [TownCode],
			   [CreatedDateTime], [CreatedUtcDateTime])
	SELECT 
	[LEVEL] as LocationType,
	[NAME] as Name,
	[CODE] as KladrCode,
	[SOCR] as [Toponym],
	(CASE WHEN [LEVEL] > 1 THEN [Region] ELSE NULL END) as [RegionCode],
	(CASE WHEN [LEVEL] > 2 AND SUBSTRING([CODE], 3, 3) <> '000' THEN [District] ELSE NULL END) as [DistrictCode],
	(CASE WHEN [LEVEL] > 3 AND SUBSTRING([CODE], 6, 3) <> '000' THEN [City] ELSE NULL END) as [CityCode],
	NULL as [TownCode],
	GETDATE(), GETUTCDATE()
	FROM
		(SELECT [NAME], [SOCR], [CODE],
		(case 
			when SUBSTRING([CODE], 3, 9) = '000000000' then 1
			when SUBSTRING([CODE], 6, 6) = '000000' then 2
			when SUBSTRING([CODE], 9, 3) = '000' then 3
			else 4 END
		) as [LEVEL],
		SUBSTRING([CODE], 1, 2) + '00000000000' as Region,
		SUBSTRING([CODE], 1, 5) + '00000000' as District,
		SUBSTRING([CODE], 1, 8) + '00000' as City
		FROM Geopoints.KLADR) as tk
		WHERE SUBSTRING([CODE], 12, 2) = '00'

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[KLADR]->[Geopoints].[BufferCityLocation]');



	-- Перенос новых данных КЛАДР в таблицу [BufferStreetLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[STREET]->[Geopoints].[BufferStreetLocation]');

	INSERT INTO [Geopoints].[BufferStreetLocation]
			   ([LocationType],  [Name], [KladrCode], 
			   [Toponym], [ParentCode],
			   [CreatedDateTime], [CreatedUtcDateTime])
	SELECT
	5 as LocationType,
	[NAME] as Name,
	[CODE] as KladrCode,
	[SOCR] as [Toponym],
	SUBSTRING([CODE], 1, 11) + '00' as ParentCode,
	GETDATE(), GETUTCDATE()
	FROM Geopoints.STREET
	WHERE SUBSTRING([CODE], 16, 2) = '00'

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[STREET]->[Geopoints].[BufferStreetLocation]');



	-- Перенос текущей версии в таблицу Geopoints.BufferDestinationLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[V_Location]->[Geopoints].[BufferDestinationLocation]');

	INSERT INTO [Geopoints].[BufferDestinationLocation](
		[Id], [ParentId], [LocationType],
		[Name], [Toponym], [KladrCode], [CountryId],
		[RegionName], [RegionId], [RegionToponym],
		[DistrictName], [DistrictId], [DistrictToponym],
		[CityName], [CityId], [CityToponym],
		[TownName], [TownId], [TownToponym],
		[EtlPackageId], [EtlSessionId],
		[CreatedDateTime], [CreatedUtcDateTime],
		[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT
		[Id], [ParentId], [LocationType],
		[Name], [Toponym], [KladrCode], [CountryId],
		[RegionName], [RegionId], [RegionToponym],
		[DistrictName], [DistrictId], [DistrictToponym],
		[CityName], [CityId], [CityToponym],
		[TownName], [TownId], [TownToponym],
		[EtlPackageId], [EtlSessionId],
		[CreatedDateTime], [CreatedUtcDateTime], 
		[ModifiedDateTime], [ModifiedUtcDateTime]
	FROM Geopoints.V_Location
	
	IF NOT EXISTS(SELECT TOP(1) Id FROM [Geopoints].[BufferDestinationLocation] WHERE Id = '6f661444-deae-4318-ae35-e149f322fc0b')
		INSERT INTO [Geopoints].[BufferDestinationLocation]
		(
			id,
			locationtype,
			name,
			[EtlPackageId],
			[EtlSessionId],
			createddatetime,
			createdutcdatetime,
			modifieddatetime,
			modifiedutcdatetime
		)
		VALUES
		(
			'6f661444-deae-4318-ae35-e149f322fc0b',
			0,
			'Россия',
			@EtlPackageId,
			@EtlSessionId,
			getdate(),
			getutcdate(),
			getdate(),
			getutcdate()
		)

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[V_Location]->[Geopoints].[BufferDestinationLocation]');


	-- Актуализация кодов в текущей версии КЛАДР Geopoints.BufferDestinationLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[ALTNAMES]->[Geopoints].[BufferDestinationLocation]');
	
	UPDATE bdl
	SET bdl.KladrCode = an.NEWCODE
	FROM [Geopoints].[BufferDestinationLocation] bdl
	JOIN Geopoints.ALTNAMES an ON bdl.KladrCode = an.OLDCODE
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[ALTNAMES]->[Geopoints].[BufferDestinationLocation]');


	-- Актуализация Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BufferCityLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferCityLocation]');
	
	UPDATE sl
	SET
		sl.Id = dl.Id,
		sl.CreatedDateTime = dl.CreatedDateTime,
		sl.CreatedUtcDateTime = dl.CreatedUtcDateTime
	FROM [Geopoints].[BufferCityLocation] sl
	JOIN [Geopoints].[BufferDestinationLocation] dl ON sl.KladrCode = dl.KladrCode
	WHERE dl.EtlPackageId = @EtlPackageId
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferCityLocation]');


	-- Актуализация Id, CreatedDateTime, CreatedUtcDateTime в [Geopoints].[BufferStreetLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferStreetLocation]');
	
	UPDATE sl
	SET
		sl.Id = dl.Id,
		sl.CreatedDateTime = dl.CreatedDateTime,
		sl.CreatedUtcDateTime = dl.CreatedUtcDateTime
	FROM [Geopoints].[BufferStreetLocation] sl
	JOIN [Geopoints].[BufferDestinationLocation] dl ON sl.KladrCode = dl.KladrCode
	WHERE dl.EtlPackageId = @EtlPackageId
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[BufferStreetLocation]');


	-- Актуализация Id в [Geopoints].[BufferCityLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. SET NEW ID');
	
	UPDATE Geopoints.BufferCityLocation
	SET Id = NewId()
	WHERE Id IS NULL
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. SET NEW ID');


	-- Актуализация Id в [Geopoints].[BufferStreetLocation]
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferStreetLocation]. SET NEW ID');

	UPDATE Geopoints.BufferStreetLocation
	SET Id = NewId()
	WHERE Id IS NULL

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferStreetLocation]. SET NEW ID');


	-- UPDATE Country in Geopoints.BufferCityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE REGION');

	UPDATE Geopoints.BufferCityLocation
	SET CountryId = '6f661444-deae-4318-ae35-e149f322fc0b'

	UPDATE Geopoints.BufferCityLocation
	SET ParentId = '6f661444-deae-4318-ae35-e149f322fc0b'
	WHERE LocationType = 1;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE REGION');


	-- UPDATE Region in Geopoints.BufferCityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE REGION');

	UPDATE bl
	SET 
		bl.RegionName = blr.Name,
		bl.RegionId = blr.Id,
		bl.RegionToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BufferCityLocation bl
	JOIN Geopoints.BufferCityLocation blr ON bl.RegionCode = blr.KladrCode;

	UPDATE bl
	SET 
		bl.RegionName = bl.Name,
		bl.RegionToponym = bl.Toponym,
		bl.RegionId = bl.Id,
		bl.RegionCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 1;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE REGION');


	-- UPDATE District in Geopoints.BufferCityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE DISTRICT');

	UPDATE bl
	SET 
		bl.DistrictName = blr.Name,
		bl.DistrictId = blr.Id,
		bl.DistrictToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BufferCityLocation bl
	JOIN Geopoints.BufferCityLocation blr ON bl.DistrictCode = blr.KladrCode;

	UPDATE bl
	SET 
		bl.DistrictName = bl.Name,
		bl.DistrictToponym = bl.Toponym,
		bl.DistrictId = bl.Id,
		bl.DistrictCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 2;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE DISTRICT');


	-- UPDATE City in Geopoints.BufferCityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE CITY');

	UPDATE bl
	SET 
		bl.CityName = blr.Name,
		bl.CityId = blr.Id,
		bl.CityToponym = blr.Toponym,
		bl.ParentId = blr.Id
	FROM Geopoints.BufferCityLocation bl
	JOIN Geopoints.BufferCityLocation blr ON bl.CityCode = blr.KladrCode;

	UPDATE bl
	SET 
		bl.CityName = bl.Name,
		bl.CityToponym = bl.Toponym,
		bl.CityId = bl.Id,
		bl.CityCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 3;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE CITY');


	-- UPDATE Town in Geopoints.BufferCityLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferCityLocation]. UPDATE TOWN');

	UPDATE bl
	SET 
		bl.TownName = bl.Name,
		bl.TownToponym = bl.Toponym,
		bl.TownId = bl.Id,
		bl.TownCode = bl.KladrCode
	FROM Geopoints.BufferCityLocation bl
	WHERE bl.LocationType = 4;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferCityLocation]. UPDATE TOWN');


	-- UPDATE Region, District, City, Town in Geopoints.BufferStreetLocation
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN [Geopoints].[BufferStreetLocation]. UPDATE REGION, DISTRICT, CITY, TOWN');

	UPDATE bl
	SET 
		bl.CountryId = '6f661444-deae-4318-ae35-e149f322fc0b',
		bl.RegionName = (CASE blr.LocationType WHEN 1 THEN blr.Name ELSE blr.RegionName END),
		bl.RegionId = (CASE blr.LocationType WHEN 1 THEN blr.Id ELSE blr.RegionId END),
		bl.RegionToponym = (CASE blr.LocationType WHEN 1 THEN blr.Toponym ELSE blr.RegionToponym END),
		bl.DistrictName = (CASE blr.LocationType WHEN 2 THEN blr.Name ELSE blr.DistrictName END),
		bl.DistrictId = (CASE blr.LocationType WHEN 2 THEN blr.Id ELSE blr.DistrictId END),
		bl.DistrictToponym = (CASE blr.LocationType WHEN 2 THEN blr.Toponym ELSE blr.DistrictToponym END),
		bl.CityName = (CASE blr.LocationType WHEN 3 THEN blr.Name ELSE blr.CityName END),
		bl.CityId = (CASE blr.LocationType WHEN 3 THEN blr.Id ELSE blr.CityId END),
		bl.CityToponym = (CASE blr.LocationType WHEN 3 THEN blr.Toponym ELSE blr.CityToponym END),
		bl.TownName = (CASE blr.LocationType WHEN 4 THEN blr.Name ELSE blr.TownName END),
		bl.TownId = (CASE blr.LocationType WHEN 4 THEN blr.Id ELSE blr.TownId END),
		bl.TownToponym = (CASE blr.LocationType WHEN 4 THEN blr.Toponym ELSE blr.TownToponym END),
		bl.ParentId = blr.Id
	FROM Geopoints.BufferStreetLocation bl
	JOIN Geopoints.BufferCityLocation blr ON blr.KladrCode = bl.ParentCode;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END [Geopoints].[BufferStreetLocation]. UPDATE REGION, DISTRICT, CITY, TOWN');

END;
GO


MergeUtilsDropSPIfExist '[Geopoints].SaveDataToLocality'
GO

CREATE PROCEDURE [Geopoints].[SaveDataToLocality]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier,
@Date nvarchar(20)
AS
BEGIN
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[Location' + @Date + ']');
	
	DECLARE @CREATELOCATION nvarchar(MAX);
	SET @CREATELOCATION = N'CREATE TABLE [Geopoints].[Location' + @Date + '](
		[Id] [uniqueidentifier] NOT NULL,
		[ParentId] [uniqueidentifier] NULL,
		[ExternalId] [nvarchar](50) NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](20) NULL,
		[CountryId] [uniqueidentifier] NULL,
		[RegionName] [nvarchar](255) NULL,
		[RegionId] [uniqueidentifier] NULL,
		[RegionToponym] [nvarchar](10) NULL,
		[DistrictName] [nvarchar](255) NULL,
		[DistrictId] [uniqueidentifier] NULL,
		[DistrictToponym] [nvarchar](10) NULL,
		[CityName] [nvarchar](255) NULL,
		[CityId] [uniqueidentifier] NULL,
		[CityToponym] [nvarchar](10) NULL,
		[TownName] [nvarchar](255) NULL,
		[TownId] [uniqueidentifier] NULL,
		[TownToponym] [nvarchar](10) NULL,
		[EtlPackageId] [uniqueidentifier] NULL,
		[EtlSessionId] [uniqueidentifier] NULL,
		[CreatedDateTime] [datetime] NOT NULL,
		[CreatedUtcDateTime] [datetime] NOT NULL,
		[ModifiedDateTime] [datetime] NOT NULL,
		[ModifiedUtcDateTime] [datetime] NOT NULL,
	 CONSTRAINT [PK_Location' + @Date +'] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]'
	EXEC sp_executesql @CREATELOCATION;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[Location' + @Date + ']');
	
	DECLARE @ModifiedDateTime datetime;
	DECLARE @ModifiedUtcDateTime datetime;

	SET @ModifiedDateTime = GETDATE()
	SET @ModifiedUtcDateTime = GETUTCDATE()

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[Location' + @Date + ']');
	
	DECLARE @BufferDestinationLocation2Location nvarchar(1500);
	DECLARE @BufferDestinationLocation2LocationParm NVARCHAR(500);
	SET @BufferDestinationLocation2Location = N'INSERT INTO [Geopoints].[Location' + @Date + ']
			   ([Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime]
	FROM [Geopoints].[BufferDestinationLocation]
	WHERE EtlPackageId IS NULL OR [EtlPackageId] <> @EtlPackageId';
	SET @BufferDestinationLocation2LocationParm = N'@EtlPackageId uniqueidentifier';
	EXEC sp_executesql @BufferDestinationLocation2Location, @BufferDestinationLocation2LocationParm, @EtlPackageId = @EtlPackageId;
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferDestinationLocation]->[Geopoints].[Location' + @Date + ']');


	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferCityLocation]->[Geopoints].[Location' + @Date + ']');

	DECLARE @BufferCityLocation2Location nvarchar(1500);
	DECLARE @BufferCityLocation2LocationParm NVARCHAR(500);
	SET @BufferCityLocation2Location = N'INSERT INTO [Geopoints].[Location' + @Date + ']
			   ([Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,@EtlPackageId as [EtlPackageId], @EtlSessionId as [EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,@ModifiedDateTime as [ModifiedDateTime], @ModifiedUtcDateTime as [ModifiedUtcDateTime]
	FROM [Geopoints].[BufferCityLocation]';
	SET @BufferCityLocation2LocationParm = N'@EtlPackageId uniqueidentifier, @EtlSessionId uniqueidentifier, @ModifiedDateTime datetime, @ModifiedUtcDateTime datetime';
	EXEC sp_executesql @BufferCityLocation2Location, @BufferCityLocation2LocationParm, @EtlPackageId = @EtlPackageId,
	@EtlSessionId = @EtlSessionId, @ModifiedDateTime = @ModifiedDateTime, @ModifiedUtcDateTime = @ModifiedUtcDateTime;
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferCityLocation]->[Geopoints].[Location' + @Date + ']');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferStreetLocation]->[Geopoints].[Location' + @Date + ']');
	
	DECLARE @BufferStreetLocation2Location nvarchar(1500);
	DECLARE @BufferStreetLocation2LocationParm NVARCHAR(500);
	
	SET @BufferStreetLocation2Location = N'INSERT INTO [Geopoints].[Location' + @Date + ']
			   ([Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode], [CountryId]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType]
	   ,[Name], [Toponym], [KladrCode], [CountryId]
	   ,[RegionName], [RegionId], [RegionToponym]
	   ,[DistrictName], [DistrictId], [DistrictToponym]
	   ,[CityName], [CityId], [CityToponym]
	   ,[TownName], [TownId], [TownToponym]
	   ,@EtlPackageId as [EtlPackageId], @EtlSessionId as [EtlSessionId]
	   ,[CreatedDateTime], [CreatedUtcDateTime]
	   ,@ModifiedDateTime as [ModifiedDateTime], @ModifiedUtcDateTime as [ModifiedUtcDateTime]
	FROM [Geopoints].[BufferStreetLocation]';
	SET @BufferStreetLocation2LocationParm = N'@EtlPackageId uniqueidentifier, @EtlSessionId uniqueidentifier, @ModifiedDateTime datetime, @ModifiedUtcDateTime datetime';
	EXEC sp_executesql @BufferStreetLocation2Location, @BufferStreetLocation2LocationParm, @EtlPackageId = @EtlPackageId,
	@EtlSessionId = @EtlSessionId, @ModifiedDateTime = @ModifiedDateTime, @ModifiedUtcDateTime = @ModifiedUtcDateTime

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferStreetLocation]->[Geopoints].[Location' + @Date + ']');


	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE INDEX. [Geopoints].[Location' + @Date + ']');

	DECLARE @sql NVARCHAR(3000);

	SET @sql = '
	CREATE NONCLUSTERED INDEX [IX_PARENT_LOCATIONTYPE] ON [Geopoints].[Location' + @Date + ']
	(
		[ParentId] ASC,
		[LocationType] ASC
	)
	INCLUDE
	(
		[Id],
		[ExternalId],
		[Name],
		[Toponym],
		[KladrCode],
		[CountryId],
		[RegionName],
		[RegionId],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityId],
		[CityToponym],
		[TownName],
		[TownId],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_REGION_TOPONYM] ON [Geopoints].[Location' + @Date + '] 
	(
		[Name] ASC,
		[RegionName] ASC,
		[Toponym] ASC
	)
	INCLUDE
	(
		[Id],
		[ParentId],
		[ExternalId],
		[LocationType],
		[KladrCode],
		[CountryId],
		[RegionId],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityId],
		[CityToponym],
		[TownName],
		[TownId],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_REGIONID_NAME_LOCATIONTYPE] ON [Geopoints].[Location' + @Date + '] 
	(
		[RegionId] ASC,
		[Name] ASC,
		[LocationType] ASC
	)
	INCLUDE
	(
		[Id],
		[ParentId],
		[ExternalId],
		[Toponym],
		[KladrCode],
		[CountryId],
		[RegionName],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityId],
		[CityToponym],
		[TownName],
		[TownId],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_CITYID_NAME_LOCATIONTYPE] ON [Geopoints].[Location' + @Date + '] 
	(
		[CityId] ASC,
		[Name] ASC,
		[LocationType] ASC
	)
	INCLUDE
	(
		[Id],
		[ParentId],
		[ExternalId],
		[Toponym],
		[KladrCode],
		[CountryId],
		[RegionName],
		[RegionId],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityToponym],
		[TownName],
		[TownId],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_TOWNID_NAME_LOCATIONTYPE] ON [Geopoints].[Location' + @Date + '] 
	(
		[TownId] ASC,
		[Name] ASC,
		[LocationType] ASC
	)
	INCLUDE
	(
		[Id],
		[ParentId],
		[ExternalId],
		[Toponym],
		[KladrCode],
		[CountryId],
		[RegionName],
		[RegionId],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityId],
		[CityToponym],
		[TownName],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_LOCATIONTYPE_NAME] ON [Geopoints].[Location' + @Date + '] 
	(
		 [LocationType] ASC,
		 [Name] ASC
	)
		INCLUDE
	(
		[Id],
		[ParentId],
		[ExternalId],
		[Toponym],
		[KladrCode],
		[CountryId],
		[RegionName],
		[RegionId],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityId],
		[CityToponym],
		[TownName],
		[TownId],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_ID_NAME] ON [Geopoints].[Location' + @Date + '] 
	(
		[Id] ASC,
		[Name] ASC
	)
	INCLUDE
	(
		[ParentId],
		[ExternalId],
		[LocationType],
		[Toponym],
		[KladrCode],
		[CountryId],
		[RegionName],
		[RegionId],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityId],
		[CityToponym],
		[TownName],
		[TownId],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_ID] ON [Geopoints].[Location' + @Date + '] 
	(
		[Name] ASC,
		[Id] ASC
	)
	INCLUDE
	(
		[ParentId],
		[ExternalId],
		[LocationType],
		[Toponym],
		[KladrCode],
		[CountryId],
		[RegionName],
		[RegionId],
		[RegionToponym],
		[DistrictName],
		[DistrictId],
		[DistrictToponym],
		[CityName],
		[CityId],
		[CityToponym],
		[TownName],
		[TownId],
		[TownToponym],
		[EtlPackageId],
		[EtlSessionId],
		[CreatedDateTime],
		[CreatedUtcDateTime],
		[ModifiedDateTime],
		[ModifiedUtcDateTime]
	);';
	EXEC sp_executesql @sql;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE INDEX. [Geopoints].[Location' + @Date + ']');
END;
GO