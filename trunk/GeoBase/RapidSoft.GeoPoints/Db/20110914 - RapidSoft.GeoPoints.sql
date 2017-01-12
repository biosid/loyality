IF schema_id('Geopoints') is null
BEGIN
	EXECUTE('CREATE SCHEMA Geopoints');
END;

GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='LocationType' AND TABLE_SCHEMA='Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[LocationType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
	[ModifiedUtcDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_LocationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;
GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='Location' AND TABLE_SCHEMA='Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[Location](
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[LocationType] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[KladrCode] [nvarchar](20) NULL,
	[Address] [nvarchar](500) NOT NULL,
	[RegionName] [nvarchar](255) NULL,
	[RegionId] [uniqueidentifier] NULL,
	[RegionToponym] [nvarchar](10) NULL,
	[DistrictName] [nvarchar](255) NULL,
	[DistrictId] [uniqueidentifier] NULL,
	[DistrictToponym] [nvarchar](10) NULL,
	[CityName] [nvarchar](255) NULL,
	[CityId] [uniqueidentifier] NULL,
	[CityToponym] [nvarchar](10) NULL,
	[LocalityName] [nvarchar](255) NULL,
	[LocalityId] [uniqueidentifier] NULL,
	[LocalityToponym] [nvarchar](10) NULL,
	[EtlPackageId] [uniqueidentifier] NULL,
	[EtlSessionId] [uniqueidentifier] NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
	[ModifiedUtcDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;

GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='LocationLocalization' AND TABLE_SCHEMA='Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[LocationLocalization](
	[Id] [uniqueidentifier] NOT NULL,
	[Locale] [nvarchar](50) NOT NULL,
	[Name] [decimal](10, 6) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
	[ModifiedUtcDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_LocationLocalization] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;

GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='LocationGeoInfo' AND TABLE_SCHEMA='Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[LocationGeoInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[GeoSystem] [nvarchar](255) NOT NULL,
	[Lat] [decimal](10, 6) NULL,
	[Lng] [decimal](10, 6) NULL,
	[GeoCodingStatus] [tinyint] NULL,
	[GeoCodingAccuracy] [int] NULL,
	[GeoDateTime] [datetime] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NULL,
	[EtlSessionId] [uniqueidentifier] NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
	[ModifiedUtcDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_LocationGeoInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;

GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='TradePoint' AND TABLE_SCHEMA='Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[TradePoint](
	[LocationId] [uniqueidentifier] NOT NULL,
	[ExternalId] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[TypeId] [uniqueidentifier] NULL,
	[EtlPackageId] [uniqueidentifier] NULL,
	[EtlSessionId] [uniqueidentifier] NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
	[ModifiedUtcDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TradePoint] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;
GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='TradePointType' AND TABLE_SCHEMA='Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[TradePointType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
	[ModifiedUtcDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TradePointType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;

GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='IPLocation' AND TABLE_SCHEMA='Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[IPLocation](
	[Id] [bigint] NOT NULL,
	[IPV4From] [bigint] NOT NULL,
	[IPV4To] [bigint] NOT NULL,
	[IPV4FromString] [nvarchar](30) NOT NULL,
	[IPV4ToString] [nvarchar](30) NOT NULL,
	[Company] [nvarchar](255) NULL,
	[LocationId] [uniqueidentifier] NULL,
	[EtlPackageId] [uniqueidentifier] NULL,
	[EtlSessionId] [uniqueidentifier] NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ModifiedDateTime] [datetime] NOT NULL,
	[ModifiedUtcDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_IPLocation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;

GO

IF NOT EXISTS (SELECT * FROM Geopoints.LocationType)
BEGIN
	INSERT INTO [Geopoints].[LocationType]([Id], [Name], [CreatedDateTime], [CreatedUtcDateTime], [ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT 0, N'Страна', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 1, N'Регион', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 2, N'Район', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 3, N'Город', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 4, N'Населенный пункт', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 5, N'Улица', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 6, N'Зарезервировано для будущего использования', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 7, N'Зарезервировано для будущего использования', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE() UNION ALL
	SELECT 8, N'Точка', GETDATE(), GETUTCDATE(), GETDATE(), GETUTCDATE()
END;

GO

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'V_IPLocation' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('CREATE VIEW [Geopoints].[V_IPLocation] AS SELECT * FROM Geopoints.IPLocation');
END;

GO

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'V_Location' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('CREATE VIEW [Geopoints].[V_Location] AS SELECT * FROM Geopoints.Location');
END;

GO

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'V_LocationGeoInfo' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('CREATE VIEW [Geopoints].[V_LocationGeoInfo] AS SELECT * FROM Geopoints.LocationGeoInfo');
END;

GO

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'V_LocationLocalization' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('CREATE VIEW [Geopoints].[V_LocationLocalization] AS SELECT * FROM Geopoints.LocationLocalization');
END;

GO

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'V_LocationType' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('CREATE VIEW [Geopoints].[V_LocationType] AS SELECT * FROM Geopoints.LocationType');
END;

GO

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'V_TradePoint' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('CREATE VIEW [Geopoints].[V_TradePoint] AS SELECT * FROM Geopoints.TradePoint');
END;

GO

IF NOT EXISTS(SELECT * FROM sys.views WHERE name = 'V_TradePointType' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('CREATE VIEW [Geopoints].[V_TradePointType] AS SELECT * FROM Geopoints.TradePointType');
END;

GO