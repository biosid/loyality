namespace Rapidsoft.Loyalty.NotificationSystem.Services.Templates
{
    /// <summary>
    /// параметры шаблона письма оповещения клиента
    /// </summary>
    public partial class ClientMessageNotificationBody
    {
        public string ThreadUrl { get; set; }

        public string UnsubscribeUrl { get; set; }
    }
}