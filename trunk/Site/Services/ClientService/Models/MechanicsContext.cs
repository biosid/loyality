using System.Collections.Generic;
using System.Linq;

namespace Vtb24.Site.Services.ClientService.Models
{
    public class MechanicsContext
    {
        public static implicit operator Dictionary<string,string>(MechanicsContext context)
        {
            var groups = context.ClientGroups == null || !context.ClientGroups.Any()
                ? "Standart"
                : string.Join(";", context.ClientGroups);

            return new Dictionary<string, string>
            {
                { "ClientProfile.KLADR", context.ClientLocationKladr },
                { "ClientProfile.Audiences", groups }
            };
        }

        /// <summary>
        /// Местоположение клиента
        /// </summary>
        public string ClientLocationKladr { get; set; }

        /// <summary>
        /// Группы, в которые входит клиент (ЦА и сегменты)
        /// </summary>
        public string[] ClientGroups { get; set; }
    }
}