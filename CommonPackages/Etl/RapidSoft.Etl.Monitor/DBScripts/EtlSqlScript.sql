-- usage in C#: new ResourceManager("RapidSoft.Etl.Logging.Properties.Resources", typeof(IEtlLogger).Assembly).GetString("EtlSqlTables")

IF schema_id('sysmon') is null
	EXEC('CREATE SCHEMA sysmon')

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlPackages]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlPackages]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlSessions]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlSessions]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlParameters]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlParameters]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlDatashots]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlDatashots]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlMessages]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlMessages]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlMessages]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlMessages]

GO

CREATE TABLE [sysmon].[EtlPackages](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Text] [nvarchar](MAX) NULL,
	[RunIntervalSeconds] [int] NOT NULL DEFAULT(0),
	[Enabled] [bit] NULL,
	CONSTRAINT PK_EtlPackages PRIMARY KEY NONCLUSTERED 
	(
		Id
	)
) ON [PRIMARY]

GO

CREATE TABLE [sysmon].[EtlSessions](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[StartUtcDateTime] [datetime] NOT NULL,
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

GO

CREATE TABLE [sysmon].[EtlParameters]
(
	[EtlPackageId] uniqueidentifier NOT NULL,
	[EtlSessionId] uniqueidentifier NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	[Value] nvarchar(1000) NULL,
	[IsSecure] bit NOT NULL,
	[IsExternal] bit NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NOT NULL,
)

GO

CREATE TABLE [sysmon].[EtlDatashots](
	[EtlPackageId] [uniqueidentifier] NOT NULL, --1
	[EtlSessionId] [uniqueidentifier] NOT NULL, --2
	[EtlStepId] [uniqueidentifier] NOT NULL, --3
	[IsDestination] [bit] NOT NULL, --4
	[Name] [nvarchar](255) NOT NULL, --5
	[Path] [nvarchar](2000) NOT NULL, --6
	[LogDateTime] [datetime] NOT NULL, --7
	[LogUtcDateTime] [datetime] NOT NULL, --8
	[ContentSizeBytes] [bigint] NULL, --9
	[Content] [nvarchar](max) NULL --10
) ON [PRIMARY]

GO

CREATE TABLE [sysmon].[EtlMessages](
    [SequentialId] [bigint] identity(1,1) primary key, --1
	[EtlPackageId] [uniqueidentifier] NOT NULL, --2
	[EtlSessionId] [uniqueidentifier] NOT NULL, --3
	[EtlStepId] [uniqueidentifier] NULL, --4
    [MessageType] [int] NOT NULL, --5
    [Text] [nvarchar](1000) NULL, --6
	[Flags] [bigint] NULL, --6
    [StackTrace] [nvarchar](1000) NULL, --7
	[LogDateTime] [datetime] NOT NULL, --8
	[LogUtcDateTime] [datetime] NOT NULL --9
) ON [PRIMARY]

GO

-- Compatibility

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlStepSources]') and type in (N'SN'))
	DROP SYNONYM [sysmon].[EtlStepSources]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlStepSources]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlStepSources]

GO

CREATE SYNONYM [sysmon].[EtlStepSources] FOR [sysmon].[EtlDatashots]

GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlSessionVariables]') and type in (N'SN'))
	DROP SYNONYM [sysmon].[EtlSessionVariables]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sysmon].[EtlSessionVariables]') AND type in (N'U'))
	DROP TABLE [sysmon].[EtlSessionVariables]

GO

CREATE SYNONYM [sysmon].[EtlSessionVariables] FOR [sysmon].[EtlVariables]

GO