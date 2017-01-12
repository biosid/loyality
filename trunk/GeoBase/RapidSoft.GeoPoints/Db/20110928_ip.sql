GO
MergeUtilsDropSPIfExist '[geopoints].[IPLocationCreateTable]';
GO 

MergeUtilsDropSPIfExist '[geopoints].[IPRangesCreateTable]';

GO


CREATE PROCEDURE [geopoints].[IPRangesCreateTable]
	@TableName NVARCHAR(128)	
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	SET @S = 'CREATE TABLE ' + @TableName + '(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IPV4From] [bigint] NOT NULL,
	[IPV4To] [bigint] NOT NULL,
	[IPV4FromString] [nvarchar](30) NOT NULL,
	[IPV4ToString] [nvarchar](30) NOT NULL,
	[Company] [nvarchar](255) NULL,
	[LocationId] [uniqueidentifier] NULL,
	'  + [dbo].ShemaUtilsGetStandardColumns(1, 1) +   '
	 CONSTRAINT [PK_' + [dbo].[RuntimeUtilsGetPureTableName](@TableName) + '] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC 
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]';

	EXEC MergeUtilsExecSQL @S;	

END


GO 
MergeUtilsDropSPIfExist '[geopoints].[IPLocationApplyTableConstraints]';
GO 
MergeUtilsDropSPIfExist '[geopoints].[IPRangesApplyTableConstraints]';
GO

CREATE PROCEDURE [geopoints].[IPRangesApplyTableConstraints]
	@TableName NVARCHAR(128)	
AS
BEGIN
	DECLARE @S NVARCHAR(1024)
 	SET @S = 'CREATE NONCLUSTERED INDEX [IX_IPV4From_IPV4To] ON ' + @TableName + ' 
	(
		[IPV4From] ASC,
		[IPV4To] ASC
	);'
	EXEC MergeUtilsExecSQL @S;		
END

GO


DECLARE @tbl_IPLocation NVARCHAR(128);
DECLARE @s NVARCHAR(1024);
SET @tbl_IPLocation = dbo.RuntimeUtilsGetTableNameWithDate('[geopoints].IPRanges');
EXEC [geopoints].[IPRangesCreateTable] @tbl_IPLocation;

EXEC [geopoints].[IPRangesApplyTableConstraints] @tbl_IPLocation;

EXEC RuntimeUtilsSetViewDefinition '[geopoints].IPRanges_VIEW', @tbl_IPLocation;