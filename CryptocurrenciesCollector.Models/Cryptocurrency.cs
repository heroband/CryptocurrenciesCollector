using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models
{
    public class Cryptocurrency
    {
        public required string Id { get; init; }
        public required int Rank { get; init; }
        public required string Name { get; init; }
        public decimal PriceUsd { get; init; }
        public decimal ChangePercent24Hr { get; init; }

    }
}
