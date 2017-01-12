create table [dbo].[OrderAttempts]
(
    [ClientId] nvarchar(36) NOT NULL,
    [InsertedDate] datetime2(7) NOT NULL,
    constraint PK_OrderAttempts primary key clustered
    (
        [ClientId]
    ) with (statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on)
) on [primary]

alter table [dbo].[OrderAttempts] add default (getdate()) for [InsertedDate]
