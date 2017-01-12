if exists (select * from sys.objects where object_id = object_id(N'[prod].[ProductsFromAllPartners]') and type in (N'U'))
   drop table [prod].[ProductsFromAllPartners]
go

create table [prod].[ProductsFromAllPartners]
(
    [ProductId] [nvarchar](256) not null,
    [PartnerId] [int] not null,
    [InsertedDate] [datetime] not null,
    [UpdatedDate] [datetime] null,
    [PartnerProductId] [nvarchar](256) not null,
    [Type] [nvarchar](20) null,
    [Bid] [int] null,
    [CBid] [int] null,
    [Available] [bit] null,
    [Name] [nvarchar](256) null,
    [Url] [nvarchar](256) null,
    [PriceRUR] [money] not null,
    [CurrencyId] [nchar](3) not null,
    [CategoryId] [int] not null,
    [Pictures] [xml] null,
    [TypePrefix] [nvarchar](50) null,
    [Vendor] [nvarchar](256) null,
    [Model] [nvarchar](256) null,
    [Store] [bit] null,
    [Pickup] [bit] null,
    [Delivery] [bit] null,
    [Description] [nvarchar](2000) null,
    [VendorCode] [nvarchar](256) null,
    [LocalDeliveryCost] [money] null,
    [SalesNotes] [nvarchar](50) null,
    [ManufacturerWarranty] [bit] null,
    [CountryOfOrigin] [nvarchar](256) null,
    [Downloadable] [bit] null,
    [Adult] [nchar](10) null,
    [Barcode] [xml] null,
    [Param] [xml] null,
    [Author] [nvarchar](256) null,
    [Publisher] [nvarchar](256) null,
    [Series] [nvarchar](256) null,
    [Year] [int] null,
    [ISBN] [nvarchar](256) null,
    [Volume] [int] null,
    [Part] [int] null,
    [Language] [nvarchar](50) null,
    [Binding] [nvarchar](50) null,
    [PageExtent] [int] null,
    [TableOfContents] [nvarchar](512) null,
    [PerformedBy] [nvarchar](50) null,
    [PerformanceType] [nvarchar](50) null,
    [Format] [nvarchar](50) null,
    [Storage] [nvarchar](50) null,
    [RecordingLength] [nvarchar](50) null,
    [Artist] [nvarchar](256) null,
    [Media] [nvarchar](50) null,
    [Starring] [nvarchar](256) null,
    [Director] [nvarchar](50) null,
    [OriginalName] [nvarchar](50) null,
    [Country] [nvarchar](50) null,
    [WorldRegion] [nvarchar](50) null,
    [Region] [nvarchar](50) null,
    [Days] [int] null,
    [DataTour] [nvarchar](50) null,
    [HotelStars] [nvarchar](50) null,
    [Room] [nchar](10) null,
    [Meal] [nchar](10) null,
    [Included] [nvarchar](256) null,
    [Transport] [nvarchar](256) null,
    [Place] [nvarchar](256) null,
    [HallPlan] [nvarchar](256) null,
    [Date] [datetime] null,
    [IsPremiere] [bit] null,
    [IsKids] [bit] null,
    [Status] [int] not null,
    [ModerationStatus] [int] not null,
    [Weight] [int] null,
    [UpdatedUserId] [nvarchar](50) null,
    [IsRecommended] [bit] not null,
    [BasePriceRUR] [money] null,
    constraint [PK_ProductsFromAllPartners] primary key clustered
    (
        [ProductId] asc
    )
    with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on)
    on [primary]
)
on [primary]
go

create nonclustered index [ProductsFromAllPartners_PartnerId_CategoryId]
on [prod].[ProductsFromAllPartners]
(
    [PartnerId] asc,
    [CategoryId] asc
)
with (statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, drop_existing = off, online = off, allow_row_locks = on, allow_page_locks = on)
on [primary]
go

create nonclustered index [ProductsFromAllPartners_Name]
on [prod].[ProductsFromAllPartners] 
(
    [Name] asc
)
with (statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, drop_existing = off, online = off, allow_row_locks = on, allow_page_locks = on)
on [primary]
go

create nonclustered index [ProductsFromAllPartners_CategoryId]
on [prod].[ProductsFromAllPartners] 
(
    [CategoryId] asc
)
with (statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, drop_existing = off, online = off, allow_row_locks = on, allow_page_locks = on)
on [primary]
go

if exists (select * from sys.indexes where object_id = object_id(N'[prod].[ProductCategories]') and name = N'IX_ProductCategories_ParentId')
drop index [IX_ProductCategories_ParentId] on [prod].[ProductCategories] with (online = off)
go

create nonclustered index [IX_ProductCategories_ParentId]
on [prod].[ProductCategories]
(
    [ParentId] ASC
)
with (statistics_norecompute = off, sort_in_tempdb = off, ignore_dup_key = off, drop_existing = off, online = off, allow_row_locks = on, allow_page_locks = on)
on [primary]
go
