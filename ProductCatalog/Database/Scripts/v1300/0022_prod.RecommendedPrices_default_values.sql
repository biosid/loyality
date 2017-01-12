delete from [prod].[RecommendedPrices]

insert into [prod].[RecommendedPrices]([Balance], [MinPrice], [MaxPrice])
values
    (2399.99, 0, 2000),
    (3399.99, 0, 2400),
    (4399.99, 800, 3400),
    (5499.99, 1400, 4400),
    (6499.99, 2000, 5400),
    (7499.99, 2500, 6400),
    (8999.99, 3000, 7400),
    (999999999, 4000, 999999999)
