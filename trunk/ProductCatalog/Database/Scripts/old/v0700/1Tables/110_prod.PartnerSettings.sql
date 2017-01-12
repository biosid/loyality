/****** Object:  Table [prod].[PartnerSettings]    Script Date: 29.07.2013 9:37:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [prod].[PartnerSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_PartnerSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [prod].[PartnerSettings]  WITH CHECK ADD  CONSTRAINT [FK_PartnerSettings_Partners] FOREIGN KEY([PartnerId])
REFERENCES [prod].[Partners] ([Id])
GO

ALTER TABLE [prod].[PartnerSettings] CHECK CONSTRAINT [FK_PartnerSettings_Partners]
GO