using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System;

namespace CryptocurrenciesCollector.Models.Responses
{
    public class AssetsWrap<T>
    {
        [JsonPropertyName("data")]
        public required T Data { get; set; }
        [JsonPropertyName("timestamp")]
        public required long Timestamp { get; set; }
    }
}
