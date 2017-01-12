create table [dbo].[UnitellerPayments]
(
    [OrderId] int NOT NULL,
    [BillNumber] nvarchar(12) NOT NULL,
    [InsertedDate] datetime2(7) NOT NULL,
    constraint PK_UnitellerPayments primary key clustered
    (
        [OrderId]
    ) with (statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on)
) on [primary]

alter table [dbo].[UnitellerPayments] add default (getdate()) for [InsertedDate]
