IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CopyAllDataTypesTable]') AND type in (N'P'))
DROP PROCEDURE [dbo].[CopyAllDataTypesTable]

GO

CREATE PROCEDURE [dbo].[CopyAllDataTypesTable]
(
	@etlPackageId uniqueidentifier,
	@etlSessionId uniqueidentifier
)
AS
BEGIN

insert into [dbo].[AllDataTypesTableCopy]
(
	[Id],
	[Null],
	[Boolean],
	[Byte],
	[DateTime],
	[Decimal],
	[Double],
	[Guid],
	[Int16],
	[Int32],
	[Int64],
	[Single],
	[String],
	[EtlPackageId],
	[EtlSessionId],
	[EtlInsertedDateTime],
	[EtlInsertedUtcDateTime]
)
select
	[Id],
	[Null],
	[Boolean],
	[Byte],
	[DateTime],
	[Decimal],
	[Double],
	[Guid],
	[Int16],
	[Int32],
	[Int64],
	[Single],
	[String],
	@etlPackageId,
	@etlSessionId,
	getdate(),
	getutcdate()
from
	[dbo].[AllDataTypesTable]

select 1 as a

END

GO
