namespace RapidSoft.Loaylty.ProductCatalog.ImportTests
{
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Helper
    {
        private static HttpListener listener;

        private static CancellationTokenSource tokenSource;

        private static CancellationToken token;

        private static Task task;

        public static string StartHttpListener(string dicPath, string fileName)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));

            var port = ((IPEndPoint)sock.LocalEndPoint).Port;
            sock.Close();

            var url = string.Format("http://localhost:{0}/", port);
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            task = Task.Factory.StartNew(
                () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        // NOTE: Если у вас в процессе debug выполнения упала след. строка - ЗАБЕЙ!
                        var context = listener.GetContext();
                        Task.Factory.StartNew(
                            ctx => WriteFile((HttpListenerContext)ctx, dicPath, fileName),
                            context,
                            TaskCreationOptions.LongRunning);
                    }
                },
                token);

            return url;
        }

        public static void StopHttpListener()
        {
            tokenSource.Cancel();

            listener.Stop();
        }

        static void WriteFile(HttpListenerContext ctx, string dicPath, string fileName)
        {
            var response = ctx.Response;

            var innerDicPath = dicPath;
            if (innerDicPath[innerDicPath.Length - 1] != '\\')
            {
                innerDicPath = innerDicPath + '\\';
            }

            using (FileStream fs = File.OpenRead(innerDicPath + fileName))
            {
                response.ContentLength64 = fs.Length;
                response.SendChunked = false;
                response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                response.AddHeader("Content-disposition", "attachment; filename=" + fileName);

                byte[] buffer = new byte[64 * 1024];
                using (BinaryWriter bw = new BinaryWriter(response.OutputStream))
                {
                    int read;
                    while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, read);
                        bw.Flush();
                    }

                    bw.Close();
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.OutputStream.Close();
            }
        }
    }
}