MergeUtilsDropSPIfExist '[geopoints].[TradePointsGetByLocation]';
GO

CREATE PROCEDURE [geopoints].[TradePointsGetByLocation]
(
	@LocationId uniqueidentifier, 
    @locale VARCHAR(2),    
    @skip INT = NULL,
    @top INT = NULL
)
AS
BEGIN
	DECLARE @default_top INT SET @default_top = 1000;

	SET @skip = ISNULL(@skip, 0);
	SET @top = ISNULL(@top, @default_top);

	SELECT * FROM [Geopoints].V_TradePoint t	
	WHERE t.Name  in (
	
					(SELECT Name FROM (
                            SELECT 
                                    ROW_NUMBER() OVER (ORDER BY t2.Name ASC) AS rownumber,
                                    t2.Name
                            FROM
                                    [Geopoints].V_TradePoint AS t2 
                            INNER JOIN [Geopoints].V_Location vl ON vl.Id=t2.LocationId
                            WHERE vl.LocationType = 2 OR vl.LocationType = 3
                            ) AS sub1 
                            WHERE sub1.rownumber > @skip AND sub1.rownumber <= @skip + @top 
                    )	
	) ORDER BY Name	
END
GO
