using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CryptocurrenciesCollector.Models;
using CryptocurrenciesCollector.Models.Extensions;
using CryptocurrenciesCollector.Models.Interfaces;
using CryptocurrenciesCollector.Models.Responses;


namespace CryptocurrenciesCollector.Services
{
    public class CryptocurrencyApiService : ICryptocurrencyApiService
    {
        private readonly HttpClient _httpClient;

        public CryptocurrencyApiService(string apiKey)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<Cryptocurrency> GetAssetById(string id)
        {
            var assetResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}");
            assetResponse.EnsureSuccessStatusCode();
            var assetJson = await assetResponse.Content.ReadAsStringAsync();
            var asset = JsonSerializer.Deserialize<Asset>(assetJson);

            var assetMarketsResponse = await _httpClient.GetAsync($"https://api.coincap.io/v2/assets/{id}/markets");
            assetMarketsResponse.EnsureSuccessStatusCode();
            var assetMarketsJson = await assetMarketsResponse.Content.ReadAsStringAsync();
            var assetMarkets = JsonSerializer.Deserialize<AssetMarket>(assetMarketsJson);

            return asset.ToCryptocurrency(assetMarkets);
        }

        public async Task<List<TopCryptocurrencies>> GetAssets( )
        {
            var assetsResponse = await _httpClient.GetAsync("https://api.coincap.io/v2/assets");
            assetsResponse.EnsureSuccessStatusCode();
            var assetsJson = await assetsResponse.Content.ReadAsStringAsync();
            var assets = JsonSerializer.Deserialize<Assets>(assetsJson);

            return assets.Data.Select(asset => new TopCryptocurrencies
            {
                Name = asset.Name,
                Rank = int.Parse(asset.Rank)
            }).ToList();
        }
    }
}
