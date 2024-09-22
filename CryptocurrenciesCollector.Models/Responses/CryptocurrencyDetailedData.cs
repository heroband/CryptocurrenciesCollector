using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Responses
{
    public class CryptocurrencyDetailedData
    {
        [JsonPropertyName("name")]
        public required string Name { get; init; }
        [JsonPropertyName("priceUsd")]
        public required string PriceUsd { get; init; }
        [JsonPropertyName("volumeUsd24Hr")]
        public required string? VolumeUsd24Hr { get; init; }
        [JsonPropertyName("changePercent24Hr")]
        public required string? ChangePercent24Hr { get; init; }
    }
}
