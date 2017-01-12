

PRINT N'Creating [dbo].[ClientForActivation]...';


GO
CREATE TABLE [dbo].[ClientForActivation] (
    [EtlSessionId] UNIQUEIDENTIFIER NOT NULL,
    [ClientId]     NVARCHAR (36)    NOT NULL,
    [LastName]     NVARCHAR (50)    NOT NULL,
    [FirstName]    NVARCHAR (50)    NOT NULL,
    [MiddleName]   NVARCHAR (50)    NULL,
    [BirthDate]    DATE             NOT NULL,
    [Gender]       INT              NOT NULL,
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Status]       INT              NULL,
    [Reason]       NVARCHAR (128)   NULL,
    CONSTRAINT [PK_ClientForActivation_SessionId] PRIMARY KEY CLUSTERED ([ClientId] ASC, [EtlSessionId] ASC)
);


GO
PRINT N'Creating Default Constraint on [dbo].[ClientForActivation]....';


GO
ALTER TABLE [dbo].[ClientForActivation]
    ADD DEFAULT 0 FOR [Gender];


GO
ALTER TABLE [dbo].[ClientForActivation]
    ADD CONSTRAINT [FK_ClientForActivation_EtlSessions] FOREIGN KEY ([EtlSessionId]) REFERENCES [etl].[EtlSessions] ([EtlSessionId]);

