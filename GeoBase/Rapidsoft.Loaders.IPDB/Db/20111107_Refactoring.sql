MergeUtilsDropFuncIfExist '[Geopoints].[SearchLocationForIP]';
GO
CREATE FUNCTION [Geopoints].[SearchLocationForIP]
(
	@City       NVARCHAR(128),
	@Region     NVARCHAR(128),
	@FedRegion  NVARCHAR(128),
	@Country    NVARCHAR(128)
)
RETURNS UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @toponym NVARCHAR(32);
	DECLARE @toponymPosition INT;
	
	SELECT TOP 1 @toponym = NAME
	FROM   [geopoints].[LocationRegionToponymDic] d
	WHERE  CHARINDEX(LOWER(NAME), LOWER(@Region)) >= 1
	ORDER BY NAME DESC;
	
	IF (@toponym IS NOT NULL)
	BEGIN
	    SET @Region = REPLACE(@Region, @toponym, '');
	    SET @Region = LTRIM(RTRIM(@Region));
	END
	
	DECLARE @res UNIQUEIDENTIFIER;	
	
	SELECT TOP 1 @res = Id
	FROM   Geopoints.Location_VIEW loc
	WHERE  loc.RegionName = @Region
	       AND loc.Name = @City
	       AND loc.Toponym = N'ã';
	
	IF (@res IS NULL)
	    SELECT TOP 1 @res = Id
	    FROM   Geopoints.Location_VIEW loc
	    WHERE  loc.RegionName IS NULL
	           AND loc.Name = @City
	           AND loc.Toponym = N'ã';
	RETURN @res;
END
GO