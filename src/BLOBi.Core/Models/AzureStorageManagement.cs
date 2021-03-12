using Newtonsoft.Json;

namespace BLOBi.Core.Models
{
    [JsonObject("azureStorageManagement")]
    public class AzureStorageManagement
    {
        [JsonProperty("accountKey")]
        public string AccountKey { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        [JsonProperty("useManagedIdentity")]
        public bool UseManagedIdentity { get; set; }
    }
}
