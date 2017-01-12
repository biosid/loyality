update pxml
set Items.modify('insert sql:column("pxml.Prices") as last into (/ArrayOfOrderItem/OrderItem)[1]')
from
(
    select
      Id,
      Items,
      cast('<PriceRur>' + cast(PriceRur as varchar) + '</PriceRur>' +
           '<PriceBonus>' + cast(PriceBonus as varchar) + '</PriceBonus>' +
           '<AmountPriceRur>' + cast(AmountPriceRur as varchar) + '</AmountPriceRur>' +
           '<AmountPriceBonus>' + cast(AmountPriceBonus as varchar) + '</AmountPriceBonus>'
           as xml) as Prices
    from
    (
        select
          Id,
          Items,
          ItemsCost / Items.value('(/ArrayOfOrderItem/OrderItem/Amount)[1]', 'int') as PriceRur,
          BonusItemsCost / Items.value('(/ArrayOfOrderItem/OrderItem/Amount)[1]', 'int') as PriceBonus,
          ItemsCost as AmountPriceRur,
          BonusItemsCost as AmountPriceBonus
        from prod.Orders
        where
          Items.value('(/ArrayOfOrderItem/OrderItem/Amount)[1]', 'int') is not null
          and
          Items.value('(/ArrayOfOrderItem/OrderItem/Amount)[1]', 'int') > 0
          and
          Items.value('count(/ArrayOfOrderItem/OrderItem)', 'int') = 1
    ) p
) pxml
