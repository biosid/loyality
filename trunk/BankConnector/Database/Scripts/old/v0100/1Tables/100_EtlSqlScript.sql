PRINT N'Creating [etl]...';


GO
CREATE SCHEMA [etl]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating [etl].[EtlCounters]...';


GO
CREATE TABLE [etl].[EtlCounters](
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[EntityName] [nvarchar](255) NOT NULL,
	[CounterName] [nvarchar](255) NOT NULL,
	[CounterValue] [bigint] NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogUtcDateTime] [datetime] NOT NULL
) ON [PRIMARY]

GO
PRINT N'Creating [etl].[EtlVariables]...';


GO
CREATE TABLE [etl].[EtlVariables] (
    [EtlPackageId]   UNIQUEIDENTIFIER NOT NULL,
    [EtlSessionId]   UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (50)    NOT NULL,
    [Modifier]       INT              NOT NULL,
    [Value]          NVARCHAR (1000)  NULL,
    [IsSecure]       BIT              NOT NULL,
    [LogDateTime]    DATETIME         NOT NULL,
    [LogUtcDateTime] DATETIME         NOT NULL
);


GO
PRINT N'Creating [etl].[EtlSessions]...';


GO
CREATE TABLE [etl].[EtlSessions] (
    [EtlSessionId]       UNIQUEIDENTIFIER NOT NULL,
    [EtlPackageId]       UNIQUEIDENTIFIER NOT NULL,
    [EtlPackageName]     NVARCHAR (255)   NULL,
    [StartDateTime]      DATETIME         NOT NULL,
    [StartUtcDateTime]   DATETIME         NOT NULL,
    [EndDateTime]        DATETIME         NULL,
    [EndUtcDateTime]     DATETIME         NULL,
    [Status]             INT              NOT NULL,
    [ParentEtlSessionId] UNIQUEIDENTIFIER NULL,
    [UserName]           NVARCHAR (50)    NULL,
    CONSTRAINT [PK_EtlSessions_EtlPackageIdEtlSessionId] PRIMARY KEY NONCLUSTERED ([EtlPackageId] ASC, [EtlSessionId] ASC),
    CONSTRAINT [AK_EtlSessions_EtlSessionId] UNIQUE NONCLUSTERED ([EtlSessionId] ASC)
);


GO
PRINT N'Creating [etl].[EtlPackages]...';


GO
CREATE TABLE [etl].[EtlPackages] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [Name]               NVARCHAR (255)   NOT NULL,
    [Text]               NVARCHAR (MAX)   NULL,
    [RunIntervalSeconds] INT              NOT NULL,
    [Enabled]            BIT              NULL,
    CONSTRAINT [PK_EtlPackages_Id] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [etl].[EtlMessages]...';


GO
CREATE TABLE [etl].[EtlMessages] (
    [SequentialId]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [EtlPackageId]   UNIQUEIDENTIFIER NOT NULL,
    [EtlSessionId]   UNIQUEIDENTIFIER NOT NULL,
    [EtlStepName]    NVARCHAR (255)   NULL,
    [MessageType]    INT              NOT NULL,
    [Text]           NVARCHAR (MAX)  NULL,
    [Flags]          BIGINT           NULL,
    [StackTrace]     NVARCHAR (MAX)  NULL,
    [LogDateTime]    DATETIME         NOT NULL,
    [LogUtcDateTime] DATETIME         NOT NULL,
    CONSTRAINT [PK_EtlMessages_SequentialId] PRIMARY KEY CLUSTERED ([SequentialId] ASC)
);

GO
PRINT N'Creating [etl].[EtlSessionVariables]...';


GO
CREATE SYNONYM [etl].[EtlSessionVariables] FOR [etl].[EtlVariables];


GO
PRINT N'Creating [etl].[EtlParameters]...';


GO
CREATE SYNONYM [etl].[EtlParameters] FOR [etl].[EtlVariables];


GO
PRINT N'Creating DF_EtlPackages_RunIntervalSeconds...';


GO
ALTER TABLE [etl].[EtlPackages]
    ADD CONSTRAINT [DF_EtlPackages_RunIntervalSeconds] DEFAULT ((0)) FOR [RunIntervalSeconds];

GO
ALTER TABLE [etl].[EtlCounters]
    ADD CONSTRAINT [FK_EtlCounters_EtlPackageId] FOREIGN KEY ([EtlPackageId]) REFERENCES [etl].[EtlPackages] ([Id]);


GO
PRINT N'Creating FK_EtlCounters_EtlSessions...';


GO
ALTER TABLE [etl].[EtlCounters]
    ADD CONSTRAINT [FK_EtlCounters_EtlSessions] FOREIGN KEY ([EtlSessionId]) REFERENCES [etl].[EtlSessions] ([EtlSessionId]);


GO
PRINT N'Creating FK_EtlVariables_EtlPackageId...';


GO
ALTER TABLE [etl].[EtlVariables]
    ADD CONSTRAINT [FK_EtlVariables_EtlPackageId] FOREIGN KEY ([EtlPackageId]) REFERENCES [etl].[EtlPackages] ([Id]);


GO
PRINT N'Creating FK_EtlVariables_EtlSessions...';


GO
ALTER TABLE [etl].[EtlVariables]
    ADD CONSTRAINT [FK_EtlVariables_EtlSessions] FOREIGN KEY ([EtlSessionId]) REFERENCES [etl].[EtlSessions] ([EtlSessionId]);


GO
PRINT N'Creating FK_EtlSessions_ToTable...';


GO
ALTER TABLE [etl].[EtlSessions]
    ADD CONSTRAINT [FK_EtlSessions_ToTable] FOREIGN KEY ([EtlPackageId]) REFERENCES [etl].[EtlPackages] ([Id]);


GO
PRINT N'Creating FK_EtlMessages_EtlPackageId...';


GO
ALTER TABLE [etl].[EtlMessages]
    ADD CONSTRAINT [FK_EtlMessages_EtlPackageId] FOREIGN KEY ([EtlPackageId]) REFERENCES [etl].[EtlPackages] ([Id]);


GO
PRINT N'Creating FK_EtlMessages_EtlSessions...';


GO
ALTER TABLE [etl].[EtlMessages]
    ADD CONSTRAINT [FK_EtlMessages_EtlSessions] FOREIGN KEY ([EtlSessionId]) REFERENCES [etl].[EtlSessions] ([EtlSessionId]);

