  DECLARE @tbl_IPRanges NVARCHAR(128);
  SET @tbl_IPRanges = dbo.RuntimeUtilsGetTableNameWithDate('[geopoints].IPRanges');
  EXEC [geopoints].[IPRangesCreateTable] @tbl_IPRanges;
  EXEC RuntimeUtilsSetViewDefinition '[geopoints].IPRanges_VIEW', @tbl_IPRanges;