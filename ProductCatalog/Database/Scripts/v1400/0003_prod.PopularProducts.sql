if exists(select * from sys.columns where Name = N'GroupId' and object_id = object_id(N'prod.PopularProducts'))
begin
    alter table prod.PopularProducts
    drop column GroupId
end
