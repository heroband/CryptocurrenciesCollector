using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models
{
    public class History
    {
        public decimal PriceUsd { get; init; }
        public required DateTimeOffset Time { get; init; }
        public string? CirculatingSupply { get; init; }
        public required string Date { get; init; }
    }
}
