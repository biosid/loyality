namespace RapidSoft.Loaylty.PartnersConnector.HttpHandlers
{
	using System.IO;
	using System.Web;

	/// <summary>
	/// Содержит методы-расширители для класса <see cref="HttpResponse"/>.
	/// </summary>
	internal static class HttpHelpers
	{
		/// <summary>
		/// Формирование ответа 400.
		/// </summary>
		/// <param name="response">The response.</param>
		public static void BadRequestResponse(this HttpResponse response)
		{
			response.InitResponse(400);
		}

		/// <summary>
		/// Формирование ответа 200.
		/// </summary>
		/// <param name="response">The response.</param>
		public static void OkResponse(this HttpResponse response)
		{
			InitResponse(response, 200);
		}

		/// <summary>
		/// Формирование ответа 500.
		/// </summary>
		/// <param name="response">The response.</param>
		public static void InternalServerErrorResponse(this HttpResponse response)
		{
			InitResponse(response, 500);
		}
		
		public static void OkXmlResponse(this HttpResponse response, string body)
		{
			response.ContentType = "text/xml";
			response.Write(body);
		}

		/// <summary>
		/// Читает тело запроса.
		/// </summary>
		/// <param name="request">
		/// Полученный запрос.
		/// </param>
		/// <returns>
		/// Строковое представление тела запроса.
		/// </returns>
		public static string ReadBody(this HttpRequest request)
		{
			request.InputStream.Position = 0;
			using (var ms = new MemoryStream(request.BinaryRead(request.ContentLength)))
			{
				ms.Position = 0;
				using (var sr = new StreamReader(ms))
				{
					return sr.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Формирование ответа с заданым кодом.
		/// </summary>
		/// <param name="response">
		/// The response.
		/// </param>
		/// <param name="code">
		/// Код ответа.
		/// </param>
		private static void InitResponse(this HttpResponse response, int code)
		{
			response.ContentType = "text/plain";
			response.StatusCode = code;
		}
	}
}