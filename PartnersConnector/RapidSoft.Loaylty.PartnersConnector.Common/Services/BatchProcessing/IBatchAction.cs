namespace RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing
{
    internal interface IBatchAction
    {
        Batch Execute(Batch batch);
    }
}