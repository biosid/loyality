/************************************************************
* EtlTables.sql
************************************************************/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlPackages]') AND type in (N'U'))
DROP TABLE [dbo].[EtlPackages]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlEntities]') AND type in (N'U'))
DROP TABLE [dbo].[EtlEntities]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlSessions]') AND type in (N'U'))
DROP TABLE [dbo].[EtlSessions]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlEntityCounters]') AND type in (N'U'))
DROP TABLE [dbo].[EtlEntityCounters]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlMessages]') AND type in (N'U'))
DROP TABLE [dbo].[EtlMessages]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlSessionVariables]') AND type in (N'U'))
DROP TABLE [dbo].[EtlSessionVariables]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlFiles]') AND type in (N'U'))
DROP TABLE [dbo].[EtlFiles]

GO

CREATE TABLE [dbo].[EtlPackages](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Enabled] [bit] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[EtlSessions](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[StartUtcDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[EndUtcDateTime] [datetime] NULL,
	[Status] [int] NOT NULL,
	[StartMessage] [nvarchar](1000) NULL,
	[EndMessage] [nvarchar](1000) NULL,
	[ParentEtlSessionId] [uniqueidentifier] NULL,
	[UserName] [nvarchar](50) NULL,
	CONSTRAINT [PK_EtlSessions] PRIMARY KEY NONCLUSTERED
	(
		[EtlPackageId] ASC,
		[EtlSessionId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[EtlEntityCounters](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlEntityName] [nvarchar](255) NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NULL,
	[CounterName] nvarchar(50) NOT NULL,
	[CounterValue] [bigint] NOT NULL,
	[EtlPackageStepId] [uniqueidentifier] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[EtlMessages](
    [SequentialId] [bigint] identity(1,1) primary key,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlPackageStepId] [uniqueidentifier] NULL,
	[EtlPackageStepIndex] int NOT NULL DEFAULT(0),
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NULL,
    [MessageType] int NOT NULL,
    [Text] nvarchar(1000) NULL,
    [RecordIndex] bigint NULL,
    [AffectedObjectCount] bigint NULL,
    [ErrorType] nvarchar(255) NULL,
    [StackTrace] nvarchar(1000) NULL
) ON [PRIMARY]

GO

CREATE TABLE [EtlSessionVariables]
(
	[EtlPackageId] uniqueidentifier NOT NULL,
	[EtlSessionId] uniqueidentifier NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NULL,
	[VariableName] nvarchar(50) NOT NULL,
	[VariableValue] nvarchar(1000) NULL,
	[IsSecure] bit NOT NULL,
	[IsExternal] bit NOT NULL,
)

GO

CREATE TABLE [EtlFiles]
(
	[EtlPackageId] uniqueidentifier NOT NULL,
	[EtlSessionId] uniqueidentifier NOT NULL,
	[FileId] uniqueidentifier NOT NULL,
	[ParentId] uniqueidentifier NULL,
	[InsertedDateTime] datetime NOT NULL,
	[InsertedUtcDateTime] datetime NULL,
	[FileName] nvarchar(1000) NOT NULL,
	[FileExtension] nvarchar(50) NULL,
	[FileSizeBytes] bigint NULL,
	[FileCreatedDateTime] datetime NULL,
	[FileModifiedDateTime] datetime NULL,
	[FileData] nvarchar(max) NULL,
	CONSTRAINT [PK_EtlFiles] PRIMARY KEY NONCLUSTERED
	(
		[EtlPackageId] ASC,
		[EtlSessionId] ASC,
		[FileId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)

GO




GO

/************************************************************
* SynchronizationJobs.sql
************************************************************/

IF dbo.MergeUtilsCheckTableExist('[dbo].[SynchronizationJobs]') = 0
BEGIN
CREATE TABLE [dbo].[SynchronizationJobs](
	[Id] [uniqueidentifier] NOT NULL,
	[SequentalId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProcedureName] [nvarchar](250) NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[XmlData] [nvarchar](max) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL
) ON [PRIMARY]
END
GO

/************************************************************
* BUFFER_GoogleGeocodingCache.sql
************************************************************/


IF NOT EXISTS (SELECT 1 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='BUFFER_GoogleGeocodingCache' AND TABLE_SCHEMA = 'Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[BUFFER_GoogleGeocodingCache](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](256) NOT NULL,
	[RawResponse] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_GoogleGeocodingCache] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;
GO

/************************************************************
* BUFFER_YandexGeocodingCache.sql
************************************************************/


IF NOT EXISTS (SELECT 1 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME='BUFFER_YandexGeocodingCache' AND TABLE_SCHEMA = 'Geopoints') 
BEGIN
CREATE TABLE [Geopoints].[BUFFER_YandexGeocodingCache](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](256) NOT NULL,
	[RawResponse] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_YandexGeocodingCache] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;

GO

/************************************************************
* IPRanges.sql
************************************************************/

declare @tabname nvarchar(128);
set @tabname = dbo.RuntimeUtilsGetTableNameWithDate(N'[Geopoints].IPRanges');

exec [Geopoints].[CreateIPRangesTable] @tabname
exec RuntimeUtilsSetViewDefinition N'[Geopoints].[IPRanges_VIEW]', @tabname;

GO

/************************************************************
* Location.sql
************************************************************/

declare @tabname nvarchar(128);
set @tabname = dbo.RuntimeUtilsGetTableNameWithDate(N'[Geopoints].Location');

exec [Geopoints].[CreateLocationTable] @tabname
exec RuntimeUtilsSetViewDefinition N'[Geopoints].[Location_VIEW]', @tabname;

GO

/************************************************************
* LocationGeoInfo.sql
************************************************************/

declare @tabname nvarchar(128);
set @tabname = dbo.RuntimeUtilsGetTableNameWithDate(N'[Geopoints].LocationGeoInfo');

exec [Geopoints].[CreateLocationGeoInfoTable] @tabname
exec RuntimeUtilsSetViewDefinition N'[Geopoints].[LocationGeoInfo_VIEW]', @tabname;

GO

/************************************************************
* LocationRegionToponymDic.sql
************************************************************/

IF([dbo].[MergeUtilsCheckTableExist]('[geopoints].[LocationRegionToponymDic]') = 1) 
	DROP TABLE [geopoints].[LocationRegionToponymDic];


CREATE TABLE [geopoints].[LocationRegionToponymDic]
(
	Name NVARCHAR(32),
	CONSTRAINT PK_LocationRegionToponymDic PRIMARY KEY CLUSTERED 
	(
		[Name] ASC
	)
)


INSERT INTO [geopoints].[LocationRegionToponymDic](Name)
VALUES(N'область');

INSERT INTO [geopoints].[LocationRegionToponymDic](Name)
VALUES(N'автономный округ');


INSERT INTO [geopoints].[LocationRegionToponymDic](Name)
VALUES(N'округ');

INSERT INTO [geopoints].[LocationRegionToponymDic](Name)
VALUES(N'автономная область');

INSERT INTO [geopoints].[LocationRegionToponymDic](Name)
VALUES(N'край');

INSERT INTO [geopoints].[LocationRegionToponymDic](Name)
VALUES(N'республика');

GO

/************************************************************
* ServicePoints.sql
************************************************************/

declare @tabname nvarchar(128);
set @tabname = dbo.RuntimeUtilsGetTableNameWithDate(N'[Geopoints].ServicePoints');

exec [Geopoints].[CreateServicePointsTable] @tabname
exec RuntimeUtilsSetViewDefinition N'[Geopoints].[ServicePoints_VIEW]', @tabname;

GO

