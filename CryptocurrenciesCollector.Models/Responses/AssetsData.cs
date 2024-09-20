using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrenciesCollector.Models.Responses
{
    public class AssetsData
    {
        [JsonPropertyName("name")]
        public required string Name { get; init; }
        [JsonPropertyName("rank")]
        public required string Rank { get; init; }
    }
}
