WITH t AS
(
	select newid() [Id], 1 n, ':A' suffix union all
	select newid() [Id], 2 n, ':B' suffix union all
	select newid() [Id], 3 n, ':A' suffix union all
	select newid() [Id], 4 n, ':B' suffix union all
	select newid() [Id], 5 n, ':A' suffix union all
	select newid() [Id], 6 n, ':B' suffix union all
	select newid() [Id], 7 n, ':A' suffix union all
	select newid() [Id], 8 n, ':B' suffix union all
	select newid() [Id], 9 n, ':A' suffix union all
	select newid() [Id], 10 n, ':B' suffix
)
INSERT INTO [dbo].[AllDataTypesTable]
SELECT
	cast([Id] as uniqueidentifier) as [Id],
	null as [Null],
	cast(1 as bit) as [Boolean],
	cast(255 as tinyint) as [Byte],
	getdate() as [DateTime],
	79228162514264337593543950335 as [Decimal],
	100*1000*1000 + 0.123456789012345 as [Double],
	cast('00000000-0000-0000-0000-000000000000' as uniqueidentifier) as [Guid],
	cast(32767 as smallint) as [Int16],
	cast(2147483647 as int) - n as [Int32],
	cast(9223372036854775807 as bigint) as [Int64],
	10 * 1000 * 1000 + 0.12345 as [Single],
	N'Value' + suffix as [String],
	'F66DE1D7-D3B2-435F-A5E4-8B198D3B9F1D' as [EtlPackageId],
	'C7848C3B-38AB-485F-AB62-CDA9B6EA2F10' as [EtlSessionId],
	getdate(),
	getutcdate()
FROM
	t

GO