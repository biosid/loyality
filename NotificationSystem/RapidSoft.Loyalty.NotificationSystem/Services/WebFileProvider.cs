namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    using System.Web.Hosting;

    public class WebFileProvider : IFileProvider
    {
        public string MapPath(string filePath)
        {
            return HostingEnvironment.MapPath(filePath);
        }
    }
}
