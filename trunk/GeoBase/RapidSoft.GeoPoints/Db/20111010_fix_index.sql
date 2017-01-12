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
	)INCLUDE ( [IPV4FromString],
	[IPV4ToString],
	[Company],
	[LocationId],
	[CreatedDateTime],
	[CreatedUtcDateTime],
	[ModifiedDateTime],
	[ModifiedUtcDateTime],
	[EtlPackageId],
	[EtlSessionId]) ;'
	EXEC MergeUtilsExecSQL @S;		
END

GO