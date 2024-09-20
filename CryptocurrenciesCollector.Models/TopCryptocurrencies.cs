using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models
{
    public class TopCryptocurrencies
    {
        public required string Name { get; init; }
        public required int Rank { get; init; }
    }
}
