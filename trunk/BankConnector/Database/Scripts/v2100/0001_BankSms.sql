create table [dbo].BankSms
(
    [Id] bigint identity(1,1) not null,
    [TypeCode] int not null,
    [Phone] nvarchar(11) not null,
    [DisplayPhone] nvarchar(16) not null,
    [Password] nvarchar(20) null,
    [InsertedDate] datetime2(7) not null default (getdate()),
    [EtlSessionId] uniqueidentifier null
    constraint [PK_BankSms_Id] primary key clustered
    (
        [Id] asc
    )
    with
    (
        pad_index = off,
        statistics_norecompute = off,
        ignore_dup_key = off,
        allow_row_locks = on,
        allow_page_locks = on
    ) on [primary]
)
on [primary]
