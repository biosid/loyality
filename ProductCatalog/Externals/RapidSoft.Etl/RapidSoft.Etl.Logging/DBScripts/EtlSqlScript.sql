-- usage in C#: new ResourceManager("RapidSoft.Etl.Logging.Properties.Resources", typeof(IEtlLogger).Assembly).GetString("EtlSqlTables")

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlPackages]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlPackages]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlSessions]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlSessions]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlVariables]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlVariables]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlCounters]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlCounters]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlMessages]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlMessages]

GO

CREATE TABLE [dbo].[EtlPackages](
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

CREATE TABLE [dbo].[EtlSessions](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlPackageName] [nvarchar](255) NULL,
	[StartDateTime] [datetime] NOT NULL,
	[StartUtcDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NULL,
	[EndUtcDateTime] [datetime] NULL,
	[Status] [int] NOT NULL,
	[ParentEtlSessionId] [uniqueidentifier] NULL,
	[UserName] [nvarchar](50) NULL,
	CONSTRAINT [PK_EtlSessions] PRIMARY KEY NONCLUSTERED
	(
		[EtlPackageId] ASC,
		[EtlSessionId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[EtlVariables]
(
	[EtlPackageId] uniqueidentifier NOT NULL,
	[EtlSessionId] uniqueidentifier NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	[Modifier] int NOT NULL,
	[Value] nvarchar(1000) NULL,
	[IsSecure] bit NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NOT NULL,
)

GO

CREATE TABLE [dbo].[EtlCounters]
(
	[EtlPackageId] uniqueidentifier NOT NULL,
	[EtlSessionId] uniqueidentifier NOT NULL,
	[EntityName] nvarchar(255) NOT NULL,
	[CounterName] nvarchar(255) NOT NULL,
	[CounterValue] bigint NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NOT NULL,
)

GO

CREATE TABLE [dbo].[EtlMessages](
    [SequentialId] [bigint] identity(1,1) primary key, --1
	[EtlPackageId] [uniqueidentifier] NOT NULL, --2
	[EtlSessionId] [uniqueidentifier] NOT NULL, --3
	[EtlStepName] [nvarchar](255) NULL, --4
    [MessageType] [int] NOT NULL, --5
    [Text] [nvarchar](1000) NULL, --6
	[Flags] [bigint] NULL, --6
    [StackTrace] [nvarchar](1000) NULL, --7
	[LogDateTime] [datetime] NOT NULL, --8
	[LogUtcDateTime] [datetime] NOT NULL --9
) ON [PRIMARY]

GO

-- Compatibility

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlStepSources]') and type in (N'SN'))
	DROP SYNONYM [dbo].[EtlStepSources]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlStepSources]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlStepSources]

GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlSessionVariables]') and type in (N'SN'))
	DROP SYNONYM [dbo].[EtlSessionVariables]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlSessionVariables]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlSessionVariables]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlVariables]') AND type in (N'U'))
	CREATE SYNONYM [dbo].[EtlSessionVariables] FOR [dbo].[EtlVariables]

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlParameters]') and type in (N'SN'))
	DROP SYNONYM [dbo].[EtlParameters]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlParameters]') AND type in (N'U'))
	DROP TABLE [dbo].[EtlParameters]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtlVariables]') AND type in (N'U'))
	CREATE SYNONYM [dbo].[EtlParameters] FOR [dbo].[EtlVariables]

GO