SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

declare @createStatement nvarchar(max) = N'
CREATE FUNCTION [prod].[GetProductGroupNames]
(
    @productName nvarchar(255)
)
RETURNS @groupnames
TABLE ([name] nvarchar(255))
AS
BEGIN
    declare @length int = datalength(@productName),
            @startIndex int = 0,
            @endIndex int,
            @itemLength int

    while (@length > @startIndex)
    begin
        set @startIndex = charindex(N''"'', @productName, @startIndex)

        if (@startIndex is null or @startIndex <= 0)
            return

        set @endIndex = charindex(N''"'', @productName, @startIndex + 1)

        if (@endIndex is null or @endIndex <= 0)
            return

        set @itemLength = @endIndex - @startIndex - 1

        if (@itemLength > 0)
            insert @groupnames
            select substring(@productName, @startIndex + 1, @itemLength)

        set @startIndex = @endIndex + 1
    end
    return
END
'

if not exists (select * from sys.objects where object_id = object_id(N'prod.GetProductGroupNames') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
begin
    exec sp_executesql @statement = @createStatement
end
GO
