
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace CryptocurrenciesCollector.Models
{
    public class CryptocurrencyDetailedInfo
    {
        public required string Name { get; init; }
        public decimal PriceUsd { get; init; }
        public decimal VolumeUsd24Hr { get; init; }
        public decimal ChangePercent24Hr { get; init; }
        public List<MarketPrice>? Markets { get; set; }
    }
}
