IF NOT EXISTS(SELECT * FROM sys.tables where [name] = 'EmailBlackList')
BEGIN
	CREATE TABLE [mess].[EmailBlackList](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ClientEmail] [nvarchar](255) NOT NULL,
		[InsertedDate] [datetime] NOT NULL,
	 CONSTRAINT [PK_EmailBlackList] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [mess].[EmailBlackList] ADD  CONSTRAINT [DF_EmailBlackList_InsertedDate]  DEFAULT (getdate()) FOR [InsertedDate]

	CREATE NONCLUSTERED INDEX [IX_EmailBlackList_ClientEmail] ON [mess].[EmailBlackList] ([ClientEmail] ASC)
END


