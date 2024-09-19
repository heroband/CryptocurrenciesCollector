
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace CryptocurrenciesCollector.Models
{
    public class Cryptocurrency
    {
        public required string Name { get; init; }
        public decimal PriceUsd { get; init; }
        public decimal VolumeUsd24Hr { get; init; }
        public decimal ChangePercent24Hr { get; init; }
        public List<Market>? Markets { get; set; }
    }
}
