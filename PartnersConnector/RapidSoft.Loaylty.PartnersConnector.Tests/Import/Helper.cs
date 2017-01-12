namespace RapidSoft.Loaylty.PartnersConnector.Tests.Import
{
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Quartz;
    using Quartz.Impl;

    public static class Helper
    {
        private static HttpListener listener;

        private static bool work;

        public static IScheduler GetScheduler()
        {
            var shedulerFactory = new StdSchedulerFactory();
            return shedulerFactory.GetScheduler();
        }

        public static string StartHttpListener()
        {
            var freeport = FreeTcpPort();
            var url = string.Format("http://localhost:{0}/", freeport);
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            work = true;
            Task.Factory.StartNew(
                () =>
                    {
                        while (work)
                        {
                            var context = listener.GetContext();
                            Task.Factory.StartNew(
                                ctx => WriteFile((HttpListenerContext)ctx), context, TaskCreationOptions.LongRunning);
                        }
                    },
                TaskCreationOptions.LongRunning);

            return url;
        }

        public static void StopHttpListener()
        {
            listener.Stop();
            work = false;
            listener = null;
        }

        private static void WriteFile(HttpListenerContext ctx)
        {
            var response = ctx.Response;
            
            //response is HttpListenerContext.Response...
            response.ContentLength64 = 1024;
            response.SendChunked = false;
            response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
            response.AddHeader("Content-disposition", "attachment; filename=file.file");

            var buffer = new byte[1024];
            using (var bw = new BinaryWriter(response.OutputStream))
            {
                bw.Write(buffer, 0, 1024);
                bw.Flush();

                bw.Close();
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.OutputStream.Close();
        }

        private static int FreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}