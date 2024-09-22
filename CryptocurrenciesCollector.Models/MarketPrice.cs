using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models
{
    public class MarketPrice
    {
        public required string ExchangeId { get; set; }
        public decimal PriceUsd { get; set; }
    }
}
