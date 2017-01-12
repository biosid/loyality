
IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'SQL_STORED_PROCEDURE' AND name = 'RecreateBaseKlandTable')
BEGIN
	DROP PROCEDURE [Geopoints].[RecreateBaseKlandTable]
END;

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
		[KladrCode] [nvarchar](20) NULL,
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
		[KladrCode] [nvarchar](20) NULL,
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

IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = 'SQL_STORED_PROCEDURE' AND name = 'CreateLocationTable')
BEGIN
	DROP PROCEDURE [Geopoints].[CreateLocationTable]
END;

GO

CREATE PROCEDURE [Geopoints].[CreateLocationTable]
	@Date NVARCHAR(8)
AS
BEGIN
	DECLARE @SQL nvarchar(4000)
	SET @SQL = N'CREATE TABLE [Geopoints].[Location' + @Date + N'](
		[Id] [uniqueidentifier] NOT NULL,
		[ParentId] [uniqueidentifier] NULL,
		[LocationType] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Toponym] [nvarchar](10) NULL,
		[KladrCode] [nvarchar](20) NULL,
		[EtlPackageId] [uniqueidentifier] NULL,
		[EtlSessionId] [uniqueidentifier] NULL,
		[CreatedDateTime] [datetime] NOT NULL,
		[CreatedUtcDateTime] [datetime] NOT NULL,
		[ModifiedDateTime] [datetime] NOT NULL,
		[ModifiedUtcDateTime] [datetime] NOT NULL)';

	EXEC sp_executesql @SQL;
END;