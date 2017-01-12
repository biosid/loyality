

insert into geopoints.v_location
(
id,
locationtype,
name,
address,
createddatetime,
createdutcdatetime,
modifieddatetime,
modifiedutcdatetime
)
values
(
'6f661444-deae-4318-ae35-e149f322fc0b',
0,
N'Россия',
N'Россия',
getdate(),
getutcdate(),
getdate(),
getutcdate()
)

update geopoints.v_location set parentid='6f661444-deae-4318-ae35-e149f322fc0b'
where locationtype=1

select * from geopoints.v_location where locationtype=1