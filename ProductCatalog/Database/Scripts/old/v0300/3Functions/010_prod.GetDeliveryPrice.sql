-- NOTE: Есть подозрение что функция не нужна.
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[prod].[GetDeliveryPrice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [prod].[GetDeliveryPrice]
END
GO