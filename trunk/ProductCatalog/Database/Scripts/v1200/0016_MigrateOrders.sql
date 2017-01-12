--delete from [prod].orders where id = 1124
--GO
--set  IDENTITY_INSERT [prod].[Orders] on
--INSERT [prod].[Orders] ([Id], [ExternalOrderId], [PartnerId], [Status], [ExternalOrderStatusCode], [OrderStatusDescription], [ExternalOrderStatusDateTime], [StatusChangedDate], [StatusUtcChangedDate], [DeliveryInfo], [InsertedDate], [InsertedUtcDate], [UpdatedDate], [UpdatedUtcDate], [UpdatedUserId], [Items], [TotalWeight], [ItemsCost], [BonusItemsCost], [DeliveryCost], [BonusDeliveryCost], [TotalCost], [BonusTotalCost], [PaymentStatus], [DeliveryPaymentStatus], [CarrierId], [ClientId]) VALUES (1124, N'11501158-0002', 1, 50, NULL, N'', CAST(0x0000A2CB013D88E4 AS DateTime), CAST(0x0000A2D5017C21DD AS DateTime), CAST(0x0000A2D5013A36DA AS DateTime), N'<DeliveryInfo xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><CountryCode>RU</CountryCode><CountryName>Россия</CountryName><PostCode>119571</PostCode><AddressKladrCode>7700000000000</AddressKladrCode><RegionKladrCode>7700000000000</RegionKladrCode><RegionTitle>г. Москва</RegionTitle><StreetTitle>Проспект Вернадского</StreetTitle><House>92 корпус 1</House><Flat>36</Flat><AddressText>г. Москва, Проспект Вернадского, 92 корпус 1, 36</AddressText><DeliveryDate xsi:nil="true" /><DeliveryTimeFrom xsi:nil="true" /><DeliveryTimeTo xsi:nil="true" /><Comment>Будни с 18:00 до 22:00, выходные с 11:00 до 18:00</Comment><Contacts><Contact><FirstName>Михаил</FirstName><LastName>Рогальский</LastName><MiddleName>Львович</MiddleName><Phone><LocalNumber>1599426</LocalNumber><CityCode>916</CityCode><CountryCode>7</CountryCode></Phone><Email>mike@rogalsky.ru</Email></Contact></Contacts></DeliveryInfo>', CAST(0x0000A2CB013D5D26 AS DateTime), CAST(0x0000A2CB00FB7223 AS DateTime), CAST(0x0000A2CB013D5D26 AS DateTime), CAST(0x0000A2CB00FB7223 AS DateTime), N'vtbSystemUser', N'<ArrayOfOrderItem xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><OrderItem><Amount>1</Amount><BasketItemId>3e9fe1cb-153a-4b9c-b97c-2651c34962ef</BasketItemId><Product><ProductId>1_19681469</ProductId><PartnerProductId>19681469</PartnerProductId><PartnerId>1</PartnerId><Name>AppleTV медиаплеер</Name><CategoryId>743</CategoryId><CategoryName>Аудио-видео техника </CategoryName><CategoryNamePath>/Техника и связь/Гаджеты, мобильные телефоны/Аудио-видео техника /</CategoryNamePath><Status>Active</Status><ModerationStatus>Applied</ModerationStatus><StatusChangedDate>0001-01-01T00:00:00</StatusChangedDate><LastChangedDate>0001-01-01T00:00:00</LastChangedDate><Description>Медиаплеер AppleTV сочетает в себе множество развлечений. С Apple TV всё, что Вы хотели посмотреть — фильмы, в том числе и с разрешением 1080p, слайд-шоу из фотографий и многое другое — передаётся по беспроводной сети на ваш широкоэкранный телевизор. Управление хранением файлов не требуется. Синхронизация с медиатекой iTunes также не нужна. HD-фильмы из iTunes воспроизводятся через Интернет на вашем HD-телевизоре, а музыка и фотографии передаются с компьютера в потоковом режиме. Вам остаётся только нажать на кнопку и смотреть. Благодаря iCloud и Фотопотоку Ваши фотографии обновляются автоматически — их не нужно отправлять или синхронизировать. Apple TV работает тихо и экономично, и он настолько маленький, что уместится у Вас на ладони. А значит, его можно положить возле телевизора или на полку с дисками. А когда вы посмотрите все свои боевики, драмы и комедии, и дадите ему отдохнуть — он будет потреблять меньше энергии, чем ночной светильник. Интуитивно понятный интерфейс. ...</Description><Available>true</Available><CurrencyId>RUR</CurrencyId><Url>http://www.ozon.ru/context/detail/id/19681469/</Url><Pictures><string>http://vtb24.ozone.ru/multimedia/spare_covers/1005767815.jpg</string><string>http://vtb24.ozone.ru/multimedia/spare_other/1005767794.jpg</string></Pictures><Vendor>Apple</Vendor><VendorCode>MD199RU/A</VendorCode><Bid>0</Bid><CBid>0</CBid><Store>false</Store><Pickup>false</Pickup><Delivery>false</Delivery><LocalDeliveryCost>0.0000</LocalDeliveryCost><ManufacturerWarranty>false</ManufacturerWarranty><Downloadable>false</Downloadable><Barcode><string>0885909719266</string></Barcode><Param><ProductParam><Name>Тип</Name><Value>Электроника</Value></ProductParam><ProductParam><Name>Вес</Name><Unit>г</Unit><Value>485</Value></ProductParam><ProductParam><Name>Форматы файлов</Name><Value>AAC/eAAC, AVI, GIF, H.264, JPEG, MOV, MP3, MP4, MPEG4, TIFF, WAV</Value></ProductParam><ProductParam><Name>Количество разъемов HDMI</Name><Value>1</Value></ProductParam><ProductParam><Name>Дополнительно</Name><Value>Wi-Fi 802.11n</Value></ProductParam></Param><Year>0</Year><Volume>0</Volume><Part>0</Part><PageExtent>0</PageExtent><Days>0</Days><Date xsi:nil="true" /><IsPremiere>false</IsPremiere><IsKids>false</IsKids><Weight>485</Weight><InsertedDate>2014-02-04T19:30:48.107</InsertedDate><UpdatedDate>2014-02-07T18:30:38.07</UpdatedDate><PublicStatus>Available</PublicStatus><PublicStatusQuantity>Available</PublicStatusQuantity><PriceRUR>4700.0000</PriceRUR><PriceBase>15667</PriceBase><PriceAction>15667</PriceAction><PriceDeliveryRur>249.0000</PriceDeliveryRur><PriceDelivery>830</PriceDelivery><PriceDeliveryQuantity>830</PriceDeliveryQuantity><PriceDeliveryQuantityRur>249.0000</PriceDeliveryQuantityRur><PriceTotal>16497</PriceTotal><ExternalLocationId>2</ExternalLocationId><IsActionPrice>false</IsActionPrice><CarrierId>6</CarrierId><IsRecommended>false</IsRecommended></Product><PriceTotal>16497</PriceTotal><PriceRUR>4700</PriceRUR><FixedPrice><PriceAction>15667</PriceAction><PriceBase>15667</PriceBase><PriceRUR>4700</PriceRUR><FixDate>2014-02-07T19:13:59.0197049+04:00</FixDate></FixedPrice></OrderItem></ArrayOfOrderItem>', 485, 4700.0000, 15667.0000, 249.0000, 830.0000, 4949.0000, 16497.0000, 1, 1, 6, N'680ef397-9649-4814-b0b1-f2f5c9f3cccb')
--set  IDENTITY_INSERT [prod].[Orders] off
--Go

UPDATE t
set 
[DeliveryInfo].modify('
insert sql:column("t.Addr") as first into (/)[1]
')
from 
(select 
DeliveryInfo 
,CAST('<tmp>'+ CAST(DeliveryInfo.query(N'/DeliveryInfo') as nvarchar(max)) +'</tmp>' as xml) as Addr
from [prod].[Orders]) as t
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/*)
')
where DeliveryInfo is not null

-- Формируем DeliveryInfo.Address
UPDATE t
set 
[DeliveryInfo].modify('
insert sql:column("t.Addr") into (/DeliveryInfo)[1]
')
from 
(select 
DeliveryInfo 
,CAST('<Address>'+ CAST(DeliveryInfo.query(N'/tmp/DeliveryInfo/*') as nvarchar(max)) +'</Address>' as xml) as Addr
from [prod].[Orders]) as t
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/Contacts)
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/Comment)
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/CountryCode)
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/CountryName)
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/AddressKladrCode)
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/DeliveryDate)
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/DeliveryTimeFrom)
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/DeliveryInfo/Address/DeliveryTimeTo)
')
where DeliveryInfo is not null

-- Формируем DeliveryInfo
UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
insert <DeliveryType>Delivery</DeliveryType> as first into (/DeliveryInfo)[1]
')
where DeliveryInfo is not null

UPDATE t
set 
[DeliveryInfo].modify('
insert sql:column("t.Contact") into (/DeliveryInfo)[1]
')
from 
(select 
DeliveryInfo 
,DeliveryInfo.query(N'/tmp/DeliveryInfo/Contacts/Contact') as Contact
from [prod].[Orders]) as t
where DeliveryInfo is not null

UPDATE t
set 
[DeliveryInfo].modify('
insert sql:column("t.Comment") into (/DeliveryInfo)[1]
')
from 
(select 
DeliveryInfo 
,DeliveryInfo.query(N'/tmp/DeliveryInfo/Comment') as Comment
from [prod].[Orders]) as t
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set
[DeliveryInfo].modify('
insert <DeliveryVariantName>Курьерская доставка</DeliveryVariantName> into (/DeliveryInfo)[1]
')
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set
[DeliveryInfo].modify('
insert <DeliveryVariantsLocation><KladrCode></KladrCode></DeliveryVariantsLocation> into (/DeliveryInfo)[1]
')
where DeliveryInfo is not null

UPDATE t
set
[DeliveryInfo].modify('
insert sql:column("t.Kladr") into (/DeliveryInfo/DeliveryVariantsLocation/KladrCode)[1]
')
from
(select
DeliveryInfo
,DeliveryInfo.query(N'/tmp/DeliveryInfo/AddressKladrCode/text()') as Kladr
from [prod].[Orders]) as t
where DeliveryInfo is not null

UPDATE [prod].[Orders]
set 
[DeliveryInfo].modify('
delete (/tmp)
')
where DeliveryInfo is not null

--SELECT DeliveryInfo FROM [ProductCatalogDB].[prod].[Orders]
