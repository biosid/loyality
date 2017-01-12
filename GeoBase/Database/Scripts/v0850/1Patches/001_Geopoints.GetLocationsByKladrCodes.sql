IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Geopoints].[GetLocationsByKladrCodes]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [Geopoints].[GetLocationsByKladrCodes]
GO

CREATE PROCEDURE [Geopoints].[GetLocationsByKladrCodes]
(
    @kladrCodes NVARCHAR(max)
)
AS
BEGIN
	SELECT DISTINCT * 
	FROM Geopoints.Location_VIEW l
	JOIN [dbo].[RuntimeUtilsSplitString](@kladrCodes, ',') c ON c.Substr = l.KladrCode
END