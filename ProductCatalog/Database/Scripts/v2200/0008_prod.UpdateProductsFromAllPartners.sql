
/****** Object:  StoredProcedure [prod].[UpdateProductsFromAllPartners]    Script Date: 03/20/2015 16:07:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[UpdateProductsFromAllPartners]') AND type in (N'P', N'PC'))
DROP PROCEDURE [prod].[UpdateProductsFromAllPartners]
GO

/****** Object:  StoredProcedure [prod].[UpdateProductsFromAllPartners]    Script Date: 03/20/2015 16:07:50 ******/
CREATE procedure [prod].[UpdateProductsFromAllPartners]
as
begin

MERGE prod.ProductsFromAllPartners AS t
USING prod.Products AS s
ON (s.ProductId = t.ProductId)
WHEN MATCHED AND s.UpdatedDate > t.UpdatedDate
    THEN UPDATE SET 
    t.[PartnerId] = s.[PartnerId],
    t.[InsertedDate] = s.[InsertedDate],
    t.[UpdatedDate] = s.[UpdatedDate],
    t.[PartnerProductId] = s.[PartnerProductId],
    t.[Type] = s.[Type],
    t.[Bid] = s.[Bid],
    t.[CBid] = s.[CBid],
    t.[Available] = s.[Available],
    t.[Name] = s.[Name],
    t.[Url] = s.[Url],
    t.[PriceRUR] = s.[PriceRUR],
    t.[CurrencyId] = s.[CurrencyId],
    t.[CategoryId] = s.[CategoryId],
    t.[Pictures] = s.[Pictures],
    t.[TypePrefix] = s.[TypePrefix],
    t.[Vendor] = s.[Vendor],
    t.[Model] = s.[Model],
    t.[Store] = s.[Store],
    t.[Pickup] = s.[Pickup],
    t.[Delivery] = s.[Delivery],
    t.[Description] = s.[Description],
    t.[VendorCode] = s.[VendorCode],
    t.[LocalDeliveryCost] = s.[LocalDeliveryCost],
    t.[SalesNotes] = s.[SalesNotes],
    t.[ManufacturerWarranty] = s.[ManufacturerWarranty],
    t.[CountryOfOrigin] = s.[CountryOfOrigin],
    t.[Downloadable] = s.[Downloadable],
    t.[Adult] = s.[Adult],
    t.[Barcode] = s.[Barcode],
    t.[Param] = s.[Param],
    t.[Author] = s.[Author],
    t.[Publisher] = s.[Publisher],
    t.[Series] = s.[Series],
    t.[Year] = s.[Year],
    t.[ISBN] = s.[ISBN],
    t.[Volume] = s.[Volume],
    t.[Part] = s.[Part],
    t.[Language] = s.[Language],
    t.[Binding] = s.[Binding],
    t.[PageExtent] = s.[PageExtent],
    t.[TableOfContents] = s.[TableOfContents],
    t.[PerformedBy] = s.[PerformedBy],
    t.[PerformanceType] = s.[PerformanceType],
    t.[Format] = s.[Format],
    t.[Storage] = s.[Storage],
    t.[RecordingLength] = s.[RecordingLength],
    t.[Artist] = s.[Artist],
    t.[Media] = s.[Media],
    t.[Starring] = s.[Starring],
    t.[Director] = s.[Director],
    t.[OriginalName] = s.[OriginalName],
    t.[Country] = s.[Country],
    t.[WorldRegion] = s.[WorldRegion],
    t.[Region] = s.[Region],
    t.[Days] = s.[Days],
    t.[DataTour] = s.[DataTour],
    t.[HotelStars] = s.[HotelStars],
    t.[Room] = s.[Room],
    t.[Meal] = s.[Meal],
    t.[Included] = s.[Included],
    t.[Transport] = s.[Transport],
    t.[Place] = s.[Place],
    t.[HallPlan] = s.[HallPlan],
    t.[Date] = s.[Date],
    t.[IsPremiere] = s.[IsPremiere],
    t.[IsKids] = s.[IsKids],
    t.[Status] = s.[Status],
    t.[ModerationStatus] = s.[ModerationStatus],
    t.[Weight] = s.[Weight],
    t.[UpdatedUserId] = s.[UpdatedUserId],
    t.[IsRecommended] = s.[IsRecommended],
    t.[BasePriceRUR] = s.[BasePriceRUR],
	t.[IsDeliveredByEmail] = s.[IsDeliveredByEmail]
WHEN NOT MATCHED THEN
    INSERT ([ProductId]
           ,[PartnerId]
           ,[InsertedDate]
           ,[UpdatedDate]
           ,[PartnerProductId]
           ,[Type]
           ,[Bid]
           ,[CBid]
           ,[Available]
           ,[Name]
           ,[Url]
           ,[PriceRUR]
           ,[CurrencyId]
           ,[CategoryId]
           ,[Pictures]
           ,[TypePrefix]
           ,[Vendor]
           ,[Model]
           ,[Store]
           ,[Pickup]
           ,[Delivery]
           ,[Description]
           ,[VendorCode]
           ,[LocalDeliveryCost]
           ,[SalesNotes]
           ,[ManufacturerWarranty]
           ,[CountryOfOrigin]
           ,[Downloadable]
           ,[Adult]
           ,[Barcode]
           ,[Param]
           ,[Author]
           ,[Publisher]
           ,[Series]
           ,[Year]
           ,[ISBN]
           ,[Volume]
           ,[Part]
           ,[Language]
           ,[Binding]
           ,[PageExtent]
           ,[TableOfContents]
           ,[PerformedBy]
           ,[PerformanceType]
           ,[Format]
           ,[Storage]
           ,[RecordingLength]
           ,[Artist]
           ,[Media]
           ,[Starring]
           ,[Director]
           ,[OriginalName]
           ,[Country]
           ,[WorldRegion]
           ,[Region]
           ,[Days]
           ,[DataTour]
           ,[HotelStars]
           ,[Room]
           ,[Meal]
           ,[Included]
           ,[Transport]
           ,[Place]
           ,[HallPlan]
           ,[Date]
           ,[IsPremiere]
           ,[IsKids]
           ,[Status]
           ,[ModerationStatus]
           ,[Weight]
           ,[UpdatedUserId]
           ,[IsRecommended]
           ,[BasePriceRUR]
           ,[IsDeliveredByEmail])
        VALUES (s.[ProductId]
           ,s.[PartnerId]
           ,s.[InsertedDate]
           ,s.[UpdatedDate]
           ,s.[PartnerProductId]
           ,s.[Type]
           ,s.[Bid]
           ,s.[CBid]
           ,s.[Available]
           ,s.[Name]
           ,s.[Url]
           ,s.[PriceRUR]
           ,s.[CurrencyId]
           ,s.[CategoryId]
           ,s.[Pictures]
           ,s.[TypePrefix]
           ,s.[Vendor]
           ,s.[Model]
           ,s.[Store]
           ,s.[Pickup]
           ,s.[Delivery]
           ,s.[Description]
           ,s.[VendorCode]
           ,s.[LocalDeliveryCost]
           ,s.[SalesNotes]
           ,s.[ManufacturerWarranty]
           ,s.[CountryOfOrigin]
           ,s.[Downloadable]
           ,s.[Adult]
           ,s.[Barcode]
           ,s.[Param]
           ,s.[Author]
           ,s.[Publisher]
           ,s.[Series]
           ,s.[Year]
           ,s.[ISBN]
           ,s.[Volume]
           ,s.[Part]
           ,s.[Language]
           ,s.[Binding]
           ,s.[PageExtent]
           ,s.[TableOfContents]
           ,s.[PerformedBy]
           ,s.[PerformanceType]
           ,s.[Format]
           ,s.[Storage]
           ,s.[RecordingLength]
           ,s.[Artist]
           ,s.[Media]
           ,s.[Starring]
           ,s.[Director]
           ,s.[OriginalName]
           ,s.[Country]
           ,s.[WorldRegion]
           ,s.[Region]
           ,s.[Days]
           ,s.[DataTour]
           ,s.[HotelStars]
           ,s.[Room]
           ,s.[Meal]
           ,s.[Included]
           ,s.[Transport]
           ,s.[Place]
           ,s.[HallPlan]
           ,s.[Date]
           ,s.[IsPremiere]
           ,s.[IsKids]
           ,s.[Status]
           ,s.[ModerationStatus]
           ,s.[Weight]
           ,s.[UpdatedUserId]
           ,s.[IsRecommended]
           ,s.[BasePriceRUR]
           ,s.[IsDeliveredByEmail])
WHEN NOT MATCHED BY SOURCE THEN DELETE;

end

GO