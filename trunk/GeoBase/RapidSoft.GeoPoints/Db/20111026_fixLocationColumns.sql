
--ВНИМАНИЕ!!! Долгий скрипт, удаляет Address из Location и пересобирает индексы
--Возможно бесполезен, если была произведена разноска КЛАДР
--СОЗДАНИЕ ТАБЛИЦЫ Location
DECLARE @tmname NVARCHAR(128);
DECLARE @s NVARCHAR(max);

SET @tmname = dbo.RuntimeUtilsGetTableNameWithDate('[Geopoints].Location');
DECLARE @OldLocationTName NVARCHAR(128);
SET @OldLocationTName = '[Geopoints].'+dbo.[ShemaUtilsGetLastestTableName] ('Location');
set @s ='
CREATE TABLE '+@tmname+'(
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
	 CONSTRAINT [PK_'+ [dbo].RuntimeUtilsGetPureTableName(@tmname) +'] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]';
EXEC [dbo].[MergeUtilsExecSQL] @s;
	
SET @s = 'INSERT INTO ' + @tmname + ' 
(		[Id],[ParentId],[LocationType],[Name],[Toponym],[KladrCode],[RegionName],
		[RegionId],[RegionToponym],[DistrictName],[DistrictId],[DistrictToponym],
		[CityName],[CityId],[CityToponym],[TownName],[TownId],[TownToponym],
		[EtlPackageId],[EtlSessionId],[CreatedDateTime],[CreatedUtcDateTime],
		[ModifiedDateTime],[ModifiedUtcDateTime] 
)
SELECT 
	[Id],[ParentId],[LocationType],[Name],[Toponym],[KladrCode],[RegionName],
		[RegionId],[RegionToponym],[DistrictName],[DistrictId],[DistrictToponym],
		[CityName],[CityId],[CityToponym],[TownName],[TownId],[TownToponym],
		[EtlPackageId],[EtlSessionId],[CreatedDateTime],[CreatedUtcDateTime],
		[ModifiedDateTime],[ModifiedUtcDateTime] 
FROM [Geopoints].V_Location;
';
EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = '
	CREATE NONCLUSTERED INDEX [IX_PARENT_LOCATIONTYPE] ON ' + @tmname + '
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = 'CREATE NONCLUSTERED INDEX [IX_NAME_REGION_TOPONYM] ON ' + @tmname + '
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = 'CREATE NONCLUSTERED INDEX [IX_REGIONID_NAME_LOCATIONTYPE] ON ' + @tmname + ' 
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = 'CREATE NONCLUSTERED INDEX [IX_CITYID_NAME_LOCATIONTYPE] ON ' + @tmname + ' 
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = 'CREATE NONCLUSTERED INDEX [IX_TOWNID_NAME_LOCATIONTYPE] ON ' + @tmname + ' 
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = 'CREATE NONCLUSTERED INDEX [IX_LOCATIONTYPE_NAME] ON ' + @tmname + ' 
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = 'CREATE NONCLUSTERED INDEX [IX_ID_NAME] ON ' + @tmname + ' 
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

	SET @s = 'CREATE NONCLUSTERED INDEX [IX_NAME_ID] ON ' + @tmname + ' 
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
	EXEC [dbo].[MergeUtilsExecSQL] @s;

----------------------------------------------------------------

EXEC RuntimeUtilsSetViewDefinition '[Geopoints].V_Location', @tmname;
IF (dbo.MergeUtilsCheckTableExist(@OldLocationTName)=1)
BEGIN

	WHILE EXISTS(select * from sys.foreign_keys where referenced_object_id = object_id(@OldLocationTName))
	BEGIN

		SELECT TOP 1 
			@s =	'ALTER TABLE ' + SCHEMA_NAME(schema_id) + '.' + OBJECT_NAME(parent_object_id) + ' DROP CONSTRAINT ' + name
		FROM sys.foreign_keys
		WHERE referenced_object_id = object_id(@OldLocationTName)
		
		EXEC dbo.MergeUtilsExecSQL @s
	END

	SET @s = 'DROP TABLE '+ @OldLocationTName;
	EXEC [dbo].[MergeUtilsExecSQL] @s;
END;