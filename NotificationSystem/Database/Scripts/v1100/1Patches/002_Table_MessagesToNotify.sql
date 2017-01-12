/****** Object:  Table [mess].[MessagesToNotify]    Script Date: 04/03/2014 16:16:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [mess].[MessagesToNotify]
(
    [Id] [uniqueidentifier] NOT NULL,
    [ThreadId] [uniqueidentifier] NOT NULL,
    [MessageIndex] [int] NOT NULL,
    [MessageTime] [datetime] NOT NULL,
    CONSTRAINT [PK_MessagesToNotify] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )
    WITH
    (
        PAD_INDEX  = OFF,
        STATISTICS_NORECOMPUTE  = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS  = ON,
        ALLOW_PAGE_LOCKS  = ON
    )
    ON [PRIMARY]
) ON [PRIMARY]

GO
