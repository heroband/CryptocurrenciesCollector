using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Responses
{
    public class HistoryData
    {
        [JsonPropertyName("priceUsd")]
        public required string PriceUsd { get; init; }
        [JsonPropertyName("time")]
        public required long Time { get; init; }
        [JsonPropertyName("circulatingSupply")]
        public string? CirculatingSupply { get; init; }
        [JsonPropertyName("date")]
        public required string Date { get; init; }
    }
}
