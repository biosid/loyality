
PRINT N'Creating [dbo].[ClientCardRegStatus]...'

GO
CREATE TABLE [dbo].[ClientCardRegStatus](
	[ClientId] [nvarchar](36) NOT NULL,
	[IsCardRegistered] [bit] NOT NULL,
    CONSTRAINT [PK_ClientCardRegStatus] PRIMARY KEY CLUSTERED ( [ClientId] ASC )
)

GO

PRINT N'Creating Default Constraint on [dbo].[ClientCardRegStatus]....'

GO

ALTER TABLE [dbo].[ClientCardRegStatus] ADD  CONSTRAINT [DF_ClientCardRegStatus_IsCardRegistered]  DEFAULT ((0)) FOR [IsCardRegistered]
