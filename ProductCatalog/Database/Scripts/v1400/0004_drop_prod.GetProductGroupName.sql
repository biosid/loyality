if exists (select * from sys.objects where object_id = object_id(N'prod.GetProductGroupName') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
drop function prod.GetProductGroupName
