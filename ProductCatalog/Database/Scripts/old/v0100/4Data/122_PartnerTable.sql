
CREATE TABLE [prod].[Products_1_00000000_000000](
	[ProductId] [nvarchar](256) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[PartnerProductId] [nvarchar](256) NOT NULL,
	[Type] [nvarchar](20) NULL,
	[Bid] [int] NULL,
	[CBid] [int] NULL,
	[Available] [bit] NULL,
	[Name] [nvarchar](256) NULL,
	[Url] [nvarchar](256) NULL,
	[PriceRUR] [money] NOT NULL,
	[CurrencyId] [nchar](3) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Pictures] [xml] NULL,
	[TypePrefix] [nvarchar](50) NULL,
	[Vendor] [nvarchar](256) NULL,
	[Model] [nvarchar](256) NULL,
	[Store] [bit] NULL,
	[Pickup] [bit] NULL,
	[Delivery] [bit] NULL,
	[Description] [nvarchar](512) NULL,
	[VendorCode] [nvarchar](256) NULL,
	[LocalDeliveryCost] [money] NULL,
	[SalesNotes] [nvarchar](50) NULL,
	[ManufacturerWarranty] [bit] NULL,
	[CountryOfOrigin] [nvarchar](256) NULL,
	[Downloadable] [bit] NULL,
	[Adult] [nchar](10) NULL,
	[Barcode] [xml] NULL,
	[Param] [xml] NULL,
	[Author] [nvarchar](256) NULL,
	[Publisher] [nvarchar](256) NULL,
	[Series] [nvarchar](256) NULL,
	[Year] [int] NULL,
	[ISBN] [nvarchar](256) NULL,
	[Volume] [int] NULL,
	[Part] [int] NULL,
	[Language] [nvarchar](50) NULL,
	[Binding] [nvarchar](50) NULL,
	[PageExtent] [int] NULL,
	[TableOfContents] [nvarchar](512) NULL,
	[PerformedBy] [nvarchar](50) NULL,
	[PerformanceType] [nvarchar](50) NULL,
	[Format] [nvarchar](50) NULL,
	[Storage] [nvarchar](50) NULL,
	[RecordingLength] [nvarchar](50) NULL,
	[Artist] [nvarchar](256) NULL,
	[Media] [nvarchar](50) NULL,
	[Starring] [nvarchar](256) NULL,
	[Director] [nvarchar](50) NULL,
	[OriginalName] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[WorldRegion] [nvarchar](50) NULL,
	[Region] [nvarchar](50) NULL,
	[Days] [int] NULL,
	[DataTour] [nvarchar](50) NULL,
	[HotelStars] [nvarchar](50) NULL,
	[Room] [nchar](10) NULL,
	[Meal] [nchar](10) NULL,
	[Included] [nvarchar](256) NULL,
	[Transport] [nvarchar](256) NULL,
	[Place] [nvarchar](256) NULL,
	[HallPlan] [nvarchar](256) NULL,
	[Date] [datetime] NULL,
	[IsPremiere] [bit] NULL,
	[IsKids] [bit] NULL,
	[Status] [int] NOT NULL,
	[ModerationStatus] [int] NOT NULL,
	[Weight] [int] NULL,
	[UpdatedUserId] [nvarchar](50) NULL,
 CONSTRAINT [PK_Products_1_00000000_000000] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[PartnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [prod].[Products_1_00000000_000000] ADD  CONSTRAINT [DF_Products_InsertedDate_1_00000000_000000]  DEFAULT (getdate()) FOR [InsertedDate]
GO

ALTER TABLE [prod].[Products_1_00000000_000000] ADD  CONSTRAINT [DF_Products_UpdatedDate_1_00000000_000000]  DEFAULT (getdate()) FOR [UpdatedDate]
GO

ALTER TABLE [prod].[Products_1_00000000_000000]  WITH NOCHECK ADD  CONSTRAINT [FK_Products_1_00000000_000000_ProductCategories] FOREIGN KEY([CategoryId])
REFERENCES [prod].[ProductCategories] ([Id])
GO

ALTER TABLE [prod].[Products_1_00000000_000000] CHECK CONSTRAINT [FK_Products_1_00000000_000000_ProductCategories]
GO

ALTER TABLE [prod].[Products_1_00000000_000000]  WITH CHECK ADD  CONSTRAINT [CK_Products_1_00000000_000000] CHECK  (([PartnerId]=(1)))
GO

ALTER TABLE [prod].[Products_1_00000000_000000] CHECK CONSTRAINT [CK_Products_1_00000000_000000]
GO


INSERT INTO [prod].[PartnerProductCatalogs]
           ([PartnerId]
           ,[Key]
           ,[IsActive]
           ,[InsertedDate]
           ,[UpdatedDate])
     VALUES
           (1
           ,'1_00000000_000000'
           ,1
           ,getdate()
           ,getdate())
GO

DROP VIEW [prod].[Products]
GO

CREATE VIEW [prod].[Products]
AS                        
SELECT * FROM [prod].[Products_1_00000000_000000]
GO
