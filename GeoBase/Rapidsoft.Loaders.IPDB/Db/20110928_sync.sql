

GO
MergeUtilsDropFuncIfExist '[geopoints].[LocationSearchForIP]';
GO
CREATE FUNCTION [geopoints].[LocationSearchForIP]
(
	@City NVARCHAR(128), 	
	@Region NVARCHAR(128), 
	@FedRegion NVARCHAR(128),
	@Country NVARCHAR(128)
)
RETURNS uniqueidentifier
AS
BEGIN

	DECLARE @toponym NVARCHAR(32);
	DECLARE @toponymPosition INT;

	SELECT TOP 1 @toponym = Name FROM [geopoints].[LocationRegionToponymDic] d
	WHERE CHARINDEX(LOWER(NAME), LOWER(@Region)) >= 1
	ORDER BY Name DESC;

	IF(@toponym IS NOT NULL)
	BEGIN

		SET @Region = REPLACE(@Region, @toponym, '');
		SET @Region = LTrim(RTrim(@Region));
	END

	DECLARE @res uniqueidentifier;	

	SELECT TOP 1 @res = Id FROM Geopoints.V_Location loc
	WHERE loc.RegionName = @Region AND loc.Name = @City AND loc.Toponym = N'ã';

	IF(@res IS NULL)
		SELECT TOP 1 @res = Id FROM Geopoints.V_Location loc
		WHERE loc.RegionName IS NULL AND loc.Name = @City AND loc.Toponym = N'ã';
	

	return @res;
END
GO
