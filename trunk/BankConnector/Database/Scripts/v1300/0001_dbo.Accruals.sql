if exists(select * from sys.columns where Name = N'ExternalId' and Object_ID = Object_ID(N'Accruals'))
begin
    alter table dbo.Accruals
    alter column ExternalId nvarchar(40) null
end
