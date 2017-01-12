/****** Object:  Table [dbo].[ProfileCustomFields]    Script Date: 11/14/2013 12:04:49 ******/

CREATE TABLE [dbo].[ProfileCustomFields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_ProfileCustomFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ProfileCustomFieldsValues]    Script Date: 11/14/2013 12:04:57 ******/

CREATE TABLE [dbo].[ProfileCustomFieldsValues](
	[FieldId] [int] NOT NULL,
	[ClientId] [nvarchar](36) NOT NULL,
	[Value] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ProfileCustomFieldsValues] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC,
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ProfileCustomFieldsValues]  WITH CHECK ADD  CONSTRAINT [FK_ProfileCustomFieldsValues_ProfileCustomFields] FOREIGN KEY([FieldId])
REFERENCES [dbo].[ProfileCustomFields] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProfileCustomFieldsValues] CHECK CONSTRAINT [FK_ProfileCustomFieldsValues_ProfileCustomFields]
GO

/****** Object:  Index [IX_ProfileCustomFields]    Script Date: 11/14/2013 12:07:28 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProfileCustomFields] ON [dbo].[ProfileCustomFields] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
