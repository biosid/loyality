IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[OKSMCountries]') AND type in (N'U'))
	DROP TABLE [Geopoints].[OKSMCountries]
GO

CREATE TABLE [Geopoints].[OKSMCountries](
	[NumberCode] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Alpha2Code] [char](2) NOT NULL,
	[Alpha3Code] [char](3) NOT NULL,
	CONSTRAINT [PK_OKSMCountries] PRIMARY KEY CLUSTERED ( [NumberCode] ASC )
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UI_OKSMCountries.Alpha2Code] ON [Geopoints].[OKSMCountries] ([Alpha2Code] ASC)
CREATE UNIQUE NONCLUSTERED INDEX [UI_OKSMCountries.Alpha3Code] ON [Geopoints].[OKSMCountries] ([Alpha3Code] ASC)
GO