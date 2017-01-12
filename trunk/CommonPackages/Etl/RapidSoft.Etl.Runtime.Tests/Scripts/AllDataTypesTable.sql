IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AllDataTypesTable]') AND type in (N'U'))
DROP TABLE [dbo].[AllDataTypesTable]

GO

CREATE TABLE [dbo].[AllDataTypesTable]
(
	[Id] uniqueidentifier PRIMARY KEY NOT NULL,
	[Null] nvarchar(255) NULL,
	[Boolean] bit NULL,
	[Byte] tinyint NULL,
	[DateTime] datetime NULL,
	[Decimal] numeric(29) NULL,
	[Double] float NULL,
	[Guid] uniqueidentifier,
	[Int16] smallint NULL,
	[Int32] int NULL,
	[Int64] bigint NULL,
	[Single] real NULL,
	[String] nvarchar(255) NULL,
	[EtlPackageId] uniqueidentifier NOT NULL,
	[EtlSessionId] uniqueidentifier NOT NULL,
	[EtlInsertedDateTime] datetime NOT NULL,
	[EtlInsertedUtcDateTime] datetime NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AllDataTypesTableCopy]') AND type in (N'U'))
DROP TABLE [dbo].[AllDataTypesTableCopy]

GO

CREATE TABLE [dbo].[AllDataTypesTableCopy]
(
	[Id] uniqueidentifier PRIMARY KEY NOT NULL,
	[Null] nvarchar(255) NULL,
	[Boolean] bit NULL,
	[Byte] tinyint NULL,
	[DateTime] datetime NULL,
	[Decimal] numeric(29) NULL,
	[Double] float NULL,
	[Guid] uniqueidentifier,
	[Int16] smallint NULL,
	[Int32] int NULL,
	[Int64] bigint NULL,
	[Single] real NULL,
	[String] nvarchar(255) NULL,
	[EtlPackageId] uniqueidentifier NOT NULL,
	[EtlSessionId] uniqueidentifier NOT NULL,
	[EtlInsertedDateTime] datetime NOT NULL,
	[EtlInsertedUtcDateTime] datetime NOT NULL
) ON [PRIMARY]

GO

