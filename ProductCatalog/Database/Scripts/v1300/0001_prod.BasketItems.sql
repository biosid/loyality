if not exists(select * from sys.columns 
            where Name = N'BasketItemGroupId' and Object_ID = Object_ID(N'prod.BasketItems'))
begin
	ALTER TABLE prod.BasketItems 
	ADD [BasketItemGroupId] [int] NULL
end
