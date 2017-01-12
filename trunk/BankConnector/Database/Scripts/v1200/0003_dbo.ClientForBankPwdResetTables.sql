SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ClientForBankPwdReset' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[ClientForBankPwdReset](
        [SeqId] [int] IDENTITY(1,1) NOT NULL,
        [EtlSessionId] [uniqueidentifier] NOT NULL,
        [ClientId] [nvarchar](36) NOT NULL,
        [InsertedDate] [datetime] NOT NULL
        CONSTRAINT [PK_ClientForBankPwdReset] PRIMARY KEY CLUSTERED([SeqId] ASC)
        WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[ClientForBankPwdReset] ADD CONSTRAINT [DF_ClientForBankPwdReset_InsertedDate] DEFAULT (getdate()) FOR [InsertedDate]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ClientForBankPwdResetResponse' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[ClientForBankPwdResetResponse](
        [SeqId] [int] IDENTITY(1,1) NOT NULL,
        [EtlSessionId] [uniqueidentifier] NOT NULL,
        [ClientId] [nvarchar](36) NOT NULL,
        [Status] [int] NOT NULL,
        [Message] [nvarchar](1000) NULL,
        CONSTRAINT [PK_ClientForBankPwdResetResponse] PRIMARY KEY CLUSTERED([SeqId] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO
