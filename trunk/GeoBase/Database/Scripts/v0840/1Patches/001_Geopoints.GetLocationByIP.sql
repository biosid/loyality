IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[GetLocationByIP]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [Geopoints].[GetLocationByIP]
GO

CREATE PROCEDURE [Geopoints].[GetLocationByIP]
(
    @ipINT         BIGINT
)
AS
BEGIN
	SELECT TOP 1 L.*
	FROM   Geopoints.Location_VIEW L
	INNER JOIN Geopoints.IPRanges_VIEW r
		ON  L.Id = r.LocationId
	WHERE  @ipINT BETWEEN IPV4From AND IPV4To
END
GO


