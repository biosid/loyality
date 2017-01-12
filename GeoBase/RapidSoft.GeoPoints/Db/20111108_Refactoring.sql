
DECLARE @viewName NVARCHAR(128)
DECLARE @tableName NVARCHAR(128)
DECLARE @newTableNmae NVARCHAR(128)


SET @viewName = '[Geopoints].[Location_VIEW]';
SET @tableName = [dbo].[RuntimeUtilsGetViewTableName] (@viewName);
SET @newTableNmae = [dbo].[RuntimeUtilsGetTableNameWithDate]('Location');

EXEC sp_rename @tableName, @newTableNmae;
SET @newTableNmae = '[Geopoints].' + @newTableNmae;
EXEC [RuntimeUtilsSetViewDefinition] @viewName, @newTableNmae;


SET @viewName = '[Geopoints].[LocationGeoInfo_VIEW]';
SET @tableName = [dbo].[RuntimeUtilsGetViewTableName] (@viewName);
SET @newTableNmae = [dbo].[RuntimeUtilsGetTableNameWithDate]('LocationGeoInfo');

EXEC sp_rename @tableName, @newTableNmae;
SET @newTableNmae = '[Geopoints].' + @newTableNmae;
EXEC [RuntimeUtilsSetViewDefinition] @viewName, @newTableNmae;


SET @viewName = '[Geopoints].[LocationLocalization_VIEW]';
SET @tableName = [dbo].[RuntimeUtilsGetViewTableName] (@viewName);
SET @newTableNmae = [dbo].[RuntimeUtilsGetTableNameWithDate]('LocationLocalization');

EXEC sp_rename @tableName, @newTableNmae;
SET @newTableNmae = '[Geopoints].' + @newTableNmae;
EXEC [RuntimeUtilsSetViewDefinition] @viewName, @newTableNmae;


SET @viewName = '[Geopoints].[LocationType_VIEW]';
SET @tableName = [dbo].[RuntimeUtilsGetViewTableName] (@viewName);
SET @newTableNmae = [dbo].[RuntimeUtilsGetTableNameWithDate]('LocationType');

EXEC sp_rename @tableName, @newTableNmae;
SET @newTableNmae = '[Geopoints].' + @newTableNmae;
EXEC [RuntimeUtilsSetViewDefinition] @viewName, @newTableNmae;


SET @viewName = '[Geopoints].[TradePoint_VIEW]';
SET @tableName = [dbo].[RuntimeUtilsGetViewTableName] (@viewName);
SET @newTableNmae = [dbo].[RuntimeUtilsGetTableNameWithDate]('TradePoint');

EXEC sp_rename @tableName, @newTableNmae;
SET @newTableNmae = '[Geopoints].' + @newTableNmae;
EXEC [RuntimeUtilsSetViewDefinition] @viewName, @newTableNmae;


SET @viewName = '[Geopoints].[TradePointType_VIEW]';
SET @tableName = [dbo].[RuntimeUtilsGetViewTableName] (@viewName);
SET @newTableNmae = [dbo].[RuntimeUtilsGetTableNameWithDate]('TradePointType');

EXEC sp_rename @tableName, @newTableNmae;
SET @newTableNmae = '[Geopoints].' + @newTableNmae;
EXEC [RuntimeUtilsSetViewDefinition] @viewName, @newTableNmae;
