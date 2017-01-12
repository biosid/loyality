IF NOT EXISTS (
	SELECT 1
	FROM sys.columns cols (nolock) 
		join sys.tables tbls (nolock) on (tbls.object_id = cols.object_id)
		join sys.schemas shms (nolock) on (tbls.schema_id = shms.schema_id)
	WHERE tbls.name = 'Location'
	  AND cols.name = 'Toponym'
	  and shms.name = 'Geopoints'
)
BEGIN
	ALTER TABLE [Geopoints].[Location] ADD [Toponym] [nvarchar](10) NULL;
END;

GO

IF EXISTS (
	SELECT 1
	FROM sys.columns cols (nolock) 
		join sys.tables tbls (nolock) on (tbls.object_id = cols.object_id)
		join sys.schemas shms (nolock) on (tbls.schema_id = shms.schema_id)
	WHERE tbls.name = 'Location'
	  AND cols.name = 'LocalityName'
	  and shms.name = 'Geopoints'
)
BEGIN
	ALTER TABLE [Geopoints].[Location] DROP COLUMN [LocalityName];
	ALTER TABLE [Geopoints].[Location] DROP COLUMN [LocalityId];
	ALTER TABLE [Geopoints].[Location] DROP COLUMN [LocalityToponym];
	
	ALTER TABLE [Geopoints].[Location] ADD [TownName] [nvarchar](255) NULL;
	ALTER TABLE [Geopoints].[Location] ADD [TownId] [uniqueidentifier] NULL;
	ALTER TABLE [Geopoints].[Location] ADD [TownToponym] [nvarchar](10) NULL;
END;

GO

IF EXISTS(SELECT * FROM sys.views WHERE name = 'V_Location' AND schema_id = SCHEMA_ID('Geopoints'))
BEGIN
	EXECUTE('DROP VIEW [Geopoints].[V_Location]');
END;

EXECUTE('CREATE VIEW [Geopoints].[V_Location] AS SELECT * FROM Geopoints.Location');