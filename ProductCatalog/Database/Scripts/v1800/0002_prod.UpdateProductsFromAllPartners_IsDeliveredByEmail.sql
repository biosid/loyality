if exists (select * from sys.objects where object_id = object_id(N'[prod].[UpdateProductsFromAllPartners]') and type in (N'P', N'PC'))
   drop procedure [prod].[UpdateProductsFromAllPartners]
go

create procedure [prod].[UpdateProductsFromAllPartners]
as
begin

-- ќбновление таблицы Products_FromAllPartners по сырым импортированным данным от партнЄров

-- —начала добавим новые строки
while (1=1)
begin
    insert into prod.ProductsFromAllPartners
    select top 1000 *
    from prod.Products
    where ProductId not in (select ProductId from prod.ProductsFromAllPartners)
    if (@@rowcount = 0)
        break;
end

-- “еперь удалим строки, которых больше нет в источнике
while (1=1)
begin
    delete top (1000) from prod.ProductsFromAllPartners
    where ProductId not in (select ProductId from prod.Products)
    if (@@rowcount = 0)
        break;
end

-- », наконец, обновим строки, которые есть и в источнике и в назначении, но источник был обновлен
while (1=1)
begin
    update top (1000) prod.ProductsFromAllPartners
    set
        [PartnerId] = src.[PartnerId],
        [InsertedDate] = src.[InsertedDate],
        [UpdatedDate] = src.[UpdatedDate],
        [PartnerProductId] = src.[PartnerProductId],
        [Type] = src.[Type],
        [Bid] = src.[Bid],
        [CBid] = src.[CBid],
        [Available] = src.[Available],
        [Name] = src.[Name],
        [Url] = src.[Url],
        [PriceRUR] = src.[PriceRUR],
        [CurrencyId] = src.[CurrencyId],
        [CategoryId] = src.[CategoryId],
        [Pictures] = src.[Pictures],
        [TypePrefix] = src.[TypePrefix],
        [Vendor] = src.[Vendor],
        [Model] = src.[Model],
        [Store] = src.[Store],
        [Pickup] = src.[Pickup],
        [Delivery] = src.[Delivery],
        [Description] = src.[Description],
        [VendorCode] = src.[VendorCode],
        [LocalDeliveryCost] = src.[LocalDeliveryCost],
        [SalesNotes] = src.[SalesNotes],
        [ManufacturerWarranty] = src.[ManufacturerWarranty],
        [CountryOfOrigin] = src.[CountryOfOrigin],
        [Downloadable] = src.[Downloadable],
        [Adult] = src.[Adult],
        [Barcode] = src.[Barcode],
        [Param] = src.[Param],
        [Author] = src.[Author],
        [Publisher] = src.[Publisher],
        [Series] = src.[Series],
        [Year] = src.[Year],
        [ISBN] = src.[ISBN],
        [Volume] = src.[Volume],
        [Part] = src.[Part],
        [Language] = src.[Language],
        [Binding] = src.[Binding],
        [PageExtent] = src.[PageExtent],
        [TableOfContents] = src.[TableOfContents],
        [PerformedBy] = src.[PerformedBy],
        [PerformanceType] = src.[PerformanceType],
        [Format] = src.[Format],
        [Storage] = src.[Storage],
        [RecordingLength] = src.[RecordingLength],
        [Artist] = src.[Artist],
        [Media] = src.[Media],
        [Starring] = src.[Starring],
        [Director] = src.[Director],
        [OriginalName] = src.[OriginalName],
        [Country] = src.[Country],
        [WorldRegion] = src.[WorldRegion],
        [Region] = src.[Region],
        [Days] = src.[Days],
        [DataTour] = src.[DataTour],
        [HotelStars] = src.[HotelStars],
        [Room] = src.[Room],
        [Meal] = src.[Meal],
        [Included] = src.[Included],
        [Transport] = src.[Transport],
        [Place] = src.[Place],
        [HallPlan] = src.[HallPlan],
        [Date] = src.[Date],
        [IsPremiere] = src.[IsPremiere],
        [IsKids] = src.[IsKids],
        [Status] = src.[Status],
        [ModerationStatus] = src.[ModerationStatus],
        [Weight] = src.[Weight],
        [UpdatedUserId] = src.[UpdatedUserId],
        [IsRecommended] = src.[IsRecommended],
        [BasePriceRUR] = src.[BasePriceRUR],
		[IsDeliveredByEmail] = src.[IsDeliveredByEmail]
    from prod.ProductsFromAllPartners tgt
    join prod.Products src on src.ProductId = tgt.ProductId
    where src.UpdatedDate > ISNULL(tgt.UpdatedDate, tgt.InsertedDate)

    if (@@rowcount = 0)
        break;
end

end
go
