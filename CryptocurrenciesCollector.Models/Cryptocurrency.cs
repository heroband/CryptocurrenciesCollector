
using System.Diagnostics;

namespace CryptocurrenciesCollector.Models
{
    public class Cryptocurrency
    {
        public required string Name { get; init; } 
        public decimal Price { get; init; }
        public double Volume { get; init; }
        public decimal PriceChange { get; init; }
        public decimal AvailableIn { get; init; }
        public decimal MarketPrice { get; init; }
    }
}
