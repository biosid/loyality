MergeUtilsDropSPIfExist '[Geopoints].[IPRangesGetLocationsByIP]';
GO
--���� ������ ��� LocationType 1 (������, �����) � 3,4 ( ����� � ���������� �����)
--��� ��� � ���� ���������� ������ ������
CREATE PROCEDURE [Geopoints].[IPRangesGetLocationsByIP]
	@ipINT BIGINT,
	@locationType INT,
	@locale VARCHAR(2),    
    @skip INT = NULL,
    @top INT = NULL
AS
BEGIN

	DECLARE @S NVARCHAR(2048)

	DECLARE @rootLocationId uniqueIdentifier;
	DECLARE @rootType INT;

	SELECT @rootLocationId = r.LocationId, @rootType = L.LocationType FROM Geopoints.IPRanges_VIEW r
	INNER JOIN Geopoints.V_Location	L ON L.Id = r.LocationId
	WHERE IPV4From <= @ipINT AND @ipINT <= IPV4To;

	DECLARE @default_top INT SET @default_top = 1000;
	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);


	IF(@rootType = @locationType)
		BEGIN
			-- ���������� ������� �������. ���� skip > 0 ��� Top == 0 �� ���������� ������
			SELECT * FROM Geopoints.V_Location
			WHERE Id = @rootLocationId AND (@skip = 0 AND @top > 0);

		END
		--���� ���� ������� ������� ������� ����. �������� ��� ������ ������� ������
	ELSE IF(@rootType > @locationType)
		BEGIN			
			DECLARE @ResLocationId uniqueIdentifier;			

			IF(@locationType = 0)
				SELECT @ResLocationId = t.CountryId FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 1)
				SELECT @ResLocationId = t.RegionID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 2)
				SELECT @ResLocationId = t.DistrictID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 3)
				SELECT @ResLocationId = t.CityID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;
			ELSE IF (@locationType = 4)
				SELECT @ResLocationId = t.TownID FROM Geopoints.V_Location t WHERE t.Id = @rootLocationId;

			SELECT * FROM Geopoints.V_Location t 
			WHERE t.Id = @ResLocationId AND (@skip = 0 AND @top > 0);

		END
	ELSE
		BEGIN
			print @rootType
			--Root=������, ������ + �����. ������ �� RegionID
			IF(@rootType = 1)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND RegionID = @rootLocationId
					)
					SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									intLocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
						) ORDER BY Name
				END
			--Root=�����. ������ �� DistrictID
			ELSE IF(@rootType = 2)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND DistrictID = @rootLocationId
					)
					SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									intLocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
						) ORDER BY Name					

				END
			--Root=�����. ������ �� CityID
			ELSE IF(@rootType = 3)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND CityID = @rootLocationId
					)
					SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									intLocationTreeWithData AS t2  
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
						) ORDER BY Name
				END
			--Root=�������. ������ �� TownID
			ELSE IF(@rootType = 4)
				BEGIN
					WITH intLocationTreeWithData AS 
					(
						SELECT Id,Name FROM Geopoints.V_Location
						WHERE LocationType = @locationType AND TownID = @rootLocationId
					)
					SELECT * FROM Geopoints.V_Location t WHERE t.Id in (
							SELECT Id FROM 
							(
							SELECT 
									ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS rownumber, t2.Id
							FROM
									intLocationTreeWithData AS t2                                                                                 
							) 
							AS sub1 
							WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
						) ORDER BY Name
				END
			

		END

END
GO