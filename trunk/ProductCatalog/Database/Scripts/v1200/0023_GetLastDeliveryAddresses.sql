/****** Object:  StoredProcedure [prod].[GetLastDeliveryAddresses]    Script Date: 03/06/2014 18:06:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [prod].[GetLastDeliveryAddresses]
    @clientId [nvarchar](255),
    @excludeAddressesWithoutKladr [int] = null,
    @countToTake [int] = null
AS
BEGIN
    set @countToTake = ISNULL(@countToTake, 20)

    select o.DeliveryInfo as DeliveryInfo
    from (
        select max(Id) as Id
        from (
            select
                Id,
                DeliveryInfo.value('(/DeliveryInfo/DeliveryType)[1]', 'nvarchar(256)') as DeliveryType,
                DeliveryInfo.value('(/DeliveryInfo/DeliveryVariantsLocation/KladrCode)[1]', 'nvarchar(256)') as AddressKladr,
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/CountryCode)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/CountryName)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/PostCode)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/RegionTitle)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/DistrictTitle)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/CityTitle)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/TownTitle)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/StreetTitle)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/House)[1]', 'nvarchar(256)'), '') +
                isnull(DeliveryInfo.value('(/DeliveryInfo/Address/Flat)[1]', 'nvarchar(256)'), '') as Addr
            from prod.Orders
            where ClientId = @clientId) a
        where
            a.DeliveryType = 'Delivery' and
            (@excludeAddressesWithoutKladr <> 1 or a.AddressKladr is not null)
        group by a.Addr) g inner join prod.Orders o on g.Id = o.Id

END
