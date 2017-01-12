using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Helpers
{
    public static class MetadataHelpers
    {
        public static string ToJson(Metadata[] metadata)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof (Metadata[]));
            using (var stream = new MemoryStream())
            {
                jsonSerializer.WriteObject(stream, metadata);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
