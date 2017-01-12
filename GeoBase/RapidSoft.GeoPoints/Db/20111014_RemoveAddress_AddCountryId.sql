ALTER TABLE [Geopoints].[Location] DROP COLUMN [Address];
ALTER TABLE [Geopoints].[Location] ADD [CountryId] [uniqueidentifier] NULL;
GO

ALTER VIEW [Geopoints].[V_Location] AS SELECT * FROM [Geopoints].[Location]