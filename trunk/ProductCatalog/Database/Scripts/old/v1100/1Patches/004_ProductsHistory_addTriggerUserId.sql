BEGIN TRANSACTION
GO
CREATE TABLE prod.Tmp_ProductsHistory
	(
	Action char(1) NOT NULL,
	TriggerUserId nvarchar(255) NULL,
	TriggerDate datetime NOT NULL,
	TriggerUtcDate datetime NOT NULL,
	ProductId nvarchar(256) NULL,
	PartnerId int NULL,
	InsertedDate datetime NULL,
	UpdatedDate datetime NULL,
	PartnerProductId nvarchar(256) NULL,
	Type nvarchar(20) NULL,
	Bid int NULL,
	CBid int NULL,
	Available bit NULL,
	Name nvarchar(256) NULL,
	Url nvarchar(256) NULL,
	PriceRUR money NOT NULL,
	CurrencyId nchar(3) NULL,
	CategoryId int NULL,
	Pictures xml NULL,
	TypePrefix nvarchar(50) NULL,
	Vendor nvarchar(256) NULL,
	Model nvarchar(256) NULL,
	Store bit NULL,
	Pickup bit NULL,
	Delivery bit NULL,
	Description nvarchar(2000) NULL,
	VendorCode nvarchar(256) NULL,
	LocalDeliveryCost money NULL,
	SalesNotes nvarchar(50) NULL,
	ManufacturerWarranty bit NULL,
	CountryOfOrigin nvarchar(256) NULL,
	Downloadable bit NULL,
	Adult nchar(10) NULL,
	Barcode xml NULL,
	Param xml NULL,
	Author nvarchar(256) NULL,
	Publisher nvarchar(256) NULL,
	Series nvarchar(256) NULL,
	Year int NULL,
	ISBN nvarchar(256) NULL,
	Volume int NULL,
	Part int NULL,
	Language nvarchar(50) NULL,
	Binding nvarchar(50) NULL,
	PageExtent int NULL,
	TableOfContents nvarchar(512) NULL,
	PerformedBy nvarchar(50) NULL,
	PerformanceType nvarchar(50) NULL,
	Format nvarchar(50) NULL,
	Storage nvarchar(50) NULL,
	RecordingLength nvarchar(50) NULL,
	Artist nvarchar(256) NULL,
	Media nvarchar(50) NULL,
	Starring nvarchar(256) NULL,
	Director nvarchar(50) NULL,
	OriginalName nvarchar(50) NULL,
	Country nvarchar(50) NULL,
	WorldRegion nvarchar(50) NULL,
	Region nvarchar(50) NULL,
	Days int NULL,
	DataTour nvarchar(50) NULL,
	HotelStars nvarchar(50) NULL,
	Room nchar(10) NULL,
	Meal nchar(10) NULL,
	Included nvarchar(256) NULL,
	Transport nvarchar(256) NULL,
	Place nvarchar(256) NULL,
	HallPlan nvarchar(256) NULL,
	Date datetime NULL,
	IsPremiere bit NULL,
	IsKids bit NULL,
	Status int NULL,
	ModerationStatus int NULL,
	Weight int NULL,
	UpdatedUserId nvarchar(50) NULL,
	IsRecommended bit NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE prod.Tmp_ProductsHistory SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM prod.ProductsHistory)
	 EXEC('INSERT INTO prod.Tmp_ProductsHistory (Action, TriggerDate, TriggerUtcDate, ProductId, PartnerId, InsertedDate, UpdatedDate, PartnerProductId, Type, Bid, CBid, Available, Name, Url, PriceRUR, CurrencyId, CategoryId, Pictures, TypePrefix, Vendor, Model, Store, Pickup, Delivery, Description, VendorCode, LocalDeliveryCost, SalesNotes, ManufacturerWarranty, CountryOfOrigin, Downloadable, Adult, Barcode, Param, Author, Publisher, Series, Year, ISBN, Volume, Part, Language, Binding, PageExtent, TableOfContents, PerformedBy, PerformanceType, Format, Storage, RecordingLength, Artist, Media, Starring, Director, OriginalName, Country, WorldRegion, Region, Days, DataTour, HotelStars, Room, Meal, Included, Transport, Place, HallPlan, Date, IsPremiere, IsKids, Status, ModerationStatus, Weight, UpdatedUserId, IsRecommended)
		SELECT Action, TriggerDate, TriggerUtcDate, ProductId, PartnerId, InsertedDate, UpdatedDate, PartnerProductId, Type, Bid, CBid, Available, Name, Url, PriceRUR, CurrencyId, CategoryId, Pictures, TypePrefix, Vendor, Model, Store, Pickup, Delivery, Description, VendorCode, LocalDeliveryCost, SalesNotes, ManufacturerWarranty, CountryOfOrigin, Downloadable, Adult, Barcode, Param, Author, Publisher, Series, Year, ISBN, Volume, Part, Language, Binding, PageExtent, TableOfContents, PerformedBy, PerformanceType, Format, Storage, RecordingLength, Artist, Media, Starring, Director, OriginalName, Country, WorldRegion, Region, Days, DataTour, HotelStars, Room, Meal, Included, Transport, Place, HallPlan, Date, IsPremiere, IsKids, Status, ModerationStatus, Weight, UpdatedUserId, IsRecommended FROM prod.ProductsHistory WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE prod.ProductsHistory
GO
EXECUTE sp_rename N'prod.Tmp_ProductsHistory', N'ProductsHistory', 'OBJECT' 
GO
COMMIT
