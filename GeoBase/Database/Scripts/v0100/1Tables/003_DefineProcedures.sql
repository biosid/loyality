/************************************************************
* ApplyIPRangesTableConstraints.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[ApplyIPRangesTableConstraints]';
GO
CREATE PROCEDURE [Geopoints].[ApplyIPRangesTableConstraints]
	@TableName NVARCHAR(128)	
AS
BEGIN
	DECLARE @S NVARCHAR(1024)
 	SET @S = 'CREATE NONCLUSTERED INDEX [IX_IPV4From_IPV4To] ON ' + @TableName + ' 
	(
		[IPV4From] ASC,
		[IPV4To] ASC
	)INCLUDE ( [IPV4FromString],
	[IPV4ToString],
	[Company],
	[LocationId],
	[CreatedDateTime],
	[CreatedUtcDateTime],
	[ModifiedDateTime],
	[ModifiedUtcDateTime],
	[EtlPackageId],
	[EtlSessionId]) ;'
	EXEC MergeUtilsExecSQL @S;		
END


GO

/************************************************************
* ApplyLocationGeoInfoTableConstraints.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[ApplyLocationGeoInfoTableConstraints]';
GO
CREATE PROCEDURE [Geopoints].[ApplyLocationGeoInfoTableConstraints]
	@TableName NVARCHAR(128)	
AS
BEGIN
	DECLARE @S NVARCHAR(1024)
 	SET @S = 'CREATE NONCLUSTERED INDEX [IX_LNG_LAT] ON ' + @TableName + ' 
	(
		[Lat] ASC,
		[Lng] ASC,
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY];'
	EXEC MergeUtilsExecSQL @S;		
END


GO

/************************************************************
* ApplyLocationTableConstraints.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[ApplyLocationTableConstraints]';
GO
CREATE PROCEDURE [Geopoints].[ApplyLocationTableConstraints]
	@TableName NVARCHAR(128)	
AS
BEGIN

	DECLARE @sql NVARCHAR(3000);

	SET @sql = '
	CREATE NONCLUSTERED INDEX [IX_PARENT_LOCATIONTYPE] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_REGION_TOPONYM] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_REGIONID_NAME_LOCATIONTYPE] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_DISTRICTID_NAME_LOCATIONTYPE] ON ' + @TableName + '
	(
		[DistrictId] ASC,
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
		[Index],
		[CountryId],
		[RegionName],
		[RegionId],
		[RegionToponym],
		[DistrictName],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_CITYID_NAME_LOCATIONTYPE] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_TOWNID_NAME_LOCATIONTYPE] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_LOCATIONTYPE_NAME] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_ID_NAME] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_ID] ON ' + @TableName + '
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
		[Index],
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
		[ModifiedUtcDateTime],
		[IsCity]
	);';
	EXEC sp_executesql @sql;
	
	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_ISCITY] ON ' + @TableName + '
	(
		[IsCity],
		[Name]
	)
	INCLUDE
	(
		[Id],
		[ParentId],
		[ExternalId],
		[LocationType],
		[Toponym],
		[KladrCode],
		[Index],
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
	
	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_KLADRCODE] ON ' + @TableName + '
	(
		[KladrCode]
	)
	INCLUDE
	(
		[Id],
		[IsCity],
		[Name],
		[ParentId],
		[ExternalId],
		[LocationType],
		[Toponym],
		[Index],
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
	
	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_COUNTRYID_NAME_LOCATIONTYPE] ON ' + @TableName + '
	(
        [CountryId] ASC,
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
        [Index],
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
        [IsCity],
        [CreatedDateTime],
        [CreatedUtcDateTime],
        [ModifiedDateTime],
        [ModifiedUtcDateTime],
        [EtlPackageId],
        [EtlSessionId]
	);';
	EXEC sp_executesql @sql;
	
	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_LOCATIONTYPE_INDEX] ON ' + @TableName + '
(
      [LocationType] ASC,
      [Index] ASC
)
INCLUDE ([Id],
[Name]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY];';
	EXEC sp_executesql @sql;
	
	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_INDEX] ON ' + @TableName + '
(
      [Index] ASC
)
INCLUDE ([Id]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY];';
	EXEC sp_executesql @sql;
	
	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_KLADR] ON ' + @TableName + '
(
      [KladrCode] ASC
)
INCLUDE ([Id]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY];';
	EXEC sp_executesql @sql;
	
END;
GO

/************************************************************
* ApplyServicePointsTableConstraints.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[ApplyServicePointsTableConstraints]';
GO

CREATE PROCEDURE [Geopoints].[ApplyServicePointsTableConstraints]
	@ServicePointsTName NVARCHAR(128)
AS
BEGIN

	DECLARE @sql NVARCHAR(3000);

	SET @sql = '
	CREATE NONCLUSTERED INDEX [IX_PARENT_LOCATIONTYPE] ON ' + @ServicePointsTName + '
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_REGION_TOPONYM] ON ' + @ServicePointsTName + '
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_REGIONID_NAME_LOCATIONTYPE] ON ' + @ServicePointsTName + '
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_CITYID_NAME_LOCATIONTYPE] ON ' + @ServicePointsTName + '
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_TOWNID_NAME_LOCATIONTYPE] ON ' + @ServicePointsTName + '
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_LOCATIONTYPE_NAME] ON ' + @ServicePointsTName + '
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_ID_NAME] ON ' + @ServicePointsTName + '
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_ID] ON ' + @ServicePointsTName + '
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
	
END

GO



GO

/************************************************************
* CreateFinstreamLocationsLinkTable.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[CreateFinstreamLocationsLinkTable]';
GO

GO

/************************************************************
* CreateFinstreamLocationsMapTable.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[CreateFinstreamLocationsMapTable]';
GO

GO

/************************************************************
* CreateFinstreamLocationsTable.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[CreateFinstreamLocationsTable]';
GO

GO

/************************************************************
* CreateFinstreamServicePointsTable.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[CreateFinstreamServicePointsTable]';
GO


GO

/************************************************************
* CreateIPRangesBufferTable.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[CreateIPRangesBufferTable]';
GO
CREATE PROCEDURE [Geopoints].[CreateIPRangesBufferTable]
	
AS
BEGIN
	DECLARE @tmp int;
	DECLARE @TableName NVARCHAR(128);
	SET @TableName = 'geopoints.BUFFER_IPRanges';
	DECLARE @sql NVARCHAR(2048);
	IF([dbo].[MergeUtilsCheckTableExist](@TableName) = 0)
	BEGIN
		
		SET  @sql = 'CREATE TABLE ' + @TableName + '(		
			[IPV4From] [bigint] NOT NULL,
			[IPV4To] [bigint] NOT NULL,
			[IPV4FromString] [nvarchar](30) NOT NULL,
			[IPV4ToString] [nvarchar](30) NOT NULL,
			[Country] [nvarchar](64) NULL,
			[Region] [nvarchar](64) NULL,
			[FedRegion] [nvarchar](64) NULL,
			[City] [nvarchar](64) NULL

		) ON [PRIMARY];';
		EXEC dbo.MergeUtilsExecSQL @sql;		
	END

	SET  @sql = 'DELETE FROM ' + @TableName + ';'
	EXEC dbo.MergeUtilsExecSQL @sql;
	
END

GO

/************************************************************
* CreateIPRangesTable.sql
************************************************************/


MergeUtilsDropSPIfExist '[Geopoints].[CreateIPRangesTable]';
GO
CREATE PROCEDURE [Geopoints].[CreateIPRangesTable]
	@TableName NVARCHAR(128)	
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	SET @S = 'CREATE TABLE ' + @TableName + '(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IPV4From] [bigint] NOT NULL,
	[IPV4To] [bigint] NOT NULL,
	[IPV4FromString] [nvarchar](30) NOT NULL,
	[IPV4ToString] [nvarchar](30) NOT NULL,
	[Company] [nvarchar](255) NULL,
	[LocationId] [uniqueidentifier] NULL,
	'  + [dbo].ShemaUtilsGetStandardColumns(1, 1) +   '
	 CONSTRAINT [PK_' + [dbo].[RuntimeUtilsGetPureTableName](@TableName) + '] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC 
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]';

	EXEC MergeUtilsExecSQL @S;	

END


GO

/************************************************************
* CreateLocationGeoInfoTable.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[CreateLocationGeoInfoTable]';
GO


CREATE PROCEDURE [Geopoints].[CreateLocationGeoInfoTable]
	@TableName NVARCHAR(128)	
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	SET @S = 'CREATE TABLE ' + @TableName + '(
	[Id] [uniqueidentifier] NOT NULL,
	[GeoSystem] [nvarchar](255) NOT NULL,
	[Lat] [decimal](10, 6) NULL,
	[Lng] [decimal](10, 6) NULL,
	[GeoCodingStatus] [tinyint] NULL,
	[GeoCodingAccuracy] [int] NULL,
	[GeoDateTime] [datetime] NOT NULL,
	' + [dbo].ShemaUtilsGetStandardColumns(1, 1) + ',
	 CONSTRAINT [PK_' + [dbo].[RuntimeUtilsGetPureTableName](@TableName) + '] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC 
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]';

	EXEC MergeUtilsExecSQL @S;	
	
	EXEC [geopoints].ApplyLocationGeoInfoTableConstraints @TableName;

END
GO

/************************************************************
* CreateLocationTable.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[CreateLocationTable]';
GO


CREATE PROCEDURE [Geopoints].[CreateLocationTable]
	@TableName NVARCHAR(128)	
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	SET @S = 'CREATE TABLE ' + @TableName + '(
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[ExternalId] [nvarchar](50) NULL,
	[LocationType] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Toponym] [nvarchar](10) NULL,
	[KladrCode] [nvarchar](20) NULL,
	[Index] [nvarchar](6) NULL,
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
	[IsCity] [BIT] NOT NULL DEFAULT 0,
	' + [dbo].ShemaUtilsGetStandardColumns(1, 1) + ',
	 CONSTRAINT [PK_' + [dbo].[RuntimeUtilsGetPureTableName](@TableName) + '] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC 
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]';

	EXEC MergeUtilsExecSQL @S;	

END
GO

/************************************************************
* CreateServicePointsTable.sql
************************************************************/

MergeUtilsDropSPIfExist '[Geopoints].[CreateServicePointsTable]';
GO


CREATE PROCEDURE [Geopoints].[CreateServicePointsTable]
	@TableName NVARCHAR(128)	
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	SET @S = 'CREATE TABLE ' + @TableName + '(
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[ExternalId] [nvarchar](50) NULL,
	[LocationType] [int] NOT NULL,
	[InstantTransferSystem] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Address] [nvarchar](250) NULL,
	[PhoneNumber] [nvarchar](250) NULL,
	[Schedule] [nvarchar](250) NULL,
	[Currency] [nvarchar](50) NULL,
	[Summa] [nvarchar](250) NULL,
	[MaxSumma] [nvarchar](250) NULL,
	[Description] [nvarchar](500) NULL,
	[Unaddressed] [bit] NULL,
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
	' + [dbo].ShemaUtilsGetStandardColumns(1, 1) + ',
	 CONSTRAINT [PK_' + [dbo].[RuntimeUtilsGetPureTableName](@TableName) + '] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC 
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]';

	EXEC MergeUtilsExecSQL @S;	

END
GO

