if exists (select * from sys.objects where object_id = object_id(N'[connect].[LitresDownloadCodes]') and type in (N'U'))
    drop table [connect].[LitresDownloadCodes]
go

create table [connect].[LitresDownloadCodes]
(
    [Id] bigint identity(1,1) not null,
    [PartnerProductId] nvarchar(256) not null,
    [Code] nvarchar(256) not null,
    [InsertedDate] datetime default (getdate()) not null,
    [OrderId] int null,
    constraint [PK_LitresDownloadCodes] primary key clustered ([Id] asc)
)
on [primary]
go

create nonclustered index [IX_LitresDownloadCodes.PartnerProductId]
on [connect].[LitresDownloadCodes] ([PartnerProductId])
include ([OrderId])
go
