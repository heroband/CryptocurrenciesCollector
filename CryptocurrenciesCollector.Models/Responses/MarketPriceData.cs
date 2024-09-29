using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Responses
{
    public class MarketPriceData
    {
        [JsonPropertyName("exchangeId")]
        public required string ExchangeId { get; init; }
        [JsonPropertyName("priceUsd")]
        public required string PriceUsd { get; init; }
    }
}
