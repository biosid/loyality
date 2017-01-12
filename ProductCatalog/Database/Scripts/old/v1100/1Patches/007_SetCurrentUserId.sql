CREATE PROCEDURE [prod].[SetUserContext]
    @userName VARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @context VARBINARY(128)
    SET @context = CONVERT(BINARY(128), @userName)

    SET CONTEXT_INFO @context
END