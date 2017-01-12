IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[etl].[EtlFileCounter]') AND type in (N'U'))
	DROP TABLE [etl].[EtlFileCounter]
GO

CREATE TABLE [etl].[EtlFileCounter](
	[EtlPackageId] [uniqueidentifier] NOT NULL,
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[FileTemplate] [nvarchar](255) NOT NULL,
	[FileCount] [int] NOT NULL,
	[CounterDate] [date] NOT NULL,
 CONSTRAINT [PK_EtlFileCounter] PRIMARY KEY CLUSTERED 
	(
		[FileTemplate] ASC,
		[CounterDate] ASC
	)
)
GO


