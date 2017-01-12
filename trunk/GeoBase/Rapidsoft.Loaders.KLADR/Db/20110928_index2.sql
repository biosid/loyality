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
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN CREATE TABLE [Geopoints].[Location' + @Date + ']');
	DECLARE @CREATELOCATION nvarchar(MAX);
	SET @CREATELOCATION = N'CREATE TABLE [Geopoints].[Location' + @Date + '](
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[LocationType] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Toponym] [nvarchar](10) NULL,
	[KladrCode] [nvarchar](20) NULL,
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
			   ,[Name], [Toponym], [KladrCode]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime]
	FROM [Geopoints].[BufferDestinationLocation]
	WHERE [EtlPackageId] <> @EtlPackageId';
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
			   ,[Name], [Toponym], [KladrCode]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode]
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

	Print 'Перенос данных BufferStreetLocation -> Location'
	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'BEGIN TRANSFER. [Geopoints].[BufferStreetLocation]->[Geopoints].[Location' + @Date + ']');
	DECLARE @BufferStreetLocation2Location nvarchar(1500);
	DECLARE @BufferStreetLocation2LocationParm NVARCHAR(500);
	
	SET @BufferStreetLocation2Location = N'INSERT INTO [Geopoints].[Location' + @Date + ']
			   ([Id], [ParentId], [LocationType]
			   ,[Name], [Toponym], [KladrCode]
			   ,[RegionName], [RegionId], [RegionToponym]
			   ,[DistrictName], [DistrictId], [DistrictToponym]
			   ,[CityName], [CityId], [CityToponym]
			   ,[TownName], [TownId], [TownToponym]
			   ,[EtlPackageId],[EtlSessionId]
			   ,[CreatedDateTime], [CreatedUtcDateTime]
			   ,[ModifiedDateTime], [ModifiedUtcDateTime])
	SELECT [Id], [ParentId], [LocationType]
	   ,[Name], [Toponym], [KladrCode]
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


	DECLARE @sql NVARCHAR(500);

	SET @sql = '
	CREATE NONCLUSTERED INDEX [IX_PARENT_LOCATIONTYPE] ON [Geopoints].[Location' + @Date + ']
	(
		[ParentId] ASC,
		[LocationType] ASC
	);

	CREATE NONCLUSTERED INDEX [IX_NAME_REGION_TOPONYM] ON [Geopoints].[Location' + @Date + '] 
	(
		[Name] ASC,
		[RegionName] ASC,
		[Toponym] ASC
	)';

	EXEC sp_executesql @sql;





	INSERT INTO [dbo].[EtlMessages] ([EtlPackageId], [EtlSessionId], [LogDateTime], [LogUtcDateTime], [MessageType], [Text])
    VALUES (@EtlPackageId, @EtlSessionId, GETDATE(), GETUTCDATE(),8, 'END TRANSFER. [Geopoints].[BufferStreetLocation]->[Geopoints].[Location' + @Date + ']');
END;

GO