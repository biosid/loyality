delete from [prod].[RecommendedPrices]

insert into [prod].[RecommendedPrices]([Balance], [MinPrice], [MaxPrice])
values
    (2399.99, 0, 2000),
    (3399.99, 0, 2400),
    (4399.99, 0, 3400),
    (5499.99, 0, 4400),
    (6499.99, 0, 5400),
    (7499.99, 0, 6400),
    (8999.99, 0, 7400),
    (999999999, 3400, 999999999)
