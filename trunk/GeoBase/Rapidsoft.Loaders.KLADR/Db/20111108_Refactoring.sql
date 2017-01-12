IF EXISTS (
       SELECT *
       FROM   INFORMATION_SCHEMA.TABLES
       WHERE  TABLE_NAME = 'BufferCityLocation'
              AND TABLE_SCHEMA = 'Geopoints'
   )
BEGIN
    EXEC sp_rename '[Geopoints].[BufferCityLocation]', 'BUFFER_CityLocation';
END

IF EXISTS (
       SELECT *
       FROM   INFORMATION_SCHEMA.TABLES
       WHERE  TABLE_NAME = 'BufferDestinationLocation'
              AND TABLE_SCHEMA = 'Geopoints'
   )
BEGIN
    EXEC sp_rename '[Geopoints].[BufferDestinationLocation]', 'BUFFER_DestinationLocation';
END

IF EXISTS (
       SELECT *
       FROM   INFORMATION_SCHEMA.TABLES
       WHERE  TABLE_NAME = 'BufferStreetLocation'
              AND TABLE_SCHEMA = 'Geopoints'
   )
BEGIN
    EXEC sp_rename '[Geopoints].[BufferStreetLocation]', 'BUFFER_StreetLocation';
END
GO

MergeUtilsDropSPIfExist '[Geopoints].[CreateLocationTable]';
GO
CREATE PROCEDURE [Geopoints].[CreateLocationTable]
	@Date NVARCHAR(8)
AS
BEGIN
	DECLARE @SQL nvarchar(4000)
	SET @SQL = N'CREATE TABLE [Geopoints].[Location_' + @Date + N'](
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
GO


MergeUtilsDropSPIfExist '[Geopoints].[RemapKladrView]';
GO
CREATE PROCEDURE [Geopoints].[RemapKladrView]
@Date nvarchar(20)
AS
BEGIN
	DECLARE @Cmd nvarchar(200);
	SET @Cmd = N'ALTER VIEW [Geopoints].[Location_VIEW] AS SELECT * FROM [Geopoints].[Location_' + @Date + N']';
	EXEC sp_executesql @Cmd;
END;
GO


MergeUtilsDropSPIfExist '[Geopoints].[SaveDataToLocality]';
GO
CREATE PROCEDURE [Geopoints].[SaveDataToLocality]
@EtlPackageId uniqueidentifier,
@EtlSessionId uniqueidentifier,
@Date nvarchar(20)
AS
BEGIN
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[Location_' + @Date + ']');
	
	DECLARE @CREATELOCATION nvarchar(MAX);
	SET @CREATELOCATION = N'CREATE TABLE [Geopoints].[Location_' + @Date + '](
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
	 CONSTRAINT [PK_Location_' + @Date +'] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]'
	EXEC sp_executesql @CREATELOCATION;

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE TABLE [Geopoints].[Location_' + @Date + ']');
	
	DECLARE @ModifiedDateTime datetime;
	DECLARE @ModifiedUtcDateTime datetime;

	SET @ModifiedDateTime = GETDATE()
	SET @ModifiedUtcDateTime = GETUTCDATE()

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BUFFER_DestinationLocation]->[Geopoints].[Location_' + @Date + ']');
	
	DECLARE @BUFFER_DestinationLocation2Location nvarchar(1500);
	DECLARE @BUFFER_DestinationLocation2LocationParm NVARCHAR(500);
	SET @BUFFER_DestinationLocation2Location = N'INSERT INTO [Geopoints].[Location_' + @Date + ']
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
	FROM [Geopoints].[BUFFER_DestinationLocation]
	WHERE EtlPackageId IS NULL OR [EtlPackageId] <> @EtlPackageId';
	SET @BUFFER_DestinationLocation2LocationParm = N'@EtlPackageId uniqueidentifier';
	EXEC sp_executesql @BUFFER_DestinationLocation2Location, @BUFFER_DestinationLocation2LocationParm, @EtlPackageId = @EtlPackageId;
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BUFFER_DestinationLocation]->[Geopoints].[Location_' + @Date + ']');


	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BUFFER_CityLocation]->[Geopoints].[Location_' + @Date + ']');

	DECLARE @BUFFER_CityLocation2Location nvarchar(1500);
	DECLARE @BUFFER_CityLocation2LocationParm NVARCHAR(500);
	SET @BUFFER_CityLocation2Location = N'INSERT INTO [Geopoints].[Location_' + @Date + ']
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
	FROM [Geopoints].[BUFFER_CityLocation]';
	SET @BUFFER_CityLocation2LocationParm = N'@EtlPackageId uniqueidentifier, @EtlSessionId uniqueidentifier, @ModifiedDateTime datetime, @ModifiedUtcDateTime datetime';
	EXEC sp_executesql @BUFFER_CityLocation2Location, @BUFFER_CityLocation2LocationParm, @EtlPackageId = @EtlPackageId,
	@EtlSessionId = @EtlSessionId, @ModifiedDateTime = @ModifiedDateTime, @ModifiedUtcDateTime = @ModifiedUtcDateTime;
	
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BUFFER_CityLocation]->[Geopoints].[Location_' + @Date + ']');

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BUFFER_StreetLocation]->[Geopoints].[Location_' + @Date + ']');
	
	DECLARE @BUFFER_StreetLocation2Location nvarchar(1500);
	DECLARE @BUFFER_StreetLocation2LocationParm NVARCHAR(500);
	
	SET @BUFFER_StreetLocation2Location = N'INSERT INTO [Geopoints].[Location_' + @Date + ']
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
	FROM [Geopoints].[BUFFER_StreetLocation]';
	SET @BUFFER_StreetLocation2LocationParm = N'@EtlPackageId uniqueidentifier, @EtlSessionId uniqueidentifier, @ModifiedDateTime datetime, @ModifiedUtcDateTime datetime';
	EXEC sp_executesql @BUFFER_StreetLocation2Location, @BUFFER_StreetLocation2LocationParm, @EtlPackageId = @EtlPackageId,
	@EtlSessionId = @EtlSessionId, @ModifiedDateTime = @ModifiedDateTime, @ModifiedUtcDateTime = @ModifiedUtcDateTime

	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BUFFER_StreetLocation]->[Geopoints].[Location_' + @Date + ']');


	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE INDEX. [Geopoints].[Location_' + @Date + ']');

	DECLARE @sql NVARCHAR(3000);

	SET @sql = '
	CREATE NONCLUSTERED INDEX [IX_PARENT_LOCATIONTYPE] ON [Geopoints].[Location_' + @Date + ']
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_REGION_TOPONYM] ON [Geopoints].[Location_' + @Date + '] 
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_REGIONID_NAME_LOCATIONTYPE] ON [Geopoints].[Location_' + @Date + '] 
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_CITYID_NAME_LOCATIONTYPE] ON [Geopoints].[Location_' + @Date + '] 
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_TOWNID_NAME_LOCATIONTYPE] ON [Geopoints].[Location_' + @Date + '] 
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_LOCATIONTYPE_NAME] ON [Geopoints].[Location_' + @Date + '] 
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_ID_NAME] ON [Geopoints].[Location_' + @Date + '] 
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

	SET @sql = 'CREATE NONCLUSTERED INDEX [IX_NAME_ID] ON [Geopoints].[Location_' + @Date + '] 
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
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END CREATE INDEX. [Geopoints].[Location_' + @Date + ']');
END;
GO
