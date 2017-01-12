/************************************************************
* EtlPackages.sql
************************************************************/


IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'99e82656-fa85-4445-bee0-a07f169f47f3')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'99e82656-fa85-4445-bee0-a07f169f47f3', N'Процедура загрузки справочника телефонных кодов', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'30AE18CB-7807-4FB5-B2A3-6D7D328291D6')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'30AE18CB-7807-4FB5-B2A3-6D7D328291D6', N'Процедура загрузки справочника назначений платежа', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'C1E03A49-AC62-449E-81CB-48E260E7F657')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'C1E03A49-AC62-449E-81CB-48E260E7F657', N'Процедура загрузки справочника налоговых сборов', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'7FCD928A-655C-4FAE-921D-3B5248A7C51D')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'7FCD928A-655C-4FAE-921D-3B5248A7C51D', N'Процедура загрузки справочника НДС', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'EFD6CC95-95F8-44D2-9785-EE3139DCB4D7')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'EFD6CC95-95F8-44D2-9785-EE3139DCB4D7', N'Процедура загрузки справочника отделений банка', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'4B930C57-6B57-49E4-AD21-86A30DB76E64')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'4B930C57-6B57-49E4-AD21-86A30DB76E64', N'Процедура редактирования списка популярных городов с сайта', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'F4B46FB5-4254-47F8-B64D-99E1D5B948DD')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'F4B46FB5-4254-47F8-B64D-99E1D5B948DD', N'Процедура редактирования списка масок запрещенных счетов с сайта', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'37C851C8-B6CA-4036-B057-150D90E1F73A')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'37C851C8-B6CA-4036-B057-150D90E1F73A', N'Процедура редактирования списка масок счетов физических лиц с сайта', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'9806c54f-8980-44f5-8cba-cbe01a328a31')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'9806c54f-8980-44f5-8cba-cbe01a328a31', N'Процедура редактирования справочника Телефонные коды', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'47D080B0-AF66-4C4B-970B-9178CFA56F91')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'47D080B0-AF66-4C4B-970B-9178CFA56F91', N'Процедура редактирования справочника Системы моментальных переводов', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'103471A0-96E2-41D3-ACFB-C045B9D5B2FE')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'103471A0-96E2-41D3-ACFB-C045B9D5B2FE', N'Процедура редактирования справочника Назначение платежей', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'42B0C885-D782-40F7-B5DE-9C780FD7718C')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'42B0C885-D782-40F7-B5DE-9C780FD7718C', N'Процедура редактирования справочника Налоговые сборы', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'E0895679-812A-43D7-8A31-A3006F0F87E9')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'E0895679-812A-43D7-8A31-A3006F0F87E9', N'Процедура редактирования справочника НДС', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'5BF680CD-DF29-470E-962C-6C05AFE96EAF')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'5BF680CD-DF29-470E-962C-6C05AFE96EAF', N'Процедура редактирования справочника Филиалы Банка', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'C8BE0E2E-9AF5-4131-B5B2-63C95F7B1441')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'C8BE0E2E-9AF5-4131-B5B2-63C95F7B1441', N'Процедура загрузки справочника Точек обслуживания', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'EAAE6E15-4DA9-4367-A6AC-F76360A4C4EB')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'EAAE6E15-4DA9-4367-A6AC-F76360A4C4EB', N'Процедура редактирования типов отделений банка с сайта', 1
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[EtlPackages] WHERE [Id] = N'AE1B38CE-66D3-4617-8342-BBD6ED6A2ACF')
BEGIN
	INSERT INTO [dbo].[EtlPackages]([Id], [Name], [Enabled])
	SELECT N'AE1B38CE-66D3-4617-8342-BBD6ED6A2ACF', N'Процедура редактирования услуг отделений банка с сайта', 1
END;

GO

/************************************************************
* Location.sql
************************************************************/

DECLARE @TblName NVARCHAR(128);
DECLARE @S NVARCHAR(MAX);
SET @S = N' VIEW [geopoints].[Location_VIEW] With schemabinding  
			AS
			SELECT [Id]
      ,[ParentId]
      ,[ExternalId]
      ,[LocationType]
      ,[Name]
      ,[Toponym]
      ,[KladrCode]
      ,[Index]
      ,[CountryId]
      ,[RegionName]
      ,[RegionId]
      ,[RegionToponym]
      ,[DistrictName]
      ,[DistrictId]
      ,[DistrictToponym]
      ,[CityName]
      ,[CityId]
      ,[CityToponym]
      ,[TownName]
      ,[TownId]
      ,[TownToponym]
      ,[IsCity]
      ,[CreatedDateTime]
      ,[CreatedUtcDateTime]
      ,[ModifiedDateTime]
      ,[ModifiedUtcDateTime]
      ,[EtlPackageId]
      ,[EtlSessionId]
			FROM  ';
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[geopoints].[Location_VIEW]'))
BEGIN
	SET @TblName = '[geopoints].' + [dbo].ShemaUtilsGetLastestTableName(N'[geopoints].Location');
	SET @S = N'ALTER ' + @S + @TblName;
END
ELSE BEGIN
	SET @TblName = [dbo].RuntimeUtilsGetTableNameWithDate('[geopoints].Location');
	EXEC [geopoints].CreateLocationTable @TblName;
	SET @S = 'CREATE ' + @S + @TblName;
END;
EXEC [dbo].MergeUtilsExecSQL @S;
GO
CREATE UNIQUE CLUSTERED INDEX [IX_MAIN] ON [geopoints].[Location_VIEW] (
        [Id] ASC
)
GO
IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'FTCatalog')
CREATE FULLTEXT CATALOG [FTCatalog]WITH ACCENT_SENSITIVITY = OFF
AS DEFAULT
AUTHORIZATION [dbo]
GO
CREATE FULLTEXT INDEX ON [geopoints].Location_VIEW(Name) KEY INDEX IX_MAIN;
GO

/************************************************************
* LocationGeoInfo.sql
************************************************************/

declare @tabname nvarchar(128);
set @tabname = dbo.RuntimeUtilsGetTableNameWithDate(N'[Geopoints].LocationGeoInfo');

exec [Geopoints].[CreateLocationGeoInfoTable] @tabname

declare @s nvarchar(max);
set @s = ' INSERT INTO ' + @tabname + '
	SELECT [Id]
      ,[GeoSystem]
      ,[Lat]
      ,[Lng]
      ,[GeoCodingStatus]
      ,[GeoCodingAccuracy]
      ,[GeoDateTime]
      ,[CreatedDateTime]
      ,[CreatedUtcDateTime]
      ,[ModifiedDateTime]
      ,[ModifiedUtcDateTime]
      ,[EtlPackageId]
      ,[EtlSessionId] 
	FROM [geopoints].[LocationGeoInfo_VIEW]';
exec [dbo].[MergeUtilsExecSQL] @s;

exec RuntimeUtilsSetViewDefinition N'[Geopoints].[LocationGeoInfo_VIEW]', @tabname;


GO

/************************************************************
* ServicePoints.sql
************************************************************/

declare @tabname nvarchar(128);
set @tabname = dbo.RuntimeUtilsGetTableNameWithDate(N'[Geopoints].ServicePoints');

exec [Geopoints].[CreateServicePointsTable] @tabname
exec RuntimeUtilsSetViewDefinition N'[Geopoints].[ServicePoints_VIEW]', @tabname;

GO
DECLARE @TblName NVARCHAR(128);
	DECLARE @S NVARCHAR(MAX);
	SET @S=N' VIEW [geopoints].[ServicePoints_VIEW] With schemabinding  
			AS
			SELECT [Id]
      ,[ParentId]
      ,[ExternalId]
      ,[LocationType]
      ,[InstantTransferSystem]
      ,[Name]
      ,[Code]
      ,[Address]
      ,[PhoneNumber]
      ,[Schedule]
      ,[Unaddressed]
      ,[Toponym]
      ,[KladrCode]
      ,[CountryId]
      ,[RegionName]
      ,[RegionId]
      ,[RegionToponym]
      ,[DistrictName]
      ,[DistrictId]
      ,[DistrictToponym]
      ,[CityName]
      ,[CityId]
      ,[CityToponym]
      ,[TownName]
      ,[TownId]
      ,[TownToponym]
      ,[CreatedDateTime]
      ,[CreatedUtcDateTime]
      ,[ModifiedDateTime]
      ,[ModifiedUtcDateTime]
      ,[EtlPackageId]
      ,[EtlSessionId]
			FROM ';
	IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[geopoints].[ServicePoints_VIEW]'))
BEGIN
	SET @TblName = N'[geopoints].' + [dbo].ShemaUtilsGetLastestTableName(N'ServicePoints');
	SET @S = N'ALTER ' + @S + @TblName;
END
ELSE BEGIN
	SET @TblName = [dbo].RuntimeUtilsGetTableNameWithDate('[geopoints].ServicePoints');
	EXEC [geopoints].CreateServicePointsTable @TblName;
	SET @S = 'CREATE ' + @S + @TblName;
END;
EXEC [dbo].MergeUtilsExecSQL @S;
GO
CREATE UNIQUE CLUSTERED INDEX [IX_MAIN] ON [geopoints].[ServicePoints_VIEW] (
        [Id] ASC
)
GO
IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'FTCatalog')
CREATE FULLTEXT CATALOG [FTCatalog]WITH ACCENT_SENSITIVITY = OFF
AS DEFAULT
AUTHORIZATION [dbo]
GO
CREATE FULLTEXT INDEX ON [geopoints].[ServicePoints_VIEW](Name) KEY INDEX IX_MAIN;
GO

