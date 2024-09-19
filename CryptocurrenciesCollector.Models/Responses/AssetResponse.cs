using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Responses
{
    public class AssetResponse
    {
        [JsonPropertyName("data")]
        public required AssetDataResponse Data { get; set; }
        [JsonPropertyName("timestamp")]
        public required long Timestamp { get; set; }

    }
}
