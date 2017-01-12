insert into prod.[PartnerSettings]
(
    [PartnerId],
    [Key],
    [Value]
)
select
    [PartnerId],
    'AdvancePaymentSupport' as [Key],
    case when [Value] = 'true' then 'Delivery' else 'None' end as [Value]
from prod.[PartnerSettings]
where [Key] = 'IsAdvancePaymentSupported'
