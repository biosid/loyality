namespace RapidSoft.Loaylty.ProductCatalog.ImportTests
{
    using System.IO;
    using System.Text;

    public static class Sqls
    {
        public static string InsertDeliveryRatesIfNeedFormat =
            @"IF NOT EXISTS (SELECT * FROM [prod].[DeliveryRates] WHERE [PartnerId] = {0})
BEGIN
	INSERT INTO [prod].[DeliveryLocations]
	([PartnerId],[LocationName],[Kladr],[Status],[InsertDateTime])
	VALUES
	({0},{1},{1},1,GETDATE())

	DECLARE @LocationId int 

	SELECT @LocationId = SCOPE_IDENTITY()

	INSERT INTO [prod].[DeliveryRates]
	([PartnerId],[MinWeightGram],[MaxWeightGram],[PriceRur],[LocationId])
	VALUES
	({0},0,10000000,100,@LocationId)
END";

        private static string InitCategoriesScriptFile = @"Script\Init Demo Categories.sql";

        public static string LoadInitCategoriesScript()
        {
            using (StreamReader reader = new StreamReader(InitCategoriesScriptFile, Encoding.UTF8, true))
            {
                string script = reader.ReadToEnd();
                return script;
            }
        }
    }
}
